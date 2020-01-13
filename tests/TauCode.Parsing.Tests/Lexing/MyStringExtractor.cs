//using System.Collections.Generic;
//using TauCode.Parsing.Lexing.StandardExtractors;
//using TauCode.Parsing.TextDecorations;
//using TauCode.Parsing.TextProcessing;

//namespace TauCode.Parsing.Tests.Lexing
//{
//    public class MyStringExtractor : GammaStringExtractorBase
//    {
//        public MyStringExtractor()
//            : base('"',
//                '"',
//                false,
//                DoubleQuoteTextDecoration.Instance, 
//                null)
//        {
//        }

//        protected override void ConsumeSubPayload(IPayload subPayload)
//        {
//            if (subPayload is CharPayload charPayload)
//            {
//                this.StringBuilder.Append(charPayload.Char);
//            }   
//        }


//        protected override IList<ITextProcessor> CreateEscapeProcessors()
//        {
//            return new List<ITextProcessor>
//            {
//                new CLangEscapeProcessor(),
//            };
//        }
//    }
//}
