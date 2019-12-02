using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    // todo: checks for all
    public static class BuildingHelper
    {
        public static string GetItemName(this Element item)
        {
            return item.GetSingleKeywordArgument<Symbol>(":name", true)?.Name;
        }

        public static List<string> GetItemLinks(this Element item)
        {
            var links = item
                .GetAllKeywordArguments(":links", true)
                .Select(x => x.AsElement<Symbol>().Name)
                .ToList();

            return links;
        }

    }
}
