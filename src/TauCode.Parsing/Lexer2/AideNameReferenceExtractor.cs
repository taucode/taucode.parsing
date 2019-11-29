//using System.Collections.Generic;
//using TauCode.Parsing.Aide;
//using TauCode.Parsing.Tokens;

//namespace TauCode.Parsing.Lexer2
//{
//    public class AideNameReferenceExtractor : TokenExtractorBase
//    {
//        private static readonly HashSet<char> NameChars = new HashSet<char>(GetNameChars()); // todo: lot of copy-paste

//        private static char[] GetNameChars()
//        {
//            var list = new List<char>();
//            list.AddCharRange('a', 'z');
//            list.AddCharRange('A', 'Z');
//            list.Add('_');
//            return list.ToArray();
//        }

//        public AideNameReferenceExtractor(char[] spaceChars)
//            : base(spaceChars, new[] { '*' })
//        {
//        }

//        protected override void Reset()
//        {
//            //throw new NotImplementedException();
//        }

//        protected override IToken ProduceResult()
//        {
//            var resultString = this.ExtractResultString();
//            var value = resultString.Substring(1);
//            if (value.Length == 0)
//            {
//                return null;
//            }

//            return new SpecialStringToken(AideHelper.AideNameReferenceClass, value);
//        }

//        protected override TestCharResult TestCurrentChar()
//        {
//            var localPos = this.GetLocalPosition();
//            var c = this.GetCurrentChar();

//            if (localPos == 0)
//            {
//                return TestCharResult.Continue;
//            }

//            if (NameChars.Contains(c))
//            {
//                return TestCharResult.Continue;
//            }

//            return TestCharResult.End;
//        }
//    }
//}
