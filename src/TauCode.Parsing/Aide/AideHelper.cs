using System;
using System.Linq;
using System.Text;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Exceptions;
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

        public static Content GetCurrentContent(this IContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var blockDefinitionResult = context.GetLastResult<BlockDefinitionResult>();
            var content = blockDefinitionResult.Content;

            while (true)
            {
                if (content.UnitResultCount == 0)
                {
                    return content;
                }

                var lastUnitResult = content.GetLastUnitResult();

                if (lastUnitResult is OptionalResult optionalResult)
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
                else if (lastUnitResult is AlternativesResult alternativesResult)
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

        public static string GetAideTokenName(this IToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is AideToken aideToken)
            {
                return aideToken.Name;
            }

            throw new ArgumentException("Token is not Aide token.", nameof(token));
        }

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
            else if (aideResult is CloneBlockResult cloneBlockResult)
            {
                var namesString = cloneBlockResult.Arguments.FormatArguments();
                result = $@"\CloneBlock{namesString}";
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
                sb.Append($@"{syntaxElementResult.SourceNodeName.ToUnitResultName()}\{syntaxElementResult.SyntaxElement}");

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

            //foreach (var result in results)
            //{
            //    sb.Append(result.FormatUnitResult());
            //}
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
    }
}
