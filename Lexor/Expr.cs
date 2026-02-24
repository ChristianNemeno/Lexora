using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexor
{
    abstract class Expr
    {
        
        public interface IVisitor<R>
        {

        }

        public abstract R Accept <R>(IVisitor<R> visitor);
        public class Assign : Expr
        {
            private Token Name;
            private Expr value;

            public Assign(Token name, Expr value)
            {
                Name = name;
                this.value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                throw new NotImplementedException();
            }


            public class Binary : Expr
            {
                private Expr Left;
                private Token Operator;
                private Expr Right;

                public Binary(Expr left, Token op, Expr right)
                {
                    Left = left;
                    Operator = op;
                    Right = right;
                }

                public override R Accept <R> (IVisitor<R> visitor)
                {
                    throw new NotImplementedException();
                }

            }
            




        }

    }
}
