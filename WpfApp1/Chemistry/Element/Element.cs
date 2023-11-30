using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Chemistry.Element
{
    public class Element
    {
        public string Symbol { get; set; }
        public uint Valency { get; set; }

        //public Point[] Connections { get; set; }

        public Element(string symbol, uint valency)
        {
            Symbol = symbol;
            Valency = valency;
        }
    }
}
