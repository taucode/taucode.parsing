using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Old;

namespace TauCode.Parsing.Tests.Parsing.Sql.TextClasses
{
    public class SqlIdentifierClass : IOldTextClass
    {
        private readonly HashSet<string> _reservedWords;

        public SqlIdentifierClass(IList<string> reservedWords)
        {
            _reservedWords = new HashSet<string>(reservedWords.Select(x => x.ToUpperInvariant()));
        }
    }
}
