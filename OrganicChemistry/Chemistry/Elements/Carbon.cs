namespace OrganicChemistry.Chemistry.Elements
{
    public class Carbon(int x, int y) : Element(x, y)
    {
        public override string Symbol => "C";
        public override int Valency => 4;

        public override string GetRawName()
        {
            if (AvalableValency > 1)
                return Symbol + "H" + AvalableValency;
            else if (AvalableValency == 1)
                return Symbol + "H";
            return Symbol;
        }
    }
}
