//using System;

//namespace TauCode.Parsing
//{
//    public abstract class TextClassBase : ITextClass
//    {
//        public string TryConvertFrom(string text, ITextClass anotherTextClass)
//        {
//            if (text == null)
//            {
//                throw new ArgumentNullException(nameof(text));
//            }

//            if (anotherTextClass == null)
//            {
//                throw new ArgumentNullException(nameof(anotherTextClass));
//            }

//            return this.TryConvertFromImpl(text, anotherTextClass);
//        }

//        protected abstract string TryConvertFromImpl(string text, ITextClass anotherTextClass);
//    }
//}
