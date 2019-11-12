using System;
using System.Collections.Generic;
using System.Diagnostics;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Units.Impl
{
    [DebuggerDisplay("Name = '{" + nameof(Name) + "}'")]
    public abstract class UnitImpl : IUnit
    {
        #region Fields

        private string _name;
        private IBlock _owner;

        #endregion

        #region Constructor

        protected UnitImpl(string name)
        {
            // Name can be null, but you got to say it explicitly. Unnamed units are evil.

            this.Name = name;
        }

        #endregion

        #region Protected

        protected void CheckNotFinalized()
        {
            if (IsFinalized)
            {
                throw new ParserException("Unit is finalized.");
            }
        }

        protected void CheckFinalized()
        {
            if (!this.IsFinalized)
            {
                throw new ParserException($"Unit is already finalized. {this.ToUnitDiagnosticsString()}");
            }
        }

        #endregion

        #region Polymorph

        protected abstract IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context);

        protected virtual void OnBeforeFinalize()
        {
            // idle
        }

        protected virtual void FinalizeUnitImpl()
        {
            // idle
        }

        #endregion

        #region IUnit Members

        public string Name
        {
            get => _name;
            set
            {
                this.CheckNotFinalized();
                _name = value;
            }
        }

        public IBlock Owner
        {
            get => _owner;
            internal set
            {
                this.CheckNotFinalized();

                if (_owner == null)
                {
                    if (value == null)
                    {
                        throw new ParserException($"Suspicious operation: Owner is null until not initialized. {this.ToUnitDiagnosticsString()}");
                    }
                    else
                    {
                        if (value == this)
                        {
                            throw new ParserException($"Unit cannot be owner of self. {this.ToUnitDiagnosticsString()}");
                        }

                        _owner = value;
                    }
                }
                else
                {
                    throw new ParserException($"Owner already set. {this.ToUnitDiagnosticsString()}");
                }
            }
        }

        public bool IsFinalized { get; private set; }

        public void FinalizeUnit()
        {
            this.CheckNotFinalized();
            this.OnBeforeFinalize();

            this.FinalizeUnitImpl();
            
            IsFinalized = true;
        }

        public IReadOnlyList<IUnit> Process(ITokenStream stream, IContext context)
        {
            this.CheckFinalized();

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var list = this.ProcessImpl(stream, context);
            return list;
        }
        
        #endregion
    }
}
