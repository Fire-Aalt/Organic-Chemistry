using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Chemistry.Elements
{
    public class Chlor : Element
    {
        public Chlor(int x, int y) : base(x, y)
        {
         
        }

        public override string Symbol => "Cl";
        public override int Valency => 1;
    }
}
