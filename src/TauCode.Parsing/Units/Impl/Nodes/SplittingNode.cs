//using System.Collections.Generic;

//namespace TauCode.Parsing.Units.Impl.Nodes
//{
//    // todo: rename to idle node?
//    public class SplittingNode : Node
//    {
//        public SplittingNode(string name)
//            : base(name)
//        {

//        }

//        protected override IReadOnlyCollection<IUnit> ProcessImpl(ITokenStream stream, IContext context)
//        {
//            foreach (var link in this.Links)
//            {
//                IReadOnlyCollection<IUnit> result = link.Process(stream, context);
//                if (result != null)
//                {
//                    return result;
//                }
//            }

//            return null;
//        }
//    }
//}
