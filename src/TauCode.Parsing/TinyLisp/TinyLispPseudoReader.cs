using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispPseudoReader
    {
        public PseudoList Read(IList<IToken> tokens)
        {
            var list = new PseudoList();
            var pos = 0;

            this.ReadPseudoListContent(list, tokens, ref pos, 0);
            return list;
        }

        private void ReadPseudoListContent(PseudoList list, IList<IToken> tokens, ref int pos, int depth)
        {
            while (true)
            {
                if (pos == tokens.Count)
                {
                    if (depth > 0)
                    {
                        throw new TinyLispException("Unclosed form.");
                    }
                    else
                    {
                        return;
                    }
                }

                var token = tokens[pos];
                if (token is LispPunctuationToken punctuationToken)
                {
                    switch (punctuationToken.Value)
                    {
                        case Punctuation.RightParenthesis:
                            if (depth == 0)
                            {
                                throw new TinyLispException("Unexpected ')'.");
                            }
                            else
                            {
                                pos++;
                                return;
                            }

                        case Punctuation.LeftParenthesis:
                            pos++;
                            var innerList = new PseudoList();
                            this.ReadPseudoListContent(innerList, tokens, ref pos, depth + 1);
                            list.AddElement(innerList);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (token is KeywordToken keywordToken)
                {
                    var element = Symbol.Create(keywordToken.Keyword);
                    list.AddElement(element);
                    pos++;
                }
                else if (token is LispSymbolToken symbolToken)
                {
                    var element = Symbol.Create(symbolToken.SymbolName);
                    list.AddElement(element);
                    pos++;
                }
                else if (token is TextToken textToken && textToken.Class is StringTextClass)
                {
                    var element = new StringAtom(textToken.Text);
                    list.AddElement(element);
                    pos++;
                }
                else
                {
                    throw new TinyLispException($"Could not read token of type '{token.GetType().FullName}'.");
                }
            }
        }
    }
}
