//using System.Collections.Generic;
//using System.Linq;
//using TauCode.Parsing.Exceptions;
//using TauCode.Parsing.Units;
//using TauCode.Parsing.Units.Impl.Nodes;

//namespace TauCode.Parsing
//{
//    public abstract class ParserBase : IParser
//    {
//        #region Protected

//        protected IUnit Head { get; private set; }

//        #endregion

//        #region Abstract

//        protected abstract IUnit BuildTree();

//        #endregion

//        #region IParser Members

//        public IContext Parse(IEnumerable<IToken> tokens)
//        {
//            var context = new Context();
//            var stream = new TokenStream(tokens);

//            if (this.Head == null)
//            {
//                this.Head = this.BuildTree();
//            }

//            while (true)
//            {
//                if (stream.IsEndOfStream())
//                {
//                    return context;
//                }

//                var current = this.Head;

//                while (true)
//                {
//                    var result = current.Process(stream, context);
//                    if (result == null)
//                    {
//                        throw new ParserException($"'Process' method returned null. {current.ToUnitDiagnosticsString()}");
//                    }
//                    else if (result.Count == 1)
//                    {
//                        if (ParsingHelper.IsEndResult(result))
//                        {
//                            // start again
//                            break;
//                        }
//                        else
//                        {
//                            throw new ParserException($"'Process' returned single result and that result is not 'End node'. Current {current.ToUnitDiagnosticsString()} Result {result.Single().ToUnitDiagnosticsString()}");
//                        }
//                    }
//                    else
//                    {
//                        if (result.Count == 2 && result.Contains(EndNode.Instance))
//                        {
//                            result = result.Where(x => x != EndNode.Instance).ToList();
//                            if (result.Count != 1)
//                            {
//                                throw new ParserException("'Process' returned result with two units, and condition 'one of them is EndNode' not met.");
//                            }

//                            continue;
//                        }
//                        else
//                        {
//                            throw new ParserException("'Process' returned result with more than 2 units.");
//                        }
//                    }
//                }
//            }
//        }

//        #endregion
//    }
//}
