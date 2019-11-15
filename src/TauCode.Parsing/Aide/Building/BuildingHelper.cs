namespace TauCode.Parsing.Aide.Building
{
    // todo cleanup
    public static class BuildingHelper
    {
        //public static bool IsTopBlockDefinitionResult(this BlockDefinitionResult blockDefinitionResult)
        //{
        //    return
        //        blockDefinitionResult.Arguments.Count == 2 &&
        //        blockDefinitionResult.Arguments[1] == "top";
        //}

        //public static void CheckBlockDefinitionResultIsCorrect(this BlockDefinitionResult blockDefinitionResult)
        //{
        //    var correct = false;

        //    do
        //    {
        //        var args = blockDefinitionResult.Arguments;
        //        if (!args.Count.IsBetween(1, 2))
        //        {
        //            break;
        //        }

        //        if (args.Count == 2)
        //        {
        //            if (args[1] == "top")
        //            {
        //                // no problem
        //            }
        //            else
        //            {
        //                var content = blockDefinitionResult.Content;
        //                if (
        //                    content.GetAllResults().Count == 1 &&
        //                    content[0] is SyntaxElementResult syntaxElementResult &&
        //                    syntaxElementResult.SyntaxElement == SyntaxElement.Clone)
        //                {
        //                    // no problem
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }

        //        // all ok
        //        correct = true;
        //    } while (false);

        //    if (!correct)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public static string GetBlockDefinitionResultName(this BlockDefinitionResult blockDefinitionResult)
        //{
        //    return blockDefinitionResult.Arguments[0];
        //}

        //public static List<string> GetReferencedBlockNames(this BlockDefinitionResult blockDefinitionResult)
        //{
        //    var unitResults = blockDefinitionResult.GetAllUnitResults();
        //    var list = unitResults
        //        .Where(x => x.IsBlockReferenceResult())
        //        .Select(x => x.SourceNodeName)
        //        .ToList();

        //    return list;
        //}

        //public static bool IsBlockReferenceResult(this UnitResult unitResult)
        //{
        //    return
        //        unitResult is SyntaxElementResult syntaxElementResult &&
        //        syntaxElementResult.SyntaxElement == SyntaxElement.BlockReference;
        //}

        //public static List<UnitResult> GetAllUnitResults(this BlockDefinitionResult blockDefinitionResult)
        //{
        //    var list = new List<UnitResult>();
        //    var content = blockDefinitionResult.Content;
        //    content.WriteUnitResultsToList(list);
        //    return list;
        //}

        //private static void WriteUnitResultsToList(this Content content, List<UnitResult> list)
        //{
        //    var contentResults = content.GetAllResults();
        //    foreach (var contentResult in contentResults)
        //    {
        //        list.Add(contentResult);

        //        if (contentResult is OptionalResult optionalResult)
        //        {
        //            var innerContent = optionalResult.OptionalContent;
        //            innerContent.WriteUnitResultsToList(list);
        //        }
        //        else if (contentResult is AlternativesResult alternativesResult)
        //        {
        //            foreach (var alternative in alternativesResult.GetAllAlternatives())
        //            {
        //                alternative.WriteUnitResultsToList(list);
        //            }
        //        }
        //    }
        //}
    }
}
