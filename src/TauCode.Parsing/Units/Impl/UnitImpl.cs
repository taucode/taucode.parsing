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
                        throw new NotImplementedException(); // suspicious: owner is already null
                    }
                    else
                    {
                        if (value == this)
                        {
                            throw new NotImplementedException(); // cannot be owner of self.
                        }

                        _owner = value;
                    }
                }
                else
                {
                    throw new NotImplementedException(); // owner already set.
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
