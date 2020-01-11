using System;
using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Sql.TextClasses
{
    public class SqlIdentifierClass : ITextClassLab
    {
        //private readonly HashSet<string> _reservedWords;

        public static SqlIdentifierClass Instance { get; } = new SqlIdentifierClass();

        private SqlIdentifierClass(/*IList<string> reservedWords*/)
        {
            //throw new NotImplementedException();
            //_reservedWords = new HashSet<string>(reservedWords.Select(x => x.ToUpperInvariant()));
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
