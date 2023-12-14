using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using OrganicChemistry.Utility;

namespace OrganicChemistry.Chemistry
{
    public class Formula
    {
        public List<Text> Name = new();
        public SizeF Size = new();

        public string rawName;
        public Visual? visual;

        public Formula()
        {
            rawName = string.Empty;
        }

        public Formula(string rawName, Visual visual) 
        {
            this.rawName = rawName;
            this.visual = visual;

            CreateFormula();
        }

        public void CreateFormula()
        {
            if (visual == null) return;
            var upperLatter = TextFormater.FormatText("E", TextStyle.Element, visual);

            double height = upperLatter.formatted.Height;
            double width = 0.0;

            string str = "";
            bool IsIndex = false;
            for (int i = 0; i < rawName.Length; i++)
            {
                str += rawName[i];

                if ((i == rawName.Length - 1 || char.IsDigit(rawName[i + 1])) && !IsIndex)
                {
                    var text = TextFormater.FormatText(str, TextStyle.Element, visual);
                    width += text.formatted.Width;
                    Name.Add(text);
                    IsIndex = true;
                    str = "";
                }
                else if ((i == rawName.Length - 1 || !char.IsDigit(rawName[i + 1])) && IsIndex)
                {
                    var text = TextFormater.FormatText(str, TextStyle.Index, visual);
                    width += text.formatted.Width;
                    Name.Add(text);
                    IsIndex = false;
                    str = "";
                }
            }

            Size = new((float)width, (float)height);
        }
    }
}
