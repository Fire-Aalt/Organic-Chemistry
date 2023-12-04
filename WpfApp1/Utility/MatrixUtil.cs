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
        public static Element? TryGet(ref Element[,] matrix, int i, int j)
        {
            if ((i > -1 && i < matrix.GetLength(0)) && (j > -1 && j < matrix.GetLength(1)))
            {
                return matrix[i, j];
            }
            return null;
        }
    }
}
