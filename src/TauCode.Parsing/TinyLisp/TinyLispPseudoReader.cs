using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispPseudoReader
    {
        public PseudoList Read(IReadOnlyList<IToken> tokens)
        {
            var list = new PseudoList();
            var pos = 0;

            this.ReadPseudoListContent(list, tokens, ref pos, 0);
            return list;
        }

        private void ReadPseudoListContent(PseudoList list, IReadOnlyList<IToken> tokens, ref int pos, int depth)
        {
            while (true)
            {
                if (pos == tokens.Count)
                {
                    if (depth > 0)
                    {
                        throw new NotImplementedException(); // todo
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
                                throw new NotImplementedException();
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
                else if (token is StringToken stringToken)
                {
                    var element = new StringAtom(stringToken.Value);
                    list.AddElement(element);
                    pos++;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}