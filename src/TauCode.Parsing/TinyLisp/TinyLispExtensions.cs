using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Utils.Lab;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispExtensions
    {
        public static Element GetSingleKeywordArgument(
            this Element shouldBePseudoList,
            string argumentName,
            bool absenceIsAllowed = false)
        {
            if (shouldBePseudoList == null)
            {
                throw new ArgumentNullException(nameof(shouldBePseudoList));
            }

            var list = shouldBePseudoList as PseudoList;

            if (list == null)
            {
                throw new ArgumentException(
                    $"Argument is not of type '{typeof(PseudoList).FullName}'.",
                    nameof(shouldBePseudoList));
            }

            if (argumentName == null)
            {
                throw new ArgumentNullException(nameof(argumentName));
            }

            if (!TinyLispHelper.IsValidSymbolName(argumentName, true))
            {
                throw new ArgumentException($"'{argumentName}' is not a valid keyword.", nameof(argumentName));
            }

            var wantedKeyword = Symbol.Create(argumentName);
            int wantedIndex;
            var index = list.FindFirstIndexOfLab(wantedKeyword);

            if (index < 0)
            {
                if (absenceIsAllowed)
                {
                    return null;
                }
                else
                {
                    throw new TinyLispException($"No argument for keyword '{argumentName}'.");
                }
            }
            else
            {
                wantedIndex = index + 1;
            }

            if (wantedIndex == list.Count)
            {
                throw new TinyLispException($"Keyword '{argumentName}' was found, but at the end of the list.");
            }

            var wantedElement = list[wantedIndex];
            if (wantedElement is Keyword)
            {
                throw new TinyLispException($"Keyword '{argumentName}' was found, but next element is a keyword too.");
            }

            return wantedElement;
        }

        public static TElement GetSingleKeywordArgument<TElement>(
            this Element shouldBePseudoList,
            string argumentName,
            bool absenceIsAllowed = false) where TElement : Element
        {
            var element = shouldBePseudoList.GetSingleKeywordArgument(argumentName, absenceIsAllowed);
            if (element == null)
            {
                return null;
            }

            if (element is TElement expectedElement)
            {
                return expectedElement;
            }

            throw new NotImplementedException(); // error
        }

        public static PseudoList GetAllKeywordArguments(
            this Element shouldBePseudoList,
            string argumentName,
            bool absenceIsAllowed = false)
        {
            if (shouldBePseudoList == null)
            {
                throw new ArgumentNullException(nameof(shouldBePseudoList));
            }

            var list = shouldBePseudoList as PseudoList;

            if (list == null)
            {
                throw new NotImplementedException(); // error
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
                if (absenceIsAllowed)
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

        public static bool? GetSingleArgumentAsBool(this Element shouldPseudoBeList, string argumentName)
        {
            var element = shouldPseudoBeList.GetSingleKeywordArgument(argumentName, true);
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
        public static PseudoList GetFreeArguments(this Element shouldPseudoBeList)
        {
            return shouldPseudoBeList.GetMultipleFreeArgumentSets().Single();
        }

        public static IList<PseudoList> GetMultipleFreeArgumentSets(this Element shouldPseudoBeList)
        {
            if (shouldPseudoBeList == null)
            {
                throw new ArgumentNullException(nameof(shouldPseudoBeList));
            }

            var list = shouldPseudoBeList as PseudoList;

            if (list == null)
            {
                throw new NotImplementedException(); // error
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

        public static string GetCarSymbolName(this Element shouldPseudoBeList)
        {
            if (shouldPseudoBeList == null)
            {
                throw new ArgumentNullException(nameof(shouldPseudoBeList));
            }

            if (shouldPseudoBeList is PseudoList list)
            {
                if (list.Count == 0)
                {
                    throw new NotImplementedException();
                }

                var element = list[0];
                if (element is Symbol symbol)
                {
                    return symbol.Name;
                }
            }

            throw new NotImplementedException(); // todo error
        }

        public static PseudoList AsPseudoList(this Element shouldBePseudoList) =>
            shouldBePseudoList.AsElement<PseudoList>();

        public static TElement AsElement<TElement>(this Element element) where TElement : Element
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element is TElement wantedElement)
            {
                return wantedElement;
            }

            throw new ArgumentException(
                $"Argument is expected to be of type '{typeof(PseudoList).FullName}', but was of type '{element.GetType().FullName}'.",
                nameof(element));
        }
    }
}
