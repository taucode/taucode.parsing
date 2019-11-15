using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes2;

namespace TauCode.Parsing
{
    public class Parser2 : IParser2
    {
        public object[] Parse(INode2 root, IEnumerable<IToken> tokens)
        {
            // todo check args

            var stream = new TokenStream(tokens);
            IContext2 context = new Context2(stream);

            context.SetNodes(ParsingHelper.GetNonIdleNodes(new[] { root }));
            var winners = new List<INode2>();

            while (true)
            {
                var nodes = context.GetNodes();
                if (stream.IsEndOfStream())
                {
                    if (nodes.Contains(EndNode.Instance))
                    {
                        // met end of stream, but that is acceptable
                        return context.ResultAccumulator.ToArray();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                var token = stream.CurrentToken;

                winners.Clear();
                var gotActor = false;
                var gotEnd = false;
                var gotSkippers = false;

                foreach (var node in nodes)
                {
                    var inquireResult = node.Inquire(token);

                    switch (inquireResult)
                    {
                        case InquireResult.Reject:
                            // bye baby
                            break;

                        case InquireResult.Skip:
                            if (gotActor)
                            {
                                throw new NotImplementedException();
                            }
                            gotSkippers = true;
                            winners.Add(node);
                            break;

                        case InquireResult.Act:
                            if (gotActor)
                            {
                                throw new NotImplementedException();
                            }
                            gotActor = true;
                            winners.Add(node);
                            break;

                        case InquireResult.End:
                            gotEnd = true;
                            // don't add to winners
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (winners.Count == 0)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    if (gotActor)
                    {
                        var actor = winners.Single(); // todo optimize & check single
                        var oldVersion = context.ResultAccumulator.Version;
                        actor.Act(token, context.ResultAccumulator);
                        if (oldVersion + 1 != context.ResultAccumulator.Version)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        // 'gotSkippers' must be true
                        if (!gotSkippers)
                        {
                            throw new NotImplementedException(); // error
                        }
                    }

                    // skip
                    context.TokenStream.AdvanceStreamPosition();
                    var successors = winners.SelectMany(x => x.Links).ToList();

                    var nonIdleSuccessors = ParsingHelper.GetNonIdleNodes(successors);

                    // next nodes
                    context.SetNodes(nonIdleSuccessors);
                }
            }
        }
    }
}
