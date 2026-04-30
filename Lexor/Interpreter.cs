using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexor
{
    class Interpreter : Expr.IVisitor<object?>, Stmt.IVisitor<object?>
    {
        private Environment environment = new Environment();

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Lexora.RuntimeError(error);
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private object? Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        public object? VisitPrintStmt(Stmt.Print stmt)
        {
            object? value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object? VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        public object? VisitDeclareStmt(Stmt.Declare stmt)
        {
            foreach (var variable in stmt.Variables)
            {
                object? value = null;
                if (variable.Initializer != null)
                {
                    value = Evaluate(variable.Initializer);
                    CheckType(variable.Name, stmt.DataType.Type, value);
                }
                environment.Define(variable.Name.Lexeme!, stmt.DataType.Type, value);
            }
            return null;
        }
        public object? VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object? VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object? VisitUnaryExpr(Expr.Unary expr)
        {
            object? right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Minus:
                    CheckNumberOperand(expr.Operator, right);
                    return -(Convert.ToDouble(right));
                case TokenType.Not:
                    return !IsTruthy(right);
            }
            return null;
        }

        public object? VisitVariableExpr(Expr.Variable expr)
        {
            return environment.Get(expr.Name);
        }

        public object? VisitAssignExpr(Expr.Assign expr)
        {
            object? value = Evaluate(expr.Value);
            TokenType declaredType = environment.GetType(expr.Name)!.Value;
            CheckType(expr.Name, declaredType, value);
            environment.Assign(expr.Name, value);
            return value;
        }

        public object? VisitBinaryExpr(Expr.Binary expr)
        {
            object? left = Evaluate(expr.Left);
            object? right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.Plus:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) + Convert.ToDouble(right);
                case TokenType.Minus:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) - Convert.ToDouble(right);
                case TokenType.Star:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) * Convert.ToDouble(right);
                case TokenType.Slash:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) / Convert.ToDouble(right);
                case TokenType.Mod:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) % Convert.ToDouble(right);

                case TokenType.Ampersand:
                    return Stringify(left) + Stringify(right);

                case TokenType.Greater:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) > Convert.ToDouble(right);
                case TokenType.Less:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) < Convert.ToDouble(right);
                case TokenType.GreaterEqual:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) >= Convert.ToDouble(right);
                case TokenType.LessEqual:
                    CheckNumberOperands(expr.Operator, left, right);
                    return Convert.ToDouble(left) <= Convert.ToDouble(right);

                case TokenType.NotEqual: return !IsEqual(left, right);
                case TokenType.DoubleEqual: return IsEqual(left, right);
            }

            return null;
        }



        public object? VisitBlockStmt(Stmt.Block stmt) => throw new NotImplementedException();
        public object? VisitIfStmt(Stmt.If stmt) => throw new NotImplementedException();
        public object? VisitForStmt(Stmt.For stmt) => throw new NotImplementedException();
        public object? VisitRepeatStmt(Stmt.Repeat stmt) => throw new NotImplementedException();
        public object? VisitScanStmt(Stmt.Scan stmt) => throw new NotImplementedException();

        private void CheckType(Token name, TokenType declaredType, object? value)
        {
            if (value == null) return;

            switch (declaredType)
            {
                case TokenType.Int:
                    if (value is int) return;
                    if (value is double d && d == Math.Floor(d)) return;
                    if (value is float f && f == Math.Floor(f)) return;
                    throw new RuntimeError(name, $"Type mismatch: expected INT, got {TypeName(value)}.");
                case TokenType.Float:
                    if (value is double || value is float || value is int) return;
                    throw new RuntimeError(name, $"Type mismatch: expected FLOAT, got {TypeName(value)}.");
                case TokenType.Bool:
                    if (value is bool) return;
                    throw new RuntimeError(name, $"Type mismatch: expected BOOL, got {TypeName(value)}.");
                case TokenType.Char:
                    if (value is char) return;
                    throw new RuntimeError(name, $"Type mismatch: expected CHAR, got {TypeName(value)}.");
            }
        }

        private string TypeName(object? value)
        {
            if (value == null) return "NULL";
            if (value is int) return "INT";
            if (value is double) return "FLOAT";
            if (value is float) return "FLOAT";
            if (value is bool) return "BOOL";
            if (value is char) return "CHAR";
            if (value is string) return "STRING";
            return value.GetType().Name;
        }

        private void CheckNumberOperand(Token op, object? operand)
        {
            if (operand is double || operand is int || operand is float) return;
            throw new RuntimeError(op, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token op, object? left, object? right)
        {
            if ((left is double || left is int || left is float) &&
                (right is double || right is int || right is float)) return;
            throw new RuntimeError(op, "Operands must be numbers.");
        }

        private bool IsTruthy(object? obj)
        {
            if (obj == null) return false;
            if (obj is bool b) return b;
            return true;
        }

        private bool IsEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private string Stringify(object? obj)
        {
            if (obj == null) return "null";
            if (obj is bool b) return b ? "TRUE" : "FALSE";

            if (obj is double d)
            {
                string text = d.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }
            return obj.ToString()!;
        }
    }
}
