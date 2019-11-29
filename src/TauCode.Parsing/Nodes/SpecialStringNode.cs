﻿using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class SpecialStringNode : ActionNode
    {
        public SpecialStringNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is SpecialStringToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }
    }
}
