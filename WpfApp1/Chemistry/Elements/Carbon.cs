using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Chemistry.Elements
{
    public class Carbon : Element
    {
        public Carbon(int x, int y) : base(x, y)
        {
         
        }

        public override string Symbol => "C";
        public override int Valency => 4;

        public override string GetName()
        {
            if (AvalableValency > 1)
                return Symbol + "H" + AvalableValency;
            else if (AvalableValency == 1)
                return Symbol + "H";
            return Symbol;
        }
    }
}
