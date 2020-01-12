﻿using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Old.Building;
using TauCode.Parsing.Old.Nodes;
using TauCode.Parsing.Old.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.Tokens;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.Tests.Parsing.Cli.Data.Entries;
using TauCode.Parsing.Tests.Parsing.Cli.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Old.Tests.Parsing.Cli
{
    public class CliNodeFactory : OldNodeFactoryBase
    {
        #region Constructor

        public CliNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        #endregion

        #region Overridden

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "WORKER":
                    node = this.CreateWorkerNode(item);
                    break;

                case "KEY-WITH-VALUE":
                    node = this.CreateKeyEqualsValueNode(item);
                    break;

                case "KEY-VALUE-PAIR":
                    node = this.CreateKeyValuePairNode(item);
                    break;

                case "KEY":
                    node = this.CreateKeyNode(item);
                    break;

                default:
                    throw new CliException($"Unexpected symbol: '{car}'");
            }

            return node;
        }

        #endregion

        #region Node Creators

        private INode CreateWorkerNode(PseudoList item)
        {
            var verbs = item
                .GetAllKeywordArguments(":verbs")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            INode node = new OldMultiTextNode(
                verbs,
                new IOldTextClass[] { OldTermTextClass.Instance },
                this.ProcessAlias,
                this.NodeFamily,
                item.GetItemName());

            var alias = item.GetSingleKeywordArgument<Symbol>(":worker-name").Name;
            node.Properties["worker-name"] = alias;

            return node;
        }

        private INode CreateKeyNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            var node = new OldMultiTextNode(
                keyNames,
                new IOldTextClass[] { OldKeyTextClass.Instance, },
                this.ProcessKey,
                this.NodeFamily,
                item.GetItemName());
            node.Properties["alias"] = alias;

            return node;
        }

        private INode CreateKeyEqualsValueNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            ActionNode keyNameNode = new OldMultiTextNode(
                keyNames,
                new IOldTextClass[] { OldKeyTextClass.Instance },
                this.ProcessKeySucceededByValue,
                this.NodeFamily,
                item.GetItemName());
            keyNameNode.Properties["alias"] = alias;

            INode equalsNode = new ExactPunctuationNode('=', null, this.NodeFamily, null);
            INode choiceNode = this.CreateKeyChoiceNode(item);

            keyNameNode.EstablishLink(equalsNode);
            equalsNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateKeyValuePairNode(PseudoList item)
        {
            var alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList();

            ActionNode keyNameNode = new OldMultiTextNode(
                keyNames,
                new IOldTextClass[] { OldKeyTextClass.Instance },
                this.ProcessKeySucceededByValue,
                this.NodeFamily,
                item.GetItemName());
            keyNameNode.Properties["alias"] = alias;

            INode choiceNode = this.CreateKeyChoiceNode(item);

            keyNameNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateKeyChoiceNode(PseudoList item)
        {
            var keyValuesSubform = item.GetSingleKeywordArgument(":key-values");
            if (keyValuesSubform.GetCarSymbolName() != "CHOICE")
            {
                throw new CliException("'CHOICE' symbol expected.");
            }

            var classes = keyValuesSubform.GetAllKeywordArguments(":classes").ToList();
            var values = keyValuesSubform.GetAllKeywordArguments(":values").ToList();

            var anyText = values.Count == 1 && values.Single().Equals(Symbol.Create("*"));
            string[] textValues;

            if (anyText)
            {
                textValues = null;
            }
            else
            {
                textValues = values.Select(x => ((StringAtom)x).Value).ToArray();
            }

            var textClasses = classes.Select(x => this.ParseTextClass(((Symbol)x).Name));

            INode choiceNode;

            if (textValues == null)
            {
                choiceNode = new OldTextNode(
                    textClasses,
                    ProcessKeyChoice,
                    this.NodeFamily,
                    null);
            }
            else
            {
                choiceNode = new OldMultiTextNode(
                    textValues,
                    textClasses,
                    ProcessKeyChoice,
                    this.NodeFamily,
                    null);
            }

            return choiceNode;
        }

        #endregion

        #region Node Actions

        private void ProcessAlias(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            command.WorkerName = actionNode.Properties["worker-name"];
        }

        private void ProcessKey(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new KeyCliCommandEntry
            {
                Alias = actionNode.Properties["alias"],
                Key = ((OldTextToken)token).Text,
            };
            subCommand.Entries.Add(entry);
        }

        private void ProcessKeySucceededByValue(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new KeyValueCliCommandEntry
            {
                Alias = actionNode.Properties["alias"],
                Key = ((OldTextToken)token).Text,
            };
            subCommand.Entries.Add(entry);
        }

        private void ProcessKeyChoice(ActionNode actionNode, IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = (KeyValueCliCommandEntry)subCommand.Entries.Last();
            entry.Value = ((OldTextToken)token).Text;
        }

        #endregion

        #region Misc

        private IOldTextClass ParseTextClass(string textClassSymbolName)
        {
            switch (textClassSymbolName)
            {
                case "STRING":
                    return OldStringTextClass.Instance;

                case "TERM":
                    return OldTermTextClass.Instance;

                case "KEY":
                    return OldKeyTextClass.Instance;

                case "PATH":
                    return OldPathTextClass.Instance;

                default:
                    throw new CliException($"Unexpected text class designating symbol: '{textClassSymbolName}'.");
            }
        }

        #endregion
    }
}