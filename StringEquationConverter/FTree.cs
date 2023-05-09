using StringEquationConverter.FExceptions;
using StringEquationConverter.NodeTypes;
using StringEquationConverter.NodeTypes.Operators;
using StringEquationConverter.NodeTypes.Operators.BinaryOperators;
using StringEquationConverter.NodeTypes.Operators.UnaryOperators;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        //the super class FuncContainer is used to store the values
        //that are needed at the end of the function reading.
        private class FuncContainer
        {
            public readonly string FuncName;
            public readonly int StartBN;
            public readonly Stack<ContainerNode> ArgsCont;
            public readonly ContainerNode ParentCont;
            public readonly FuncContainer? ParentFuncCont;

            public FuncContainer(string funcName, int startBN, ContainerNode parent, FuncContainer? parentFuncCont)
            {
                FuncName = funcName;
                StartBN = startBN;
                ArgsCont = new Stack<ContainerNode>();
                ParentCont = parent;
                ParentFuncCont = parentFuncCont;
            }

            public ContainerNode Next()
            {
                ArgsCont.Push(new ContainerNode());
                return ArgsCont.Peek();
            }

            public FunctionNode GetNode()
            {
                var i = FFunction.IndexOf(FuncName, ArgsCont.Count);
                if(i == -1)
                    throw new FInvalidOperationException($"{FuncName}: function with {ArgsCont.Count} arguments was not found.");
                return new FunctionNode(FFunction.Funcs[i], ArgsCont.Select(i =>
                {
                    if (i.Node is null)
                        throw new FInvalidOperationException($"{FuncName}: syntax error.");
                    return i;
                }).Reverse().ToArray());
            }
        }

        private void Rebuild()
        {
            if (string.IsNullOrWhiteSpace(Function))
                throw new FArgmentException($"'{nameof(Function)}' is empty.");

            ref string s = ref function;

            StringBuilder valueBuilder = new StringBuilder();
            ContainerNode curr_cont = new ContainerNode();
            TreeNode? tempValue = null;
            FuncContainer? tempFuncCont = null;
            int bN = 0;

            Node = curr_cont;

            bool contains() => valueBuilder.IsBuildedValue() || tempValue is not null;
            void addN()
            {
                if (valueBuilder.IsBuildedValue())
                {
                    curr_cont.AddNode(GetNode(valueBuilder));
                }
                if (tempValue is not null)
                {
                    curr_cont.AddNode(tempValue);
                    tempValue = null;
                }
            }
            void enterFunc()
            {
                valueBuilder.Clear();
                curr_cont = tempFuncCont.Next();
                curr_cont.ParentContainer = curr_cont;
            }
            void leaveFunc()
            {
                addN();
                curr_cont = tempFuncCont.ParentCont;
                tempValue = tempFuncCont.GetNode();
                tempFuncCont = tempFuncCont.ParentFuncCont;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                    continue;

                if (s[i] == '.' || char.IsDigit(s[i]))
                {
                    valueBuilder.Append(s[i]);
                }
                else if(char.IsLetter(s[i]) || s[i] == '_')
                {
                    //if the variable is preceded by a number - implicit multiplication (e.g. 2a => 2*a)
                    if (double.TryParse(valueBuilder.ToString(), out var _))
                    {
                        curr_cont.AddNode(new Multiply());
                        curr_cont.AddNode(GetNode(valueBuilder));
                    }
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
                }
                else if (s[i].IsBinaryOperator())
                {
                    switch (s[i])
                    {
                        case '-':
                            if (!contains())//if it is an implicit multiplication by -1 (e.g. -a)
                                curr_cont.AddNode(new Negative());
                            else//if it is a binary subtraction operation (e.g. a-b)
                                curr_cont.AddNode(new Subtract());
                            break;
                        case '+':
                            //if + is used as a binary operator (e.g. a+b, not +b)
                            if (contains())
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
                }
                else if (s[i] == '(')
                {
                    bN++;
                    if (FFunction.Contains(valueBuilder.ToString()))//if this container is a function call
                    {
                        tempFuncCont = new FuncContainer(valueBuilder.ToString(), bN-1, curr_cont, tempFuncCont);
                        enterFunc();
                        continue;
                    }
                    if (tempValue is not null)
                    {
                        curr_cont.AddNode(tempValue);
                        tempValue = null;
                    }

                    if (valueBuilder.IsBuildedValue())//x(...) => x*(...)
                    {
                        curr_cont.AddNode(new Multiply());
                        curr_cont.AddNode(GetNode(valueBuilder));
                    }
                    else if (i > 0 && s[i - 1] == ')')//(...)(...) => (...)*(...)
                    {
                        curr_cont.AddNode(new Multiply());
                    }
                    curr_cont = new ContainerNode(curr_cont);
                }
                else if (s[i] == ')')
                {
                    bN--;
                    if (tempFuncCont is not null && tempFuncCont.StartBN == bN)
                    {
                        leaveFunc();
                    }
                    else
                    {
                        if (valueBuilder.IsBuildedValue())
                            curr_cont.AddNode(GetNode(valueBuilder));
                        //return from nested container
                        if (curr_cont.ParentContainer is not null)
                        {
                            if (tempValue is not null)
                            {
                                curr_cont.AddNode(tempValue);
                            }
                            tempValue = curr_cont;
                            curr_cont = curr_cont.ParentContainer;
                        }
                    }
                }
                else if (s[i] == ',')
                {
                    if(tempFuncCont is null)
                        throw new FInvalidOperationException(i, $"{s[i]}: Syntax error.");
                    addN();
                    curr_cont = tempFuncCont.Next();
                }
                else if (s[i] == '|')//abs
                {
                    if(tempFuncCont is null)
                    {
                        if (valueBuilder.IsBuildedValue())//x|...| => x*|...|
                        {
                            curr_cont.AddNode(new Multiply());
                            curr_cont.AddNode(GetNode(valueBuilder));
                        }
                        tempFuncCont = new FuncContainer("abs", bN, curr_cont, tempFuncCont);
                        enterFunc();
                        bN++;
                    }
                    else
                    {
                        bN--;
                        leaveFunc();
                    }
                }
                else
                    throw new FInvalidOperationException(i, $"'{s[i]}': Syntax error.");

                if (i == s.Length - 1)
                {
                    addN();
                }
            }

            var sMpf = Node?.Simplify();
            if (sMpf is not null)
                Node = sMpf;
        }

        private static IFHValue GetNode(StringBuilder str)
        {
            var strVal = str.ToString();
            str.Clear();
            if (double.TryParse(strVal, out var n))
            {
                return new FFraction(n, null);
            }
            else if(strVal.ToLower() == "x")
            {
                return FVariable.X;
            }
            else//variable
            {
                var cl = FVariable.UserVars.Where(i => i.Name == strVal).FirstOrDefault();
                if (cl != null)
                    return cl;
            }
            throw new FArgmentException($"Invalid value: '{strVal}'.");
        }
    }
}
