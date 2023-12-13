using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Chemistry.Elements
{
    public class Iodine : Element
    {
        public Iodine(int x, int y) : base(x, y)
        {
         
        }

        public override string Symbol => "I";
        public override int Valency => 1;
    }
}
