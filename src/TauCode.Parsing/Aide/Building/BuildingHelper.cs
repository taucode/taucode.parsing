using System;
using TauCode.Parsing.Aide.Results;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Aide.Building
{
    public static class BuildingHelper
    {
        public static bool IsTopBlockDefinitionResult(this BlockDefinitionResult blockDefinitionResult)
        {
            return
                blockDefinitionResult.Arguments.Count == 2 &&
                blockDefinitionResult.Arguments[1] == "top";
        }

        public static void CheckBlockDefinitionResultIsCorrect(this BlockDefinitionResult blockDefinitionResult)
        {
            var correct = false;

            do
            {
                var args = blockDefinitionResult.Arguments;
                if (!args.Count.IsBetween(1, 2))
                {
                    break;
                }

                if (args.Count == 2 && args[1] != "top")
                {
                    break;
                }

                // all ok
                correct = true;
            } while (false);

            if (!correct)
            {
                throw new NotImplementedException();
            }
        }

        public static string GetBlockDefinitionResultName(this BlockDefinitionResult blockDefinitionResult)
        {
            return blockDefinitionResult.Arguments[0];
        }
    }
}
