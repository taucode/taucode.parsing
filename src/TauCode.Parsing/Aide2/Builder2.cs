using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Aide2
{
    public class Builder2 : IBuilder2
    {
        private Dictionary<string, PseudoList> _defblocks;

        public INode Build(PseudoList defblocks)
        {
            _defblocks = defblocks.ToDictionary(x => x.GetCarSymbolName(), x => x.AsPseudoList());

            throw new NotImplementedException();
        }
    }
}
