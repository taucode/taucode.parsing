using System;
using System.Linq;
using TauCode.Parsing.Aide.Results2;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes2;
using TauCode.Parsing.Tokens;

// todo clean up

namespace TauCode.Parsing.Aide
{
    public static class AideHelper
    {
        #region Exceptions

        internal static LexerException CreateTokenNameCannotPrecedeChar(char c)
        {
            return new LexerException($"Token name cannot precede char '{c}'.");
        }

        #endregion

        //public static Content GetCurrentContent(this IContext context)
        //{
        //    if (context is null)
        //    {
        //        throw new ArgumentNullException(nameof(context));
        //    }

        //    var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
        //    var content = blockDefinitionResult.Content;

        //    while (true)
        //    {
        //        if (content.UnitResultCount == 0)
        //        {
        //            return content;
        //        }

        //        var lastUnitResult = content.GetLastUnitResult();

        //        if (lastUnitResult is OptionalResult optionalResult)
        //        {
        //            var nextContent = optionalResult.OptionalContent;
        //            if (nextContent.IsSealed)
        //            {
        //                return content;
        //            }
        //            else
        //            {
        //                content = nextContent;
        //            }
        //        }
        //        else if (lastUnitResult is AlternativesResult alternativesResult)
        //        {
        //            var nextContent = alternativesResult.GetLastAlternative();
        //            if (nextContent.IsSealed)
        //            {
        //                return content;
        //            }
        //            else
        //            {
        //                content = nextContent;
        //            }
        //        }
        //        else
        //        {
        //            return content;
        //        }
        //    }
        //}

        //public static string ToAideResultFormat(this IAideResult aideResult)
        //{
        //    string result;

        //    if (aideResult is BlockDefinitionResult blockDefinitionResult)
        //    {
        //        var sb = new StringBuilder();
        //        var namesString = blockDefinitionResult.Arguments.FormatArguments();
        //        sb.Append($@"\BeginBlockDefinition{namesString}");
        //        sb.AppendLine();
        //        sb.AppendLine(blockDefinitionResult.Content.FormatContent());
        //        sb.Append(@"\EndBlockDefinition");

        //        result = sb.ToString();
        //    }
        //    else if (aideResult is WordNodeResult wordNodeResult)
        //    {
        //        result = $@"{wordNodeResult.SourceNodeName.ToUnitResultName()}{wordNodeResult.Word}";
        //    }
        //    else if (aideResult is SymbolNodeResult symbolNodeResult)
        //    {
        //        result = $@"{symbolNodeResult.SourceNodeName.ToUnitResultName()}\{symbolNodeResult.Value.ToFormat()}";
        //    }
        //    else if (aideResult is SyntaxElementResult syntaxElementResult)
        //    {
        //        var sb = new StringBuilder();
        //        sb.Append(
        //            $@"{syntaxElementResult.SourceNodeName.ToUnitResultName()}\{syntaxElementResult.SyntaxElement}");

        //        var args = FormatArguments(syntaxElementResult.Arguments);
        //        sb.Append(args);

        //        result = sb.ToString();
        //    }
        //    else if (aideResult is OptionalResult optionalResult)
        //    {
        //        var content = optionalResult.OptionalContent;
        //        var contentString = content.FormatContent();
        //        result = $@"{optionalResult.SourceNodeName.ToUnitResultName()}[{contentString}]";
        //    }
        //    else if (aideResult is AlternativesResult alternativesResult)
        //    {
        //        var alternatives = alternativesResult.GetAllAlternatives();
        //        var sb = new StringBuilder();
        //        sb.Append("{");

        //        for (var i = 0; i < alternatives.Count; i++)
        //        {
        //            var content = alternatives[i];
        //            var contentString = content.FormatContent();
        //            sb.Append(contentString);

        //            if (i < alternatives.Count - 1)
        //            {
        //                sb.Append(" | ");
        //            }
        //        }

        //        sb.Append("}");

        //        result = sb.ToString();
        //    }
        //    else
        //    {
        //        throw new AideException($"Not supported result type: {aideResult.GetType().FullName}.");
        //    }

        //    return result;
        //}

        //public static string FormatContent(this Content content)
        //{
        //    var results = content.GetAllResults();
        //    var sb = new StringBuilder();

        //    for (var i = 0; i < results.Count; i++)
        //    {
        //        var result = results[i];
        //        sb.Append(result.ToAideResultFormat());
        //        if (i < results.Count - 1)
        //        {
        //            sb.Append(" ");
        //        }
        //    }

        //    return sb.ToString();
        //}

        //private static string FormatArguments(this NameReferenceCollector arguments)
        //{
        //    var names = arguments.ToArray();
        //    if (names.Length == 0)
        //    {
        //        return "";
        //    }

        //    var sb = new StringBuilder();


        //    sb.Append("(");

        //    for (var i = 0; i < names.Length; i++)
        //    {
        //        sb.Append(":");
        //        sb.Append(names[i]);
        //        if (i < names.Length - 1)
        //        {
        //            sb.Append(", ");
        //        }
        //    }

        //    sb.Append(")");

        //    return sb.ToString();
        //}

