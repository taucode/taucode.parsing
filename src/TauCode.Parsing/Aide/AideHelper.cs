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
    }
}
