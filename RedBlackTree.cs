using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedBlackTree {
    public class Tree<T> {

        private static uint WeightBuffer { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        public uint Weight { get; set; }
        /// <summary>
        /// 是否在叶 0-不在 1-在
        /// </summary>
        public uint InLeaf { get; set; }

        /// <summary>
        /// 孩子节点数
        /// </summary>
        public int ChildrenNodeNumber { get; set; }

        /// <summary>
        /// 子节点id
        /// </summary>
        private Stack<Node> ChildrenNodes { get; set; }

        /// <summary>
        /// 剩余节点数
        /// </summary>
        private int NodesLeft { get; set; }

        /// <summary>
        /// 最大层数
        /// </summary>
        private uint MaximumHeight { get; set; }

        /// <summary>
        /// 根Id
        /// </summary>
        private uint RootId { get; set; }

        /// <summary>
        /// 根pId
        /// </summary>
        private uint RootPid { get; set; }
        /// <summary>
        /// 根name
        /// </summary>
        private T RootData { get; set; }

        public Tree(uint maxiumHeight, uint totalNodes, uint rootId, uint rootPid, T rootData) {
            ChildrenNodeNumber = (int)Math.Pow(totalNodes + 1, 1.0 / (maxiumHeight + 1)) + 2;
            NodesLeft = (int)Math.Abs(totalNodes) - 1;
            MaximumHeight = maxiumHeight + 1;
            RootId = rootId;
            RootData = rootData;
            RootPid = rootPid;
            ChildrenNodes = new Stack<Node>();
        }

        public class StructureNode {
            public uint Id { get; set; }
            public uint Pid { get; set; }
            public T Data { get; set; }
            public uint Sort { get; set; }
            public Queue<StructureNode> Children { get; set; }
        }

        public class Node {
            public uint Id { get; set; }
            public uint Pid { get; set; }
            public T Data { get; set; }

            public Stack<Node> Children { get; set; }

            public Node(uint id, T data, uint pid) {
                Id = id;
                Data = data;
                Pid = pid;
            }
        }

        private Queue<StructureNode> NewQueue(Stack<Node> node) {
            var queue = new Queue<StructureNode>();
            if (node != null) {
                if (node.Count != 0) {
                    var Sort = 0;
                    foreach (var item in node) {
                        if (item.Children != null && item.Children.Count != 0) {
                            queue.Enqueue(new StructureNode() {
                                Id = item.Id,
                                Pid = item.Pid,
                                Data = item.Data,
                                Sort = (uint)Sort,
                                Children = NewQueue(item.Children)
                            });
                        }
                        else {
                            queue.Enqueue(new StructureNode() {
                                Id = item.Id,
                                Pid = item.Pid,
                                Data = item.Data,
                                Sort = (uint)Sort
                            });
                        }
                        Sort += 1;
                    }
                }
            }
            return queue;
        }

        public StructureNode Tranverse(Stack<Node> nodes = null) {
            var data = new StructureNode();
            data.Id = RootId;
            data.Pid = RootPid;
            data.Data = RootData;
            data.Sort = 0;
            if (nodes == null) {
                data.Children = new Queue<StructureNode>(NewQueue(ChildrenNodes));
            }
            return data;
        }

        public bool Insert(uint id, T name, Stack<Node> nodes = null, uint pid = 0) {
            var done = false;
            if (NodesLeft == -1) {
                throw new IndexOutOfRangeException("红黑树叶节点无法生成分支");
            }
            else {
                if (ChildrenNodes.Count < ChildrenNodeNumber) {
                    ChildrenNodes.Push(new Node(id, name, RootId));
                    done = true;
                    NodesLeft -= 1;
                    if (ChildrenNodes.Count == ChildrenNodeNumber) {
                        InLeaf += 1;
                        Weight += 1;
                    }
                }
                else {
                    if (nodes == null) {
                        InLeaf = 1;
                        WeightBuffer = Weight;
                        Weight = 2;
                        Insert(id, name, ChildrenNodes, RootId);
                    }
                    if (nodes != null) {
                        var buffer = new Stack<Node>(nodes);
                        var length = nodes.Count;

                        for (var i = 0; i < length; i++) {
                            var item = buffer.Peek();
                            if (item.Children == null)
                                item.Children = new Stack<Node>();

                            if (item.Children.Count >= ChildrenNodeNumber && InLeaf < MaximumHeight - 1 && InLeaf < WeightBuffer) {
                                InLeaf += 1;
                                done = Insert(id, name, item.Children, item.Id);
                                if (done)
                                    Weight += 1;
                            }
                            else if (!done) {
                                if (item.Children.Count < ChildrenNodeNumber) {
                                    item.Children.Push(new Node(id, name, item.Id));
                                    if (Weight == MaximumHeight) {
                                        if (item.Children.Count == ChildrenNodeNumber)
                                            Weight = 1;
                                    }
                                    if (InLeaf == MaximumHeight - 1) {
                                        InLeaf += 1;
                                    }
                                    NodesLeft -= 1;
                                    done = true;
                                }
                            }
                            buffer.Pop();
                        }
                        InLeaf -= 1;
                    }

                }
            }
            return done;
        }
    }

    public class RootNode<T> {
        public uint Id { get; set; }
        public uint Pid { get; set; } = 0;
        public T Data { get; set; }

        public Tree<T> Btree { get; set; }

        public RootNode(uint nodeId, T nodeData, uint maxiumHeight, uint totalNodes) {
            Id = nodeId;
            Data = nodeData;
            Btree = new Tree<T>(maxiumHeight, totalNodes, Id, 0, Data);
        }
    }


}
