using StringEquationConverter.FExceptions;
using StringEquationConverter.NodeTypes.Operators;
using StringEquationConverter.NodeTypes.Operators.BinaryOperators;
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
        public TreeNode? Node { get; set; }
        public ContainerNode? ParentContainer { get; set; }

        public ContainerNode() { }

        public ContainerNode(ContainerNode parent)
        {
            ParentContainer = parent;
        }

        public ContainerNode(ContainerNode parent, TreeNode? node)
        {
            ParentContainer = parent;
            Node = node;
        }

        public bool IsEmpty() => Node == null;

        public void AddNode(TreeNode subNode)
        {
            if (Node is null)
            {
                Node = subNode;
                return;
            }
            if(Node is BinaryOperator)
            {
                var res = ((UnaryOperator)Node).SAddOperand(subNode);
                if (res == true)
                    return;
                if (res is null)
                    Node = subNode;
                else
                    throw ActionNotFoundException();
            }
            else if(Node is UnaryOperator) 
            {
                if (subNode is BinaryOperator)
                {
                    ((BinaryOperator)subNode).SAddOperand(Node);
                    Node = subNode;
                }
                else 
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
            else
            {
                if(subNode is not UnaryOperator)
                    throw ActionNotFoundException();
                ((BinaryOperator)subNode).SAddOperand(Node);
                Node = subNode;
            }
        }

        private FException ActionNotFoundException() => new FInvalidOperationException("Action not found.");

        public override FFraction ToFraction()
        {
            if (Node is null)
                throw new FInvalidOperationException($"{nameof(Node)} is null.");
            return Node.ToFraction();
        }

        public override string ToString()
        {
            var str = Node is null ? "null" : Node.ToString();
            return $"{{{str}}}";
        }
    }
}
