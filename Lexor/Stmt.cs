using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Lexor.Expr;

namespace Lexor
{
    /// <summary>
    /// Expr and Stmt , theyre really important , 
    /// simpy Stmt is something na mo do ug action 
    /// Expr on the other hand something naay value 
    /// </summary>
    abstract class Stmt
    {
        public interface IVisitor<R>
        {
            R VisitBlockStmt(Block stmt);
            R VisitDeclareStmt(Declare stmt);
            R VisitExpressionStmt(Expression stmt);
            R VisitIfStmt(If stmt);
            R VisitForStmt(For stmt);
            R VisitPrintStmt(Print stmt);
            R VisitRepeatStmt(Repeat stmt);
            R VisitScanStmt(Scan stmt);

        }

        public abstract R Accept<R>(IVisitor<R> visitor);

        public class Block : Stmt
        {
            public readonly List<Stmt> Statements;

            public Block (List<Stmt> statements)
            {
                this.Statements = statements;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitBlockStmt(this);
            }
        }
        
        public class Expression : Stmt
        {
            public readonly Expr expression;
            public Expression(Expr expression)
            {
                this.expression = expression;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }
        }

        public class Print : Stmt
        {
            public readonly Expr Expr;
            public Print(Expr expr) { 
                this.Expr = expr;            
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }
        }


        // this is a helper class because of the left to right
        // possible many variables in 1 assignment or declaration
        public class Declarator
        {
            public readonly Token Name;
            public readonly Expr? Initializer;

            public Declarator(Token name, Expr? initializer)
            {
                Name = name;
                Initializer = initializer;
            }
        }
        public class Declare : Stmt
        {
            public readonly Token DataType;
            public readonly List<Declarator> Variables;

            public Declare(Token datatype, List<Declarator> variables)
            {
                this.DataType= datatype;
                this.Variables = variables;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitDeclareStmt(this);
            }
        }

        public class If : Stmt
        {
            public readonly Expr Condition;
            public readonly Stmt ThenBranch;
            public readonly Stmt ElseBranch;

            public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
            {
                Condition = condition;
                ThenBranch = thenBranch;
                ElseBranch = elseBranch;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitIfStmt(this);
            }
        }

        public class For : Stmt {
            public readonly Stmt? Initialization;
            public readonly Expr? Condition;
            public readonly Expr? Update;

            public readonly Stmt Body;

            public For(Stmt initialization, Expr? condition, Expr update, Stmt body)
            {
                Initialization = initialization;
                Condition = condition;
                Update = update; 
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitForStmt(this);
            }
        }

        public class Scan : Stmt
        {
            public readonly List<Token> Names;

            public Scan(List<Token> names)
            {
                Names = names;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitScanStmt(this);
            }
        }

        public class Repeat : Stmt
        {
            public readonly Expr Condition;
            public readonly Stmt Body;

            public Repeat(Expr condition, Stmt body)
            {
                Condition = condition;
                Body = body;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitRepeatStmt(this);
            }
        }
    }
}
