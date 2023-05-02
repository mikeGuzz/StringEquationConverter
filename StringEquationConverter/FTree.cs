using StringEquationConverter.FExceptions;
using StringEquationConverter.NodeTypes;
using StringEquationConverter.NodeTypes.Operators;
using StringEquationConverter.NodeTypes.Operators.BinaryOperators;
using StringEquationConverter.NodeTypes.Operators.UnaryOperators;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StringEquationConverter
{
    public sealed class FTree : ContainerNode
    {
        private string function;

        public string Function => function;

        public FTree(string function) : base()
        {
            this.function = function;
            Rebuild();
        }

        public FTree(string function, ContainerNode parent) : base(parent)
        {
            this.function = function;
            Rebuild();
        }

        public void RebuildTree(string function)
        {
            this.function = function;
            Rebuild();
        }

        private void Rebuild()
        {
            if (string.IsNullOrWhiteSpace(Function))
                throw new FArgmentException($"'{nameof(Function)}' is empty.");

            ref string s = ref function;
            StringBuilder valueBuilder = new StringBuilder();
            ContainerNode curr_cont = new ContainerNode();
            TreeNode? temp = null;

            //если программа ожидает на операнд
            bool nOperand = false;

            var chForN = () =>
            {
                if (nOperand)
                    throw new FInvalidOperationException("Operator operand missing.");
            };

            var addN = () =>
            {
                var flag = false;
                if (valueBuilder.IsBuildedValue())
                {
                    curr_cont.AddNode(GetNode(valueBuilder));
                    flag = true;
                }
                if (temp is not null)
                {
                    curr_cont.AddNode(temp);
                    temp = null;
                    flag = true;
                }

                if(!flag)
                    chForN();
            };

            Node = curr_cont;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                    continue;

                if (s[i] == '.' || char.IsDigit(s[i]))
                {
                    valueBuilder.Append(s[i]);
                }
                else if (s[i].IsUnaryOperator())
                {
                    switch (s[i])
                    {
                        case '%':
                            curr_cont.AddNode(new Percentage());
                            break;
                        default:
                            throw new FInvalidOperationException(i, $"Invalid operation: '{s[i]}'.");
                    }
                    //addN();
                    //chForN();
                    nOperand = false;
                }
                else if (s[i].IsBinaryOperator())
                {
                    //chForN();
                    switch (s[i])
                    {
                        case '-':
                            //curr_cont.AddNode(new Negative());
                            curr_cont.AddNode(new Subtract());
                            break;
                        case '+':
                            curr_cont.AddNode(new Add());
                            break;
                        case '*':
                            curr_cont.AddNode(new Multiply());
                            break;
                        case '/':
                            curr_cont.AddNode(new Divide());
                            break;
                        case '^':
                            curr_cont.AddNode(new Power());
                            break;
                        default:
                            throw new FInvalidOperationException(i, $"Invalid operation: '{s[i]}'.");
                    }
                    addN();
                    nOperand = true;
                }
                else if (s[i] == '(')
                {
                    if (temp is not null)
                    {
                        curr_cont.AddNode(temp);
                        temp = null;
                    }
                    var cont = new ContainerNode(curr_cont);

                    //valueBuilder.IsBuildedValue() -> if the previous node is not an operator
                    //(e.g. 'x(...)' => 'x*(...)' or '(...)(...)' => '(...)*(...)')
                    if (valueBuilder.IsBuildedValue())
                    {
                        curr_cont.AddNode(new Multiply());
                        curr_cont.AddNode(GetNode(valueBuilder));
                    }
                    else if (i > 0 && s[i - 1] == ')')
                    {
                        curr_cont.AddNode(new Multiply());
                    }
                    curr_cont = cont;
                }
                else if (s[i] == ')')
                {
                    if (valueBuilder.IsBuildedValue())
                        curr_cont.AddNode(GetNode(valueBuilder));
                    else if (curr_cont.IsEmpty())
                        throw new FInvalidOperationException(i, "Empty container.");
                    //return from nested container
                    if (curr_cont.ParentContainer is not null)
                    {
                        if (temp is not null)
                        {
                            curr_cont.AddNode(temp);
                        }
                        temp = curr_cont;
                        curr_cont = curr_cont.ParentContainer;
                    }
                }

                if (i == s.Length - 1)
                {
                    addN();
                }
            }
        }

        private TreeNode GetNode(StringBuilder str)
        {
            if (double.TryParse(str.ToString(), out var n))
            {
                str.Clear();
                return new FractionNode(new FFraction(n, null));
            }
            else
                throw new FArgmentException($"Invalid value: '{str}'.");
        }

        //private void AddNode(ContainerNode cont, TreeNode node)
        //{
        //    if (node is UnaryOperator)
        //        throw new InvalidOperationException(nameof(node));
        //    if (!cont.Nodes.Any())//if the node is the first element in the collection
        //    {
        //        cont.Nodes.Add(node);
        //        return;
        //    }
        //    if (cont.Nodes.Last() is UnaryOperator unaryOp)//if the last is an operator
        //    {
        //        if (unaryOp.IsEmpty())
        //        {
        //            if (unaryOp is BinaryOperator binaryOp)
        //            {
        //                if (binaryOp.LeftOperand is null)
        //                    throw new FInvalidOperationException($"Left operand is empty.");
        //                binaryOp.RightOperand = node;
        //            }
        //            else
        //            {
        //                unaryOp.LeftOperand = cont;
        //            }
        //            return;
        //        }
        //        else
        //        {
        //            throw new FInvalidOperationException("Action not found.");
        //        }
        //    }
        //}
    }
}
