using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.Tests.Parsing.Cli.Exceptions;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliNodeFactory : NodeFactoryBase
    {
        public CliNodeFactory(
            string nodeFamilyName)
            : base(
                nodeFamilyName,
                new List<ITextClass>
                {
                    KeyTextClass.Instance,
                    PathTextClass.Instance,
                    TermTextClass.Instance,
                    StringTextClass.Instance,
                },
                true)
        {
        }

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName().ToLowerInvariant();
            if (car == "worker")
            {
                INode workerNode;

                var workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
                if (workerName == null)
                {
                    throw new CliException("Worker name is null.");
                }
                else
                {
                    workerNode = new MultiTextNode(
                        item
                            .GetAllKeywordArguments(":verbs")
                            .Cast<StringAtom>()
                            .Select(x => x.Value)
                            .ToList(),
                        new ITextClass[]
                        {
                            TermTextClass.Instance,
                        },
                        true,
                        WorkerAction,
                        this.NodeFamily,
                        $"Worker Node. Name: [{workerName}]");

                    workerNode.Properties["worker-name"] = workerName;
                }

                return workerNode;
            }

            var node = base.CreateNode(item);
            if (node is FallbackNode)
            {
                return node;
            }

            if (!(node is ActionNode ))
            {
                throw new CliException("ActionNode was expected to be created.");
            }

            var actionNode = (ActionNode)base.CreateNode(item);

            if (actionNode == null)
            {
                throw new CliException($"Could not build node for item '{car}'.");
            }

            var action = item.GetSingleKeywordArgument<Symbol>(":action", true)?.Name?.ToLowerInvariant();
            string alias;

            switch (action)
            {
                case "key":
                    actionNode.Action = KeyAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    actionNode.Properties["alias"] = alias;
                    break;

                case "value":
                    actionNode.Action = ValueAction;
                    break;

                case "option":
                    actionNode.Action = OptionAction;
                    break;

                case "argument":
                    actionNode.Action = ArgumentAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    actionNode.Properties["alias"] = alias;
                    break;

                default:
                    throw new CliException($"Keyword ':action' is missing for item '{car}'.");
            }

            return actionNode;
        }

        protected override Func<FallbackNode, IToken, IResultAccumulator, bool> CreateFallbackPredicate(string nodeName)
        {
            if (nodeName.ToLowerInvariant() == "bad-key-fallback")
            {
                return BadKeyPredicate;
            }

            throw new ArgumentException($"Unknown fallback name: '{nodeName}'.", nameof(nodeName));

        }

        private static bool BadKeyPredicate(FallbackNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            return
                node.Name?.ToLowerInvariant() == "bad-key-fallback" &&
                token is TextToken textToken &&
                textToken.Text.StartsWith("-");
        }

        private void WorkerAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator.Count == 0)
            {
                var command = new CliCommand
                {
                    WorkerName = node.Properties["worker-name"],
                };

                resultAccumulator.AddResult(command);
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                command.WorkerName = node.Properties["worker-name"];
            }
        }

        private void KeyAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new CliCommandEntry
            {
                Alias = node.Properties["alias"],
            };
            command.Entries.Add(entry);
        }

        private void ValueAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = command.Entries.Last();
            var textToken = (TextToken)token;
            entry.Value = textToken.Text;
        }

        private void OptionAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotSupportedException(); // will implement in real CLI.
        }

        private void ArgumentAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotSupportedException(); // will implement in real CLI.
        }
    }
}
