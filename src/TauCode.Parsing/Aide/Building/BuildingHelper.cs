using System.Collections.Generic;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public static class BuildingHelper
    {
        public static IEnumerable<TokenResult> GetAllTokenResultsFromContent(this IContent content)
        {
            foreach (var result in content)
            {
                if (result is TokenResult tokenResult)
                {
                    yield return tokenResult;
                }
                else if (result is OptionalResult optionalResult)
                {
                    foreach (var resultFromContent in optionalResult.OptionalContent.GetAllTokenResultsFromContent())
                    {
                        yield return resultFromContent;
                    }
                }
                else if (result is AlternativesResult alternativesResult)
                {
                    foreach (var alternative in alternativesResult.GetAllAlternatives())
                    {
                        foreach (var resultFromAlternative in alternative.GetAllTokenResultsFromContent())
                        {
                            yield return resultFromAlternative;
                        }
                    }
                }
            }
        }
    }
}
