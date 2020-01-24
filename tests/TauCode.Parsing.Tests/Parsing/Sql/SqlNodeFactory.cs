using System.Collections.Generic;
using TauCode.Parsing.Building;
using TauCode.Parsing.Tests.Parsing.Sql.TextClasses;
using TauCode.Parsing.TextClasses;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlNodeFactory : NodeFactoryBase
    {
        public SqlNodeFactory()
            : base(
                "Test-SQLite",
                new List<ITextClass>
                {
                    WordTextClass.Instance,
                    StringTextClass.Instance,
                    SqlIdentifierClass.Instance,
                },
                false)
        {
        }
    }
}
