using OrganicChemistry.Chemistry.Elements;
using System;

namespace OrganicChemistry.Chemistry
{
    public class ElementConnection : IComparable<ElementConnection>
    {
        public Element element1;
        public Element element2;

        public ElementConnection(Element element1, Element element2)
        {
            this.element1 = element1;
            this.element2 = element2;
        }

        public int CompareTo(ElementConnection? other)
        {
            if (other == null) return 0;

            if ((other.element1 == element1 && other.element2 == element2) ||
                (other.element1 == element2 && other.element2 == element1))
                return 0;

            return 1;
        }
    }
}
