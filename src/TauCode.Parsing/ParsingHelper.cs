using System;

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

        //public static IToken GetCurrentToken(this ITokenStream tokenStream)
        //{
        //    if (tokenStream.IsEndOfStream())
        //    {
        //        throw new ParserException("Unexpected end of token stream.");
        //    }

        //    var token = tokenStream.Tokens[tokenStream.Position];
        //    return token;
        //}

        public static void AdvanceStreamPosition(this ITokenStream tokenStream)
        {
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            tokenStream.Position++;
        }

        //public static void IdleTokenProcessor(IToken token, IContext context)
        //{
        //}

        //public static bool IsEndResult(IReadOnlyCollection<IUnit> result)
        //{
        //    if (result == null)
        //    {
        //        throw new ArgumentNullException(nameof(result));
        //    }

        //    var res = result.Count == 1 && result.Single() == EndNode.Instance;
        //    return res;
        //}

        //public static bool IsNestedInto(this IUnit unit, IBlock possibleSuperOwner)
        //{
        //    if (unit == null)
        //    {
        //        throw new ArgumentNullException(nameof(unit));
        //    }

        //    if (possibleSuperOwner == null)
        //    {
        //        throw new ArgumentNullException(nameof(possibleSuperOwner));
        //    }

        //    var currentOwner = unit.Owner;

        //    while (true)
        //    {
        //        if (currentOwner == null)
        //        {
        //            return false;
        //        }

        //        if (currentOwner == possibleSuperOwner)
        //        {
        //            return true;
        //        }

        //        currentOwner = currentOwner.Owner;
        //    }
        //}

        //public static bool IsBlockHeadNode(this INode node)
        //{
        //    var owner = node.Owner;
        //    if (owner == null)
        //    {
        //        return false;
        //    }

        //    return owner.Head == node;
        //}

        //internal static string ToUnitDiagnosticsString(this IUnit unit)
        //{
        //    var diag = $"Unit: type is {unit.GetType().FullName}, name is '{unit.Name}'.";
        //    return diag;
        //}
        //public static void IdleAction(IToken token, IResultAccumulator resultAccumulator)
        //{
        //    // idle
        //}
    }
}
