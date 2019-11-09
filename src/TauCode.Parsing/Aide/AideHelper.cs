using System;
using TauCode.Parsing.Aide.Parsing;

namespace TauCode.Parsing.Aide
{
    public static class AideHelper
    {
        public static UnitResult GetCurrentBlockContentResult(IContext context)
        {
            // todo null check
            if (context.ResultCount == 0)
            {
                return null;
            }

            var result = context.GetLastResult<BlockContentResult>();

            throw new NotImplementedException();
            //while (true)
            //{
            //    result.GetLastUnitResult<>()
            //}

            //return result;
        }
    }
}
