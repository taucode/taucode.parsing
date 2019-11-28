using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide
{
    public static class AideHelper
    {
        internal const string AideNameReferenceClass = "Aide.NameReference";
        internal const string AideSpecialStringClassName = "Aide.SpecialStringClassName";

        #region Exceptions

        internal static LexerException CreateTokenNameCannotPrecedeChar(char c)
        {
            return new LexerException($"Token name cannot precede char '{c}'.");
        }

        #endregion

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
            else if (aideResult is TokenResult tokenResult)
            {
                var namesString = aideResult.Arguments.FormatArguments();
                result = $@"{aideResult.Name.ToAideResultName()}{tokenResult.Token.FormatToken()}{namesString}";
            }
            else if (aideResult is OptionalResult optionalResult)
            {
                var content = optionalResult.OptionalContent;
                var contentString = content.FormatContent();
                result = $@"{optionalResult.Name.ToAideResultName()}[ {contentString} ]";
            }
            else if (aideResult is AlternativesResult alternativesResult)
            {
                var namesString = aideResult.Arguments.FormatArguments();
                var sb = new StringBuilder();

                if (aideResult.Name != null)
                {
                    sb.Append($"<{aideResult.Name}>");
                }

                var alternatives = alternativesResult.GetAllAlternatives();
                
                sb.Append("{ ");

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

                sb.Append(" }");
                sb.Append(namesString);

                result = sb.ToString();
            }
            else
            {
                throw new AideException($"Not supported result type: {aideResult.GetType().FullName}.");
            }

            return result;
        }

        public static string FormatContent(this IContent content)
        {
            var results = content.ToList();
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

        private static string FormatArguments(this IEnumerable<string> arguments)
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
                sb.Append("*");
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

                case SymbolValue.Equals:
                    return "="; // todo: use mutual map Dictionary<char, SymbolValue> and vice-versa?

                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol));
            }
        }

        private static string FormatToken(this IToken token)
        {
            string result;

            if (token is WordToken wordToken)
            {
                result = wordToken.Word;
            }
            else if (token is EnumToken<SyntaxElement> syntaxEnumToken)
            {
                var sb = new StringBuilder();
                sb.Append($@"\{syntaxEnumToken.Value}");

                if (syntaxEnumToken.Properties.Any())
                {
                    var todoHas = syntaxEnumToken.Properties.TryGetValue(AideHelper.AideSpecialStringClassName, out var @class);
                    if (!todoHas)
                    {
                        throw new NotImplementedException();
                    }

                    sb.Append($":{@class}");
                }

                //return $@"\{syntaxEnumToken.Value}"; // todo clean up
                return sb.ToString();
            }
            else if (token is SymbolToken symbolToken)
            {
                return $@"\{symbolToken.Value.ToFormat()}";
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        private static string ToAideResultName(this string name)
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

            var blockDefinitionResult = accumulator.GetLastResult<BlockDefinitionResult>();
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

        private static void AddTokenResult(IToken token, IResultAccumulator accumulator)
        {
            var content = accumulator.GetActualContent();
            var result = new TokenResult(token);
            content.AddResult(result);
        }

        public static INode BuildParserRoot()
        {
            INodeFamily family = new NodeFamily("Aide");
            var root = new IdleNode(family, "root");
            root.ClaimLink("begin_block_def");

            var beginBlockDef = new ExactEnumNode<SyntaxElement>(
                family,
                "begin_block_def",
                (token, accumulator) =>
                {
                    var blockDefinitionResult = new BlockDefinitionResult(token.Name);
                    accumulator.AddResult(blockDefinitionResult);
                },
                SyntaxElement.BeginBlockDefinition);

            var args = BuildArgumentsRoot("block def begin args", family, acc => acc.GetLastResult<BlockDefinitionResult>());
            var beginBlockDefArgs = args.Item1;
            var beginBlockDefArgsExit = args.Item2;
            beginBlockDef.EstablishLink(beginBlockDefArgs);
            beginBlockDefArgsExit.ClaimLink("leftSplitter");

            var leftSplitter = new IdleNode(family, "leftSplitter");
            leftSplitter.AddLinksByNames(
                "syntaxWord",
                "anyWord",
                "anyInteger",
                "anyString",
                "identifier",
                "syntaxSymbol",
                "specialString",
                "blockReference",
                "idle",
                "end",
                "optional",
                "alternatives");

            var syntaxWord = new WordNode(
                family,
                "syntaxWord",
                AddTokenResult);
            var anyWord = new ExactEnumNode<SyntaxElement>(
                family,
                "anyWord",
                AddTokenResult,
                SyntaxElement.Word);
            var anyInteger = new ExactEnumNode<SyntaxElement>(
                family,
                "anyInteger",
                AddTokenResult,
                SyntaxElement.Integer);
            var anyString = new ExactEnumNode<SyntaxElement>(
                family,
                "anyString",
                AddTokenResult,
                SyntaxElement.String);
            var identifier = new ExactEnumNode<SyntaxElement>(
                family,
                "identifier",
                AddTokenResult,
                SyntaxElement.Identifier);
            var syntaxSymbol = new SymbolNode(
                family,
                "syntaxSymbol",
                AddTokenResult);
            var specialString = new ExactEnumNode<SyntaxElement>(
                family,
                "specialString",
                AddTokenResult,
                SyntaxElement.SpecialString);
            var blockReference = new ExactEnumNode<SyntaxElement>(
                family,
                "blockReference",
                AddTokenResult,
                SyntaxElement.BlockReference);
            var idle = new ExactEnumNode<SyntaxElement>(
                family,
                "idle",
                AddTokenResult,
                SyntaxElement.Idle);
            var end = new ExactEnumNode<SyntaxElement>(
                family,
                "end",
                AddTokenResult,
                SyntaxElement.End);

            #region optional

            var optional = new ExactEnumNode<SyntaxElement>(
                family,
                "optional",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var optionalResult = new OptionalResult(token.Name);
                    content.AddResult(optionalResult);
                },
                SyntaxElement.LeftBracket);
            var closeOptional = new ExactEnumNode<SyntaxElement>(
                family,
                "closeOptional",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    content.Seal();
                },
                SyntaxElement.RightBracket)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    return content.Owner is OptionalResult;
                }
            };

            #endregion

            #region alternatives

            var alternatives = new ExactEnumNode<SyntaxElement>(
                family,
                "alternatives",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var alternativesResult = new AlternativesResult(token.Name);
                    content.AddResult(alternativesResult);
                },
                SyntaxElement.LeftCurlyBracket);
            var addAlternative = new ExactEnumNode<SyntaxElement>(
                family,
                "addAlternative",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    var contentOwner = (AlternativesResult)content.Owner;
                    content.Seal();
                    contentOwner.AddAlternative();
                },
                SyntaxElement.VerticalBar)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    return content.Owner is AlternativesResult;
                }
            };
            var closeAlternatives = new ExactEnumNode<SyntaxElement>(
                family,
                "closeAlternatives",
                (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    content.Seal();
                },
                SyntaxElement.RightCurlyBracket)
            {
                AdditionalChecker = (token, accumulator) =>
                {
                    var content = accumulator.GetActualContent();
                    return content.Owner is AlternativesResult;
                }
            };


            #endregion

            var beforeArgsSplitter = new IdleNode(family, "beforeArgsSplitter");
            beforeArgsSplitter.DrawLinkFromNodes(
                syntaxWord,
                anyWord,
                anyInteger,
                anyString,
                identifier,
                syntaxSymbol,
                specialString,
                blockReference,
                idle,
                end);

            args = BuildArgumentsRoot("content args", family, acc => acc.GetActualContent().Last());
            var contentNodeArgs = args.Item1;
            var contentNodeArgsExit = args.Item2;

            beforeArgsSplitter.EstablishLink(contentNodeArgs);

            var afterArgsSplitter = new IdleNode(family, "afterArgsSplitter");

            afterArgsSplitter.DrawLinkFromNodes(contentNodeArgsExit, beforeArgsSplitter);
            afterArgsSplitter.EstablishLink(leftSplitter);
            afterArgsSplitter.ClaimLink("endBlockDef");

            #region links for optional

            // opening
            optional.EstablishLink(leftSplitter);

            // closing
            closeOptional.EstablishLink(beforeArgsSplitter);
            afterArgsSplitter.EstablishLink(closeOptional);

            #endregion

            #region links for alternatives

            // opening
            alternatives.EstablishLink(leftSplitter);

            // addAlternative
            addAlternative.EstablishLink(leftSplitter);
            afterArgsSplitter.EstablishLink(addAlternative);

            // closing
            closeAlternatives.EstablishLink(beforeArgsSplitter);
            afterArgsSplitter.EstablishLink(closeAlternatives);

            #endregion

            var endBlockDef = new ExactEnumNode<SyntaxElement>(
                family,
                "endBlockDef",
                (token, accumulator) =>
                {
                    var currentBlockDef = accumulator.GetLastResult<BlockDefinitionResult>();
                    currentBlockDef.Content.Seal();
                },
                SyntaxElement.EndBlockDefinition);

            endBlockDef.EstablishLink(EndNode.Instance);

            return root;
        }

        private static Tuple<INode, INode> BuildArgumentsRoot(
            string prefix,
            INodeFamily family,
            Func<IResultAccumulator, IAideResult> resultGetter)
        {
            INode begin = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: (", null, SyntaxElement.LeftParenthesis);
            INode arg = new ClassedSpecialStringNode(
                family,
                $"{prefix}: arg",
                (token, accumulator) =>
                {
                    var result = resultGetter(accumulator);
                    result.Arguments.Add(((SpecialStringToken)token).Value);
                },
                AideNameReferenceClass);
            INode comma = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: ,", null, SyntaxElement.Comma);
            INode end = new ExactEnumNode<SyntaxElement>(family, $"{prefix}: )", null, SyntaxElement.RightParenthesis);

            begin.EstablishLink(arg);
            arg.EstablishLink(comma);
            arg.EstablishLink(end);
            comma.EstablishLink(arg);

            return Tuple.Create(begin, end);
        }
    }
}
