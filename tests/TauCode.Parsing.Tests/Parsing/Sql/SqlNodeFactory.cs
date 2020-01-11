﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.Nodes;
using TauCode.Parsing.Lab.TextClasses;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Sql.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlNodeFactory : NodeFactoryBase
    {
        public SqlNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "EXACT-TEXT":
                    node = new ExactTextNodeLab(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value,
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-TEXT":
                    node = new TextNodeLab(
                        this.ParseTextClasses(item.GetAllKeywordArguments(":classes")),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "PUNCTUATION":
                    node = new ExactPunctuationNode(
                        item.GetSingleKeywordArgument<StringAtom>(":value").Value.Single(),
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                case "SOME-INT":
                    node = new IntegerNode(
                        null,
                        this.NodeFamily,
                        item.GetItemName());
                    break;

                default:
                    throw new NotSupportedException();
            }

            return node;
        }

        private IEnumerable<ITextClassLab> ParseTextClasses(PseudoList arguments)
        {
            var textClasses = new List<ITextClassLab>();

            foreach (var argument in arguments)
            {
                ITextClassLab textClass;
                var symbolElement = (Symbol)argument;

                switch (symbolElement.Name)
                {
                    case "WORD":
                        textClass = WordTextClassLab.Instance;
                        break;

                    case "IDENTIFIER":
                        textClass = SqlIdentifierClass.Instance;
                        break;

                    case "STRING":
                        textClass = StringTextClassLab.Instance;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                textClasses.Add(textClass);
            }

            return textClasses;
        }
    }
}
