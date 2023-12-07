using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Chemistry.Element;

namespace WpfApp1.Utility
{
    public static class MatrixUtil
    {
        public static int[,] Corners = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };

        public static Element? TryGet(ref Element[,] matrix, int x, int y)
        {
            if ((x > -1 && x < matrix.GetLength(0)) && (y > -1 && y < matrix.GetLength(1)))
            {
                return matrix[x, y];
            }
            return null;
        }

        public static Tuple<int, int>? TryGetElementPos(ref Element[,] matrix, int x, int y, Element element)
        {
            for (int i = 0; i < Corners.GetLength(0); i++)
            {
                Element? found = TryGet(ref matrix, x + Corners[i, 0], y + Corners[i, 1]);
                if (element == found)
                    return new Tuple<int, int>(x + Corners[i, 0], y + Corners[i, 1]);
            }

            return null;
        }
    }
}
