//using System.Collections.Generic;
//using TauCode.Parsing.Lexing;
//using TauCode.Parsing.Lexing.StandardExtractors;
//using TauCode.Parsing.TinyLisp.TokenExtractors;
//using TauCode.Parsing.Tokens;

//namespace TauCode.Parsing.TinyLisp
//{
//    public class TinyLispLexer : LexerBase
//    {
//        protected override IList<ITokenExtractor> CreateTokenExtractors()
//        {
//            return new ITokenExtractor[]
//            {
//                new TinyLispCommentExtractor(),
//                new TinyLispSymbolExtractor(),
//                new TinyLispPunctuationExtractor(),
//                new IntegerExtractor(new[]
//                {
//                    typeof(PunctuationToken),
//                }),
//                new TinyLispKeywordExtractor(),
//                new TinyLispStringExtractor(),
//            };
//        }
//    }
//}


// todo clean up