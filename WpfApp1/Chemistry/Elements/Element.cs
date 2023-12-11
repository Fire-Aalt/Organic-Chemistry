using System.Collections.Generic;
using System.Windows.Media;

namespace WpfApp1.Chemistry.Elements
{
    public abstract class Element
    {
        public abstract string Symbol { get; }
        public abstract int Valency { get; }

        public Formula Formula = new();
        public int AvalableValency;

        public int x, y;

        public Dictionary<Element, int> Connections { get; set; }

        public Element(int x, int y)
        {
            AvalableValency = Valency;
            Connections = new Dictionary<Element, int>();
            this.x = x;
            this.y = y;
        }

        public virtual string GetRawName()
        {
            return Symbol;
        }

        public void GenerateFormula(Visual visual)
        {
            if (Formula.rawName == string.Empty)
                Formula = new Formula(GetRawName(), visual);
        }

        public bool ConnectTo(Element? element, int strength)
        {
            if (element == null) return false;

            if (AvalableValency - strength >= 0 && element.AvalableValency - strength >= 0 && !Connections.ContainsKey(element))
            {
                AvalableValency -= strength;
                Connections.Add(element, strength);

                element.AvalableValency -= strength;
                element.Connections.Add(this, strength);
                return true;
            }
            return false;
        }
    }
}
