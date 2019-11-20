using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Tests.Data
{
    public class PrimaryKeyInfo
    {
        public string Name { get; set; }
        public List<IndexColumnInfo> Columns { get; set; } = new List<IndexColumnInfo>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"    CONSTRAINT [{this.Name}] PRIMARY KEY(");

            throw new NotImplementedException();
        }
    }
}
