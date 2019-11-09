using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl
{
    public class Wrapper : UnitImpl, IWrapper
    {
        #region Fields

        private IUnit _internal;

        #endregion

        #region Overridden

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnBeforeFinalize()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IWrapper Members

        public IUnit Internal
        {
            get => _internal;
            set
            {
                this.CheckNotFinalized();

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _internal = value;
            }
        }

        #endregion
    }
}
