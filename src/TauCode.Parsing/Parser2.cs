using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class Parser2 : IParser2
    {
        public object[] Parse(INode2 root, IEnumerable<IToken> tokens)
        {
            // todo check args

            var stream = new TokenStream(tokens);
            IContext2 context = new Context2(stream);

            context.SetNodes(root);
            var winners = new List<INode2>();

            while (true)
            {
                if (stream.IsEndOfStream())
                {
                    throw new NotImplementedException();
                }

                var token = stream.CurrentToken;
                var nodes = context.GetNodes();

                winners.Clear();
                var gotConsumer = false;

                foreach (var node in nodes)
                {
                    var inquireResult = node.Inquire(token);

                    switch (inquireResult)
                    {
                        case InquireResult.Reject:
                            // bye baby
                            break;

                        case InquireResult.Skip:
                            if (gotConsumer)
                            {
                                throw new NotImplementedException();
                            }
                            winners.Add(node);
                            break;

                        case InquireResult.Act:
                            if (gotConsumer)
                            {
                                throw new NotImplementedException();
                            }
                            winners.Add(node);
                            gotConsumer = true;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    throw new NotImplementedException();
                }
            }
        }
    }
}
