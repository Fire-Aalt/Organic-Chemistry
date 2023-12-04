using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp1.Chemistry.Element;

namespace WpfApp1.Utility
{
    public class VisualFormater
    {
        public int[,] Corners = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        public void DrawElement(ref Element[,] matrix, int i, int j)
        {
            if (matrix[i, j] == null) return;
            List<KeyValuePair<Element, int>> connections = new();

            for (int c = 0; c < Corners.GetLength(0); c++)
            {
                Element? element = MatrixUtil.TryGet(ref matrix, i + Corners[i, 0], j + Corners[i, 1]);
                if (element != null)
                    connections.AddRange(element.Connections);
            }
            
            
        }
    }
}
