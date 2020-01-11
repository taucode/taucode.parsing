using System;
using System.Collections.Generic;
using TauCode.Parsing.Building;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Lab.Building
{
    public class NodeFactoryBaseLab : INodeFactory
    {
        private readonly IDictionary<string, ITextClassLab> _textClasses;

        protected NodeFactoryBaseLab(
            string nodeFamilyName,
            IList<ITextClassLab> textClasses)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
            textClasses = textClasses ?? new List<ITextClassLab>();

            _textClasses = new Dictionary<string, ITextClassLab>();

            foreach (var textClass in textClasses)
            {
                var tag = textClass.Tag;
                if (tag == null || _textClasses.ContainsKey(tag))
                {
                    continue; // won't add it to the collection
                }

                _textClasses.Add(tag, textClass);
            }
        }

        public INodeFamily NodeFamily { get; }

        public virtual INode CreateNode(PseudoList item)
        {
            throw new NotImplementedException();
        }
    }
}
