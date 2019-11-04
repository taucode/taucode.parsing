using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.ParsingUnits
{
    public class BlockParsingUnit : IParsingUnit
    {
        public BlockParsingUnit(NodeParsingUnit head)
        {
            this.Head = head ?? throw new ArgumentNullException(nameof(head));
        }

        public NodeParsingUnit Head { get; }


        public ParseResult Process(ITokenStream stream, IParsingContext context)
        {
            IParsingUnit current = this.Head;

            while (true)
            {
                var result = current.Process(stream, context);

                if (result == ParseResult.Success)
                {
                    var nextUnits = current.GetNextUnits();
                    if (nextUnits.Count == 1)
                    {
                        current = nextUnits.Single();
                        continue;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();
            }
        }

        public IReadOnlyList<IParsingUnit> GetNextUnits()
        {
            throw new NotImplementedException();
        }
    }
}
