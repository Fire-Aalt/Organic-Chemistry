using System;
using WpfApp1.Chemistry.Elements;
using WpfApp1.Utility;

namespace WpfApp1.Algorithm
{
    public class IsomerAlgorithm
    {
        public Random rng = new();
        public Element[,] matrix;
        public int carbon;
        public int chlor;
        public int brom;
        public int iodine;

        public int mainRowY, mainRowLength;
        public bool freeLayout;

        public int width, height;
        public int x, y;

        public IsomerAlgorithm(int carbon, int chlor, int brom, int iodine, bool freeLayout)
        {
            width = carbon + 2;
            height = carbon;

            matrix = new Element[width, height];
            this.carbon = carbon;
            this.chlor = chlor;
            this.brom = brom;
            this.iodine = iodine;
            this.freeLayout = freeLayout;

            mainRowY = height / 2;
            if (height % 2 == 0)
                mainRowY--;
            mainRowLength = rng.Next(carbon / 2 + 1, carbon + 1);
        }

        public void Start()
        {
            x = 1;
            y = mainRowY;

            //C();
            //return;
            int maxBranch = 0;
            int availableCarbon = carbon - mainRowLength;

            for (int i = 1; i <= mainRowLength; i++)
            {
                matrix[i, y] = new Carbon(i, y);
            }

            while (availableCarbon > 0 && x != mainRowLength)
            {
                int turn = rng.Next(-1, 2);
                if (turn != 0 && maxBranch > 0)
                {
                    var branch = rng.Next(1, Math.Min(availableCarbon, maxBranch));
                    CreateBranch(branch, turn);
                    availableCarbon -= branch;
                }

                if (mainRowLength % 2 == 0)
                {
                    if (x - 1 < mainRowLength / 2 - 1)
                        maxBranch++;
                    else if (x - 1 > mainRowLength / 2 - 1)
                        maxBranch--;
                }
                else
                {
                    if (x - 1 < mainRowLength / 2)
                        maxBranch++;
                    else if (x - 1 >= mainRowLength / 2)
                        maxBranch--;
                }

                x++;
            }
        }

        public void C()
        {
            for (int x = 1; x < width - 1; x++)
            {
                matrix[x, y] = new Carbon(x, y);
            } 
        }

        private void CreateBranch(int carbon, int yMultiplier)
        {
            for (int i = 1; i <= carbon; i++)
            {
                y = mainRowY + yMultiplier * i;
                matrix[x, y] = new Carbon(x, y);
            }
            y = mainRowY;
        }

        private void OldAlgo()
        {
            int numberOfElements = carbon;


            var rng = new Random();
            for (int i = 0; i < numberOfElements; i++)
            {
                int x = rng.Next(0, matrix.GetLength(0));
                int y = rng.Next(0, matrix.GetLength(1));
                int strength = rng.Next(1, 4);

                if (matrix[x, y] != null) continue;
                Element element = new Carbon(x, y);

                if (y == mainRowY || freeLayout)
                {
                    element.ConnectTo(MatrixUtil.TryGet(ref matrix, x + 1, y), strength);
                    element.ConnectTo(MatrixUtil.TryGet(ref matrix, x - 1, y), strength);
                }

                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y + 1), strength);
                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y - 1), strength);

                matrix[x, y] = element;
            }
        }
    }
}
