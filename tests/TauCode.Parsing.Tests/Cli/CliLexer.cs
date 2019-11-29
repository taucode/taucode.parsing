using System;
using TauCode.Parsing.Lexer2;

namespace TauCode.Parsing.Tests.Cli
{
    public class CliLexer : LexerBase
    {
        public CliLexer()
            : base(null, null)
        {
        }

        protected override void InitTokenExtractors()
        {
            throw new NotImplementedException();
            //AideSyntaxTokenExtractor aideSyntaxTokenExtractor;

            //TokenExtractorBase[] tokenExtractors =
            //{
            //    new CommentExtractor(),
            //    new WordExtractor(LexerHelper.StandardSpaceChars),
            //    aideSyntaxTokenExtractor = new AideSyntaxTokenExtractor(),
            //    new AideSymbolExtractor(LexerHelper.StandardSpaceChars),
            //};

            //foreach (var te in tokenExtractors)
            //{
            //    te.AddSuccessors(tokenExtractors);
            //    this.AddTokenExtractor(te);
            //}

            //var nameReferenceExtractor = new AideNameReferenceExtractor(LexerHelper.StandardSpaceChars);
            //this.AddTokenExtractor(nameReferenceExtractor);

            //nameReferenceExtractor.AddSuccessors(aideSyntaxTokenExtractor);
            //aideSyntaxTokenExtractor.AddSuccessors(nameReferenceExtractor);
        }
    }
}
