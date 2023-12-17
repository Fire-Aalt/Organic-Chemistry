using OrganicChemistry.Chemistry.Elements;

namespace OrganicChemistry.Utility
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
    }
}
