using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Chemistry.Element
{
    public abstract class Element
    {
        public abstract string Symbol { get; }
        public abstract int Valency { get; }

        public int AvalableValency;

        public Element()
        {
            AvalableValency = Valency;
        }

        public virtual string GetName()
        {
            return Symbol;
        }

        public bool ConnectTo(Element element)
        {
            if (AvalableValency - element.Valency >= 0)
            {
                AvalableValency -= element.Valency;
                return true;
            }
            return false;
        }
    }
}