        private static string ToFormat(this SymbolValue symbol)
        {
            switch (symbol)
            {
                case SymbolValue.Comma:
                    return ",";

                case SymbolValue.LeftParenthesis:
                    return "(";

                case SymbolValue.RightParenthesis:
                    return ")";

                case SymbolValue.Semicolon:
                    return ";";

                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol));
            }
        }

        private static string ToUnitResultName(this string name)
        {
            if (name == null)
            {
                return string.Empty;
            }
            else
            {
                return $"<{name}>";
            }
        }

        private static IContent GetActualContent(this IResultAccumulator accumulator)
        {
            if (accumulator is null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            var blockDefinitionResult = accumulator.GetLastResult<BlockDefinitionResult2>();
            var content = blockDefinitionResult.Content;

            while (true)
            {
                if (content.Count == 0)
                {
                    return content;
                }

                var lastResult = content.Last();

                if (lastResult is OptionalResult optionalResult)
                {
                    var nextContent = optionalResult.OptionalContent;
                    if (nextContent.IsSealed)
                    {
                        return content;
                    }
                    else
                    {
                        content = nextContent;
                    }
                }
                else if (lastResult is AlternativesResult alternativesResult)
                {
                    var nextContent = alternativesResult.GetLastAlternative();
                    if (nextContent.IsSealed)
                    {
                        return content;
                    }
                    else
                    {
                        content = nextContent;
                    }
                }
                else
                {
                    return content;
                }
            }
        }

        public static INode2 BuildParserRoot()
        {
            INodeFamily family = new NodeFamily("Aide");
            var root = new IdleNode(family, "root");
            root.AddLinkByName("begin_block_def");

            var beginBlockDef = new ExactEnumNode<SyntaxElement>(
                family,
                "begin_block_def",
                (token, accumulator) =>
                {
                    var blockDefinitionResult = new BlockDefinitionResult2();
                    accumulator.AddResult(blockDefinitionResult);
                },
                SyntaxElement.BeginBlockDefinition);

            var args = BuildArgumentsRoot("block def begin args", family, acc => acc.GetLastResult<BlockDefinitionResult2>());
            var beginBlockDefArgs = args.Item1;
            var beginBlockDefArgsExit = args.Item2;
            beginBlockDef.AddLink(beginBlockDefArgs);
            beginBlockDefArgsExit.AddLinkByName("leftSplitter");

            var leftSplitter = new IdleNode(family, "leftSplitter");
            leftSplitter.AddLinksByNames("word", "identifier", "symbol", "blockReference", "optional", "alternatives");

            var word = new WordNode(
                family,
                "word",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var result = new TokenResult(token);
                    content.AddResult(result);
                });
            var identifier = new ExactEnumNode<SyntaxElement>(
                family,
                "identifier",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var result = new TokenResult(token);
                    content.AddResult(result);
                },
                SyntaxElement.Identifier);
            var symbol = new SymbolNode(
                family,
                "symbol",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var result = new TokenResult(token);
                    content.AddResult(result);
                });
            var blockReference = new ExactEnumNode<SyntaxElement>(
                family,
                "blockReference",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var result = new TokenResult(token);
                    content.AddResult(result);
                },
                SyntaxElement.BlockReference);

            #region optional


            var optional = new ExactEnumNode<SyntaxElement>(
                family,
                "optional",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.LeftBracket);
            var closeOptional = new ExactEnumNode<SyntaxElement>(
                family,
                "closeOptional",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.RightBracket)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    throw new NotImplementedException();
                }
            };

            #endregion

            #region alternatives

            var alternatives = new ExactEnumNode<SyntaxElement>(
                family,
                "alternatives",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.LeftCurlyBracket);
            var addAlternative = new ExactEnumNode<SyntaxElement>(
                family,
                "addAlternative",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.VerticalBar)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    throw new NotImplementedException();
                }
            };
            var closeAlternatives = new ExactEnumNode<SyntaxElement>(
                family,
                "closeAlternatives",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.RightCurlyBracket)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    throw new NotImplementedException();
                }
            };


            #endregion

            var beforeArgsSplitter = new IdleNode(family, "beforeArgsSplitter");
            beforeArgsSplitter.DrawLinkFromNodes(word, identifier, symbol, blockReference);

            args = BuildArgumentsRoot("content args", family, acc => acc.GetActualContent().Last());
            var contentNodeArgs = args.Item1;
            var contentNodeArgsExit = args.Item2;

            beforeArgsSplitter.AddLink(contentNodeArgs);

            var afterArgsSplitter = new IdleNode(family, "afterArgsSplitter");

            afterArgsSplitter.DrawLinkFromNodes(contentNodeArgsExit, beforeArgsSplitter);
            afterArgsSplitter.AddLink(leftSplitter);
            afterArgsSplitter.AddLinkByName("endBlockDef");

            #region links for optional

            

            #endregion

            var endBlockDef = new ExactEnumNode<SyntaxElement>(
                family,
                "endBlockDef",
                (token, accumulator) =>
                {
                    var currentBlockDef = accumulator.GetLastResult<BlockDefinitionResult2>();
                    currentBlockDef.Content.Seal();
                },
                SyntaxElement.EndBlockDefinition);

            endBlockDef.AddLink(EndNode.Instance);

            return root;
        }

        private static Tuple<INode2, INode2> BuildArgumentsRoot(string prefix, INodeFamily family, Func<IResultAccumulator, IAideResult2> resultGetter)
        {
            INode2 begin = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: (", null, SyntaxElement.LeftParenthesis);
            INode2 arg = new SpecialStringNode<AideSpecialString>(
                family,
                $"{prefix}: arg",
                (token, accumulator) =>
                {
                    var result = resultGetter(accumulator);
                    result.Arguments.Add(((SpecialStringToken<AideSpecialString>)token).Value);
                },
                AideSpecialString.NameReference);
            INode2 comma = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: ,", null, SyntaxElement.Comma);
            INode2 end = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: )", null, SyntaxElement.RightParenthesis);


            begin.AddLink(arg);
            arg.AddLink(comma);
            arg.AddLink(end);
            comma.AddLink(arg);

            return Tuple.Create(begin, end);
        }
    }
}
