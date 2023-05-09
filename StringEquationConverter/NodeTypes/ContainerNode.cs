using StringEquationConverter.FExceptions;
using StringEquationConverter.NodeTypes.Operators;
using StringEquationConverter.NodeTypes.Operators.BinaryOperators;
using StringEquationConverter.NodeTypes.Operators.UnaryOperators;
using StringEquationConverter.ValueTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace StringEquationConverter.NodeTypes
{
    public class ContainerNode : TreeNode
    {
        public IFHValue? Node { get; set; }
        public ContainerNode? ParentContainer { get; set; }

        public ContainerNode() { }

        public ContainerNode(ContainerNode? parent)
        {
            ParentContainer = parent;
        }

        public ContainerNode(ContainerNode parent, TreeNode? node)
        {
            ParentContainer = parent;
            Node = node;
        }

        public bool IsEmpty() => Node == null;

        public virtual void AddNode(IFHValue subNode)
        {
            if (Node is null)
            {
                Node = subNode;
                return;
            }

            if (Node is BinaryOperator)
            {
                addN();
            }
            else if(Node is UnaryOperator) 
            {
                if (subNode is BinaryOperator)
                {
                    //(-n)^m => -(n^m)
                    if(subNode is Power && Node is Negative)
                    {
                        addN();
                        return;
                    }
                    ((BinaryOperator)subNode).SAddOperand(Node);
                    Node = subNode;
                }
                else 
                {
                    addN();
                }
            }
            else
            {
                if(subNode is not UnaryOperator)
                    throw ActionNotFoundException();
                ((BinaryOperator)subNode).SAddOperand(Node);
                Node = subNode;
            }

            void addN()
            {
                var res = ((UnaryOperator)Node).SAddOperand(subNode);
                if (res == true)
                    return;
                if (res is null)
                    Node = subNode;
                else
                    throw ActionNotFoundException();
            }
        }

        private FException ActionNotFoundException() => new FInvalidOperationException("Action not found.");

        public override FFraction ToFraction()
        {
            if (Node is null)
                throw new FInvalidOperationException($"Empty container.");
            return Node.ToFraction();
        }

        public override string ToString()
        {
            var str = Node is null ? "null" : Node.ToString();
            return $"{{{str}}}";
        }

        public override FFraction? Simplify()
        {
            return Node is null ? null : (Node.Simplify() is not null ? Node.ToFraction() : null);
        }
    }
}
