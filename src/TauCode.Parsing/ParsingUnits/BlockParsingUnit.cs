using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.ParsingUnits
{
    public class BlockParsingUnit : IParsingUnit
    {
        private readonly HashSet<IParsingUnit> _enlisted;

        public BlockParsingUnit(IParsingUnit head)
        {
            _enlisted = new HashSet<IParsingUnit>();

            this.Head = head ?? throw new ArgumentNullException(nameof(head));
            this.Enlist(head);
        }

        public IParsingUnit Head { get; }

        public void Enlist(params IParsingUnit[] parsingUnits)
        {
            // todo: check args

            foreach (var parsingUnit in parsingUnits)
            {
                if (parsingUnit == this)
                {
                    throw new NotImplementedException();
                }

                if (parsingUnit == EndNodeParsingUnit.Instance)
                {
                    throw new NotImplementedException();
                }

                _enlisted.Add(parsingUnit);
            }
        }

        private IReadOnlyList<IParsingUnit> Checked(List<IParsingUnit> units)
        {
            if (units == null)
            {
                throw new NotImplementedException();
            }

            if (units.Count == 0)
            {
                throw new NotImplementedException();
            }

            if (units.Contains(EndNodeParsingUnit.Instance))
            {
                throw new NotImplementedException(); // todo: optimize
            }

            return units;
        }

        private bool IsEnlisted(IParsingUnit parsingUnit)
        {
            // todo: check args
            return _enlisted.Contains(parsingUnit);
        }

        public IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
        {
            List<IParsingUnit> challengers = new List<IParsingUnit>(new[] { this.Head });
            var oldPosition = stream.Position;

            while (true)
            {
                if (challengers.Count == 0)
                {
                    stream.Position = oldPosition;
                    return null;
                }

                foreach (var challenger in challengers)
                {
                    var result = challenger.Process(stream, context);

                    if (result != null)
                    {
                        if (result.Count == 0)
                        {
                            throw new NotImplementedException();
                        }
                        else if (result.Count == 1)
                        {
                            if (this.IsEnlisted(result[0]))
                            {
                                challengers = result.ToList(); // todo: do something (optimize)
                                continue;
                            }
                            else
                            {
                                return Checked(result.ToList()); // todo: do something (optimize)
                            }
                        }
                        else // result.Count > 1
                        {
                            while (true)
                            {
                                var enlisted = result.Where(this.IsEnlisted).ToList();
                                var notEnlisted = result.Where(x => !this.IsEnlisted(x)).ToList(); // todo: optimize

                                if (enlisted.Count > 0)
                                {
                                    // I prefer own units before outer
                                    result = null;
                                    foreach (var unit in enlisted)
                                    {
                                        var resultOfEnlisted = unit.Process(stream, context);

                                        if (resultOfEnlisted != null)
                                        {
                                            result = resultOfEnlisted;
                                            break;
                                        }
                                    }

                                    if (result == null)
                                    {
                                        throw new NotImplementedException();
                                    }
                                    else
                                    {
                                        // Own node got some result!
                                        if (ParsingHelper.IsEndResult(result))
                                        {
                                            return result;
                                        }
                                        else
                                        {
                                            // go on.
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    return Checked(notEnlisted);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
