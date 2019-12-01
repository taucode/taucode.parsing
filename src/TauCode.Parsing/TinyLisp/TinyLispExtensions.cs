using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Utils.Lab;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispExtensions
    {
        public static Element GetSingleKeywordArgument(this PseudoList list, string argumentName, bool allowsAbsence = false)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            if (!TinyLispHelper.IsValidSymbolName(argumentName, true))
            {
                throw new NotImplementedException();
            }

            var wantedKeyword = Symbol.Create(argumentName);
            int wantedIndex;
            var index = list.FindFirstIndexOfLab(wantedKeyword);

            if (
                index < 0 || // not found
                index + 1 >= list.Count // next is keyword
                )
            {
                if (allowsAbsence)
                {
                    return null;
                }
                else
                {
                    throw new NotImplementedException(); // not found
                }
            }
            else
            {
                wantedIndex = index + 1;
            }

            var wantedElement = list[wantedIndex];
            if (wantedElement is Keyword)
            {
                throw new NotImplementedException();
            }

            return wantedElement;
        }

        public static TElement GetSingleKeywordArgument<TElement>(this PseudoList list, string argumentName, bool allowsAbsence = false) where TElement : Element
        {
            var element = list.GetSingleKeywordArgument(argumentName, allowsAbsence);
            if (element is TElement expectedElement)
            {
                return expectedElement;
            }

            throw new NotImplementedException(); // error
        }
        public static PseudoList GetAllKeywordArguments(this PseudoList list, string argumentName, bool allowsAbsence = false)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            if (!TinyLispHelper.IsValidSymbolName(argumentName, true))
            {
                throw new NotImplementedException();
            }

            var wantedKeyword = Symbol.Create(argumentName);
            var index = list.FindFirstIndexOfLab(wantedKeyword);

            if (index == -1)
            {
                if (allowsAbsence)
                {
                    return new PseudoList(); // empty
                }
                else
                {
                    throw new NotImplementedException(); // not found
                }
            }

            index++; // move forward from keyword.
            var startIndex = index;

            while (true)
            {
                if (index == list.Count)
                {
                    break;
                }

                var element = list[index];
                if (element is Keyword)
                {
                    break;
                }

                index++;
            }

            var lastIndex = index - 1;

            var result = new PseudoList();

            for (var i = startIndex; i <= lastIndex; i++)
            {
                result.AddElement(list[i]);
            }

            return result;
        }

        public static bool? GetSingleArgumentAsBool(this PseudoList list, string argumentName)
        {
            var element = list.GetSingleKeywordArgument(argumentName, true);
            if (element == null)
            {
                return null;
            }

            if (element == True.Instance)
            {
                return true;
            }

            if (element == Nil.Instance)
            {
                return false;
            }

            throw new NotImplementedException(); // only T or NIL accepted here.
        }

        // todo: check that '.Single()' doesn't throw.
        public static PseudoList GetFreeArguments(this PseudoList list)
        {
            return list.GetMultipleFreeArgumentSets().Single();
        }

        public static IList<PseudoList> GetMultipleFreeArgumentSets(this PseudoList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new NotImplementedException(); // error.
            }

            var index = 1;
            var result = new List<PseudoList>();

            var startedWithKeyword = false;
            var startIndex = 1;

            while (true)
            {
                if (index == list.Count)
                {
                    if (startIndex == -1)
                    {
                        // nothing to add.
                    }
                    else
                    {
                        // got free args.
                        var firstArgIndex = startIndex;

                        if (startedWithKeyword)
                        {
                            firstArgIndex++;
                        }

                        var lastArgIndex = index - 1;

                        var freeArgsPseudoList = new PseudoList();
                        for (var i = firstArgIndex; i <= lastArgIndex; i++)
                        {
                            freeArgsPseudoList.AddElement(list[i]);
                        }

                        result.Add(freeArgsPseudoList);
                    }

                    break;
                }

                var element = list[index];
                if (element is Keyword)
                {
                    // bumped into keyword
                    if (startIndex == -1)
                    {
                        // reset again.
                        startedWithKeyword = true;
                        index++;
                    }
                    else
                    {
                        // was started, let's check, maybe we can deliver pseudo-list of free args.
                        var delta = index - startIndex;
                        if (delta == 0 || delta == 1)
                        {
                            // got only zero or one arg, won't consider if free.
                            // reset the entire procedure.
                            startedWithKeyword = true;
                            startIndex = -1;
                            index++;
                        }
                        else
                        {
                            // got free args!
                            var firstArgIndex = startIndex;

                            if (startedWithKeyword)
                            {
                                firstArgIndex++;
                            }

                            var lastArgIndex = index - 1;

                            var freeArgsPseudoList = new PseudoList();
                            for (var i = firstArgIndex; i <= lastArgIndex; i++)
                            {
                                freeArgsPseudoList.AddElement(list[i]);
                            }

                            result.Add(freeArgsPseudoList);

                            startIndex = -1;
                            index++;

                            startedWithKeyword = true;
                        }
                    }
                }
                else
                {
                    // bumped into non-keyword.
                    if (startIndex == -1)
                    {
                        // let's start.
                        startIndex = index;
                        index++;
                    }
                    else
                    {
                        // was started, bumped into non-keyword again. good, let's continue.
                        index++;
                    }
                }
            }

            return result;
        }

        public static string GetCarSymbolName(this PseudoList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new NotImplementedException();
            }

            var element = list[0];
            if (element is Symbol symbol)
            {
                return symbol.Name;
            }

            throw new NotImplementedException();
        }

        public static Element GetPseudoLast(this Element shouldBePseudoList)
        {
            if (shouldBePseudoList == null)
            {
                throw new ArgumentNullException(nameof(shouldBePseudoList));
            }

            if (shouldBePseudoList is PseudoList pseudoList)
            {
                return pseudoList.Last();
            }

            throw new NotImplementedException(); // error
        }

        public static PseudoList AsPseudoList(this Element shouldBePseudoList)
        {
            if (shouldBePseudoList == null)
            {
                throw new ArgumentNullException(nameof(shouldBePseudoList));
            }

            if (shouldBePseudoList is PseudoList pseudoList)
            {
                return pseudoList;
            }

            throw new NotImplementedException(); // error
        }
    }
}
