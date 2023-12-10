using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using WpfApp1.Utility;

namespace WpfApp1.Chemistry.Element
{
    public abstract class Element
    {
        public abstract string Symbol { get; }
        public abstract int Valency { get; }

        public int AvalableValency;
        public List<Text> Formula = new();
        public SizeF FormulaSize = new();

        private bool _finalized = false;

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

        public void FinalizeFormula(Visual visual)
        {
            if (_finalized) return;
            string name = GetName();

            var upperLatter = TextFormater.FormatText("E", TextStyle.Element, visual);

            double height = upperLatter.formatted.Height;
            double width = 0.0;

            string str = "";
            bool IsIndex = false;
            for (int i = 0; i < name.Length; i++)
            {
                str += name[i];

                if ((i == name.Length - 1 || char.IsDigit(name[i + 1])) && !IsIndex)
                {
                    var text = TextFormater.FormatText(str, TextStyle.Element, visual);
                    width += text.formatted.Width;
                    Formula.Add(text);
                    IsIndex = true;
                    str = "";
                }
                else if ((i == name.Length - 1 || !char.IsDigit(name[i + 1])) && IsIndex)
                {
                    var text = TextFormater.FormatText(str, TextStyle.Index, visual);
                    width += text.formatted.Width;
                    Formula.Add(text);
                    IsIndex = false;
                    str = "";
                }
            }

            FormulaSize = new((float)width, (float)height);
            _finalized = true;
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
