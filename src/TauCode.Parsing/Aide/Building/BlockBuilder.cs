using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder
    {
        private readonly BlockBuildingContext _context;

        public BlockBuilder(BlockBuildingContext context)
        {
            _context = context;
        }

        public void Build()
        {
            var block = this.BuildBlockImpl();



            throw new NotImplementedException();


        }

        private IBlock BuildBlockImpl()
        {
            var mold = _context.Mold;
            var content = mold.Content;
            this.BuildContent(content);

            throw new NotImplementedException();
        }

        private void BuildContent(Content content)
        {
            var list = new List<IUnit>();
            List<IUnit> previousUnlinkedUnits = null;

            for (var i = 0; i < content.UnitResultCount; i++)
            {
                var unitResult = content[i];
                var units = this.BuildUnits(unitResult);

                var head = units[0]; // 0th must be head.
                if (previousUnlinkedUnits != null)
                {
                    foreach (var previousUnlinkedUnit in previousUnlinkedUnits)
                    {
                        if (previousUnlinkedUnit is INode node)
                        {
                            node.AddLink(head);
                        }
                        else if (previousUnlinkedUnit is IBlock block)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }

                previousUnlinkedUnits = GetUnlinkedUnits(units);
            }

            throw new NotImplementedException();
        }

        private List<IUnit> GetUnlinkedUnits(List<IUnit> units)
        {
            var list = new List<IUnit>();

            foreach (var unit in units)
            {
                if (unit is INode node)
                {
                    if (node.Links.Count == 0)
                    {
                        list.Add(node);
                    }
                }
                else if (unit is IBlock block)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return list;
        }

        private List<IUnit> BuildUnits(UnitResult unitResult)
        {
            throw new NotImplementedException();

            //List<IUnit> units = new List<IUnit>();
            //INode node;

            //if (unitResult is SyntaxElementResult syntaxElementResult)
            //{
            //    switch (syntaxElementResult.SyntaxElement)
            //    {
            //        case SyntaxElement.Identifier:
            //            node = new IdentifierNode(ParsingHelper.IdleTokenProcessor, syntaxElementResult.SourceNodeName);
            //            units.Add(node);
            //            break;
            //            //return new List<IUnit>(new[] { unit });

            //        case SyntaxElement.Link:
            //            node = new SplittingNode(syntaxElementResult.SourceNodeName);
            //            var holdingBlockName = GetLinkHoldingBlockName(syntaxElementResult);
            //            var referencedUnitName = GetReferencedUnitName(syntaxElementResult);
            //            _context.AddUnitReference((Node)node, holdingBlockName, referencedUnitName);
            //            return new List<IUnit>(new[] { unit });

            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}
            //else if (unitResult is OptionalResult optionalResult)
            //{
            //    var splitter = new SplittingNode(optionalResult.SourceNodeName);
                
            //    throw new NotImplementedException();
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
        }

        private string GetReferencedUnitName(SyntaxElementResult syntaxElementResult)
        {
            if (syntaxElementResult.Arguments.Count == 1)
            {
                return syntaxElementResult.Arguments.Single();
            }
            else if (syntaxElementResult.Arguments.Count == 2)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string GetLinkHoldingBlockName(SyntaxElementResult syntaxElementResult)
        {
            if (syntaxElementResult.Arguments.Count == 1)
            {
                return _context.BlockName;
            }
            else if (syntaxElementResult.Arguments.Count == 2)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
