//using System;
//using System.Collections.Generic;

//namespace TauCode.Parsing.Aide.Building
//{
//    public class ContentOutcome
//    {
//        public ContentOutcome()
//        {
//            this.Nodes = new List<NodeBuilder>();
//        }

//        public NodeBuilder First { get; private set; }

//        public List<NodeBuilder> Nodes { get; }

//        public void AddNode(NodeBuilder nodeBuilder, bool initPreviousNodeLinks)
//        {
//            // todo checks
//            this.Nodes.Add(nodeBuilder);
//            if (this.First == null)
//            {
//                this.First = nodeBuilder;
//            }

//            if (this.Nodes.Count > 0)
//            {
//                throw new NotImplementedException();
//            }
//        }

//        //public void InitLinks()
//        //{
//        //    for (var i = 0; i < this.Nodes.Count; i++)
//        //    {
//        //        if (i < this.Nodes.Count - 1)
//        //        {
//        //            var leftNode = this.Nodes[i];
//        //            var rightNode = this.Nodes[i + 1];

//        //            var links = leftNode.Source.Arguments;

//        //            if (links.Count > 0)
//        //            {
//        //                throw new NotImplementedException();
//        //            }
//        //        }
//        //    }
//        //}
//    }
//}
