//using System;
//using TauCode.Parsing.Tokens;

//namespace TauCode.Parsing.Units.Impl.Nodes
//{
//    public class EnumNode<TEnum> : ProcessingNode where TEnum : struct
//    {
//        public EnumNode(Action<IToken, IContext> processor, string name)
//            : base(processor, name)
//        {
//        }

//        protected override bool IsAcceptableToken(IToken token)
//        {
//            return token is EnumToken<TEnum>;
//        }
//    }
//}
