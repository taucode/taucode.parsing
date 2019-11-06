using System;
using System.Collections.Generic;
using TauCode.Parsing.ParsingUnits;
using TauCode.Parsing.ParsingUnits.Impl.Nodes;

namespace TauCode.Parsing
{
    public static class ParsingHelper
    {
        public static bool IsEndOfStream(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            return tokenStream.Position == tokenStream.Tokens.Count;
        }

        public static IToken GetCurrentToken(this ITokenStream tokenStream)
        {
            if (tokenStream.IsEndOfStream())
            {
                throw new NotImplementedException();
            }

            var token = tokenStream.Tokens[tokenStream.Position];
            return token;
        }

        public static void AdvanceStreamPosition(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            tokenStream.Position++;
        }

        public static void IdleTokenProcessor(IToken token, IParsingContext context)
        {
        }

        public static bool IsEndResult(IReadOnlyList<IParsingUnit> result)
        {
            // todo: check args
            var res = result.Count == 1 && result[0] == EndParsingNode.Instance;
            return res;
        }

        public static bool IsNestedInto(this IParsingUnit unit, IParsingBlock possibleSuperOwner)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            if (possibleSuperOwner == null)
            {
                throw new ArgumentNullException(nameof(possibleSuperOwner));
            }

            var currentOwner = unit.Owner;

            while (true)
            {
                if (currentOwner == null)
                {
                    return false;
                }

                if (currentOwner == possibleSuperOwner)
                {
                    return true;
                }

                currentOwner = currentOwner.Owner;
            }
        }

        public static bool IsBlockHeadNode(this IParsingNode node)
        {
            var owner = node.Owner;
            if (owner == null)
            {
                return false;
            }

            return owner.Head == node;
        }
    }
}
