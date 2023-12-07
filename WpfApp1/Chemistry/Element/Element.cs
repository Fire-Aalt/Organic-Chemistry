using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp1.Utility;

namespace WpfApp1.Chemistry.Element
{
    public abstract class Element
    {
        public abstract string Symbol { get; }
        public abstract int Valency { get; }

        public int AvalableValency;

        public Dictionary<Element, int> Connections { get; set; }

        public Element()
        {
            AvalableValency = Valency;
            Connections = new Dictionary<Element, int>();
        }

        public virtual string GetName()
        {
            return Symbol;
        }

        public FormattedText GetFormattedName(Visual visual)
        {
            return TextFormater.FormatText(GetName(), TextStyle.Element, visual);
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
