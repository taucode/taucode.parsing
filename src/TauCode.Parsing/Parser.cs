using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Utils.CommandLine.Parsing;

namespace TauCode.Parsing
{
    public class Parser : IParser
    {
        public object[] Parse(INode root, IEnumerable<IToken> tokens)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var stream = new TokenStream(tokens);
            IContext context = new Context(stream);
            var initialNodes = ParsingHelper.GetNonIdleNodes(new[] { root });

            context.SetNodes(initialNodes);
            var winners = new List<INode>();

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
                        throw new ParsingException("Unexpected end of stream.");
                    }
                }

                var token = stream.CurrentToken;

                winners.Clear();
                var gotActor = false;
                var gotEnd = false;
                var gotSkippers = false;

                foreach (var node in nodes)
                {
                    var inquireResult = node.Inquire(token, context.ResultAccumulator);

                    switch (inquireResult)
                    {
                        case InquireResult.Reject:
                            // bye baby
                            break;

                        case InquireResult.Skip:
                            if (gotActor)
                            {
                                throw new ParsingException("Nodes logic error. More than one node accepted the token.");
                            }
                            gotSkippers = true;
                            winners.Add(node);
                            break;

                        case InquireResult.Act:
                            if (gotActor)
                            {
                                throw new ParsingException("Nodes logic error. More than one node accepted the token.");
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
                    if (gotEnd)
                    {
                        // fine, got to end, start over.
                        context.SetNodes(initialNodes);
                    }
                    else
                    {
                        throw new ParsingException("Unexpected token.");
                    }
                }
                else
                {
                    if (gotActor)
                    {
                        var actor = winners.Single();
                        var oldVersion = context.ResultAccumulator.Version;
                        actor.Act(token, context.ResultAccumulator);
                        if (oldVersion + 1 != context.ResultAccumulator.Version)
                        {
                            throw new ParsingException("Internal error. Non sequential result accumulator versions.");
                        }
                    }
                    else
                    {
                        // 'gotSkippers' must be true
                        if (!gotSkippers)
                        {
                            throw new ParsingException("Internal parser error.");
                        }
                    }

                    // skip
                    context.TokenStream.AdvanceStreamPosition();
                    var successors = winners.SelectMany(x => x.ResolveLinks()).ToList();

                    var nonIdleSuccessors = ParsingHelper.GetNonIdleNodes(successors);

                    // next nodes
                    context.SetNodes(nonIdleSuccessors);
                }
            }
        }
    }
}
