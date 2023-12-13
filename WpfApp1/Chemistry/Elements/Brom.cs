using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1.Chemistry.Elements
{
    public class Brom : Element
    {
        public Brom(int x, int y) : base(x, y)
        {
         
        }

        public override string Symbol => "Br";
        public override int Valency => 1;
    }
}
