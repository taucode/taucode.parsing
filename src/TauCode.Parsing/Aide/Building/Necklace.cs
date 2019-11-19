using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Building
{
    public class Necklace
    {
        public Necklace()
        {
            this.Items = new List<NodeBuilder>();
        }

        public NodeBuilder Left { get; private set; }
        public NodeBuilder Right { get; private set; }
        public List<NodeBuilder> Items { get; private set; }

        public void AddItem(NodeBuilder item, bool makeLinks)
        {
            this.Items.Add(item);

            if (this.Left == null)
            {

                this.Left = item;
                this.Right = item;
            }
            else
            {
                var oldRight = this.Right;
                this.Right = item;

                if (makeLinks)
                {
                    this.MakeLinks(oldRight, this.Right);
                }
            }
        }

        private void MakeLinks(NodeBuilder from, NodeBuilder to)
        {
            var arguments = from.Arguments;

            if (arguments.Count == 0)
            {
                from.AddExplicitLink(to);
            }
            else
            {
                foreach (var argument in arguments)
                {
                    if (argument == "next")
                    {
                        from.AddExplicitLink(to);
                    }
                    else
                    {
                        from.AddLinkClaim(argument);
                    }
                }
            }
        }

        public void AppendNecklace(Necklace subNecklace)
        {
            this.Items.AddRange(subNecklace.Items);

            if (this.Left == null)
            {
                this.Left = subNecklace.Left;
                this.Right = subNecklace.Right;
            }
            else
            {
                this.MakeLinks(this.Right, subNecklace.Left);
                this.Right = subNecklace.Right;
            }
        }

        public void InsertNecklace(NodeBuilder left, NodeBuilder to, Necklace subNecklace)
        {
            this.Items.AddRange(subNecklace.Items);

            left.AddExplicitLink(subNecklace.Left);
            subNecklace.Right.AddExplicitLink(Right);
        }
    }
}
