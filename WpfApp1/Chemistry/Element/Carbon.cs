using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Chemistry.Element
{
    public class Carbon : Element
    {
        public override string Symbol => "C";
        public override int Valency => 4;

        public override string GetName()
        {
            if (AvalableValency > 1)
                return Symbol + "H" + AvalableValency;
            return Symbol;
        }
    }
}
