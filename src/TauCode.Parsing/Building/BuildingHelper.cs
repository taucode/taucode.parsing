using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public static class BuildingHelper
    {
        public static string GetItemName(this Element item)
        {
            // 'GetSingleKeywordArgument' will do all the checks.
            return item.GetSingleKeywordArgument<Symbol>(":name", true)?.Name;
        }

        public static List<string> GetItemLinks(this Element item)
        {
            // 'GetAllKeywordArguments' will do all the checks.
            var links = item
                .GetAllKeywordArguments(":links", true)
                .Select(x => x.AsElement<Symbol>().Name)
                .ToList();

            return links;
        }

    }
}
