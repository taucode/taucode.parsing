using System;
using System.Linq;
using System.Text;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes2;
using TauCode.Parsing.Tokens;

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

        public static string ToAideResultFormat(this IAideResult aideResult)
        {
            string result;

            if (aideResult is BlockDefinitionResult blockDefinitionResult)
            {
                var sb = new StringBuilder();
                var namesString = blockDefinitionResult.Arguments.FormatArguments();
                sb.Append($@"\BeginBlockDefinition{namesString}");
                sb.AppendLine();
                sb.AppendLine(blockDefinitionResult.Content.FormatContent());
                sb.Append(@"\EndBlockDefinition");

                result = sb.ToString();
            }
            else if (aideResult is WordNodeResult wordNodeResult)
            {
                result = $@"{wordNodeResult.SourceNodeName.ToUnitResultName()}{wordNodeResult.Word}";
            }
            else if (aideResult is SymbolNodeResult symbolNodeResult)
            {
                result = $@"{symbolNodeResult.SourceNodeName.ToUnitResultName()}\{symbolNodeResult.Value.ToFormat()}";
            }
            else if (aideResult is SyntaxElementResult syntaxElementResult)
            {
                var sb = new StringBuilder();
                sb.Append(
                    $@"{syntaxElementResult.SourceNodeName.ToUnitResultName()}\{syntaxElementResult.SyntaxElement}");

                var args = FormatArguments(syntaxElementResult.Arguments);
                sb.Append(args);

                result = sb.ToString();
            }
            else if (aideResult is OptionalResult optionalResult)
            {
                var content = optionalResult.OptionalContent;
                var contentString = content.FormatContent();
                result = $@"{optionalResult.SourceNodeName.ToUnitResultName()}[{contentString}]";
            }
            else if (aideResult is AlternativesResult alternativesResult)
            {
                var alternatives = alternativesResult.GetAllAlternatives();
                var sb = new StringBuilder();
                sb.Append("{");

                for (var i = 0; i < alternatives.Count; i++)
                {
                    var content = alternatives[i];
                    var contentString = content.FormatContent();
                    sb.Append(contentString);

                    if (i < alternatives.Count - 1)
                    {
                        sb.Append(" | ");
                    }
                }

                sb.Append("}");

                result = sb.ToString();
            }
            else
            {
                throw new AideException($"Not supported result type: {aideResult.GetType().FullName}.");
            }

            return result;
        }

        public static string FormatContent(this Content content)
        {
            var results = content.GetAllResults();
            var sb = new StringBuilder();

            for (var i = 0; i < results.Count; i++)
            {
                var result = results[i];
                sb.Append(result.ToAideResultFormat());
                if (i < results.Count - 1)
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }

        private static string FormatArguments(this NameReferenceCollector arguments)
        {
            var names = arguments.ToArray();
            if (names.Length == 0)
            {
                return "";
            }

            var sb = new StringBuilder();


            sb.Append("(");

            for (var i = 0; i < names.Length; i++)
            {
                sb.Append(":");
                sb.Append(names[i]);
                if (i < names.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");

            return sb.ToString();
        }

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
                    var blockDefinitionResult = new BlockDefinitionResult();
                    accumulator.AddResult(blockDefinitionResult);
                },
                SyntaxElement.BeginBlockDefinition);

            var args = BuildArgumentsRoot("block def begin args", family);
            var beginBlockDefArgs = args.Item1;
            var beginBlockDefArgsExit = args.Item2;
            beginBlockDef.AddLink(beginBlockDefArgs);
            beginBlockDefArgsExit.AddLinkByName("left_splitter");

            var leftSplitter = new IdleNode(family, "left_splitter");
            leftSplitter.AddLinksByNames("word", "identifier"/*, "optional", "alternatives" todo*/);

            var word = new WordNode(family, "word", (token, accumulator) => throw new NotImplementedException());
            var @enum = new EnumNode<SyntaxElement>(family, "enum", (token, accumulator) => throw new NotImplementedException());
            //var optional = BuildOptionalRoot("optional");
            //var alternatives = BuildAlternativesRoot("alternatives");

            //var rightSplitter = new IdleNode(family, "right_splitter");
            //rightSplitter.DrawLinkFromNodes(word, @enum/*, optional, alternatives todo */);

            
            args = BuildArgumentsRoot("content args", family);
            var contentNodeArgs = args.Item1;
            var contentNodeArgsExit = args.Item2;

            contentNodeArgs.DrawLinkFromNodes(word, @enum/*, optional, alternatives todo */);
            
            contentNodeArgsExit.AddLink(leftSplitter);
            contentNodeArgsExit.AddLinkByName("end_block_def");

            var endBlockDefs = new ExactEnumNode<SyntaxElement>(
                family,
                "end_block_def",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                SyntaxElement.BeginBlockDefinition);

            return root;
        }

        private static Tuple<INode2, INode2> BuildArgumentsRoot(string prefix, INodeFamily family)
        {
            INode2 begin = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: (", null, SyntaxElement.LeftParenthesis);
            INode2 arg = new SpecialStringNode<AideSpecialString>(
                family, 
                $"{prefix}: arg",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                AideSpecialString.NameReference);
            INode2 comma = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: ,", null, SyntaxElement.Comma);
            INode2 end = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: )", null, SyntaxElement.RightParenthesis);

            begin.LinkChain(arg, comma, end);
            comma.AddLink(arg);

            return Tuple.Create(begin, end);
        }
    }
}
