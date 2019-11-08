using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Parsing.ParsingUnits.Impl
{
    [DebuggerDisplay("Name = '{" + nameof(Name) + "}'")]
    public abstract class ParsingUnitImpl : IParsingUnit
    {
        #region Fields

        private string _name;
        private IParsingBlock _owner;

        #endregion

        #region Constructor

        internal ParsingUnitImpl()
        {
        }

        #endregion

        #region Protected

        protected void CheckNotFinalized()
        {
            if (IsFinalized)
            {
                throw new NotImplementedException(); // todo: finalized.
            }
        }

        protected void CheckFinalized()
        {
            if (!this.IsFinalized)
            {
                throw new NotImplementedException(); // todo: finalized.
            }
        }

        #endregion

        #region Polymorph

        protected abstract IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context);

        protected virtual void OnBeforeFinalize()
        {
            // idle
        }

        protected virtual void FinalizeUnitImpl()
        {
            // idle
        }

        #endregion

        #region IParsingUnit Members

        public string Name
        {
            get => _name;
            set
            {
                this.CheckNotFinalized();
                _name = value;
            }
        }

        public IParsingBlock Owner
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

        public IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context)
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
