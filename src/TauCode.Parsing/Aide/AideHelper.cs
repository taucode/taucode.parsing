using System;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide
{
    public static class AideHelper
    {
        public static Content GetCurrentContent(this IContext context)
        {
            // todo null check
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
                    content = optionalResult.OptionalContent;
                }
                else if (lastUnitResult is AlternativesResult alternativesResult)
                {
                    content = alternativesResult.GetLastAlternative();
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

        public static string FormatUnitResult(this UnitResult unitResult)
        {
            string result;

            if (unitResult is IdentifierNodeResult)
            {
                result = $@"{unitResult.SourceNodeName.ToUnitResultName()}\Identifier";
            }
            else if (unitResult is WordNodeResult wordNodeResult)
            {
                result = $@"{unitResult.SourceNodeName.ToUnitResultName()}{wordNodeResult.Word}";
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
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
