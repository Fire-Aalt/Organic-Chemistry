using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApp1.Chemistry.Elements;
using WpfApp1.Utility;

namespace WpfApp1.Algorithm
{
    public class IsomerAlgorithm
    {
        public Random rng = new();
        public Element[,] matrix = new Element[0, 0];
        public int carbon;
        public int chlor;
        public int brom;
        public int iodine;
        public string isomerType;

        public int mainRowY, mainRowLength;
        public int x, y;

        public int orderedBranch;
        public Dictionary<int, Branch> availableBranches = new();
        public int numberOfSubElements, subElementBranches;

        public List<int> unsearchedConfigurations = new();
        public IsomerAlgorithm(int carbon, int chlor, int brom, int iodine, string isomerType)
        {
            this.carbon = carbon;
            this.chlor = chlor;
            this.brom = brom;
            this.iodine = iodine;
            this.isomerType = isomerType;
            numberOfSubElements = chlor + brom + iodine;
        }

        public void Start()
        {
            int branches = (int)Math.Ceiling(numberOfSubElements / 2.0);
            if (branches > carbon)
            {
                MessageBox.Show("Impossible configuration detected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int i = 1; i <= carbon; i++)
            {
                unsearchedConfigurations.Add(i);
            }

            while (unsearchedConfigurations.Count > 0)
            {
                int index = rng.Next(0, unsearchedConfigurations.Count);
                mainRowLength = unsearchedConfigurations[index];

                int width = mainRowLength + 2;
                int height = width;

                matrix = new Element[width, height];

                mainRowY = height / 2;
                if (height % 2 == 0)
                    mainRowY--;
                orderedBranch = mainRowLength / 2;
                if (mainRowLength % 2 == 0)
                    orderedBranch--;

                bool success = CreateConfiguration();
                if (success) return;
                     
                unsearchedConfigurations.Remove(index);
            }

            if (unsearchedConfigurations.Count == 0)
                MessageBox.Show("Impossible configuration detected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool CreateConfiguration()
        {
            y = mainRowY;
            availableBranches.Clear();

            int availableCarbon = carbon - mainRowLength;
            for (int x = 1; x <= mainRowLength; x++)
            {
                AddElement(new Carbon(x, y));
                availableBranches.Add(x, new Branch());
            }

            int iteration = 0;
            while (availableCarbon > 0 && iteration < 1000)
            {
                availableCarbon = GenerateBranches(availableCarbon);
                iteration++;
            }

            if (availableCarbon > 0)
                return false;

            List<KeyValuePair<int, Branch>> available = availableBranches.Where(b => b.Value.OneAvailable()).ToList();
            if (available.Count == 0)
                return false;

            int usedChlor = 0, usedBrom = 0, usedIodine = 0;
            for (int i = 0; i < numberOfSubElements; i++)
            {
                if (available.Count == 0)
                    return false;

                KeyValuePair<int, Branch> branch;
                while (true)
                {
                    int index = rng.Next(0, available.Count);
                    branch = available[index];

                    x = branch.Key;
                    if ((x - 1) <= orderedBranch)
                        break;
                }

                int newOrderedBranch = mainRowLength - x;
                if (newOrderedBranch > orderedBranch)
                    orderedBranch = newOrderedBranch;

                if (branch.Value.BothAvailable())
                {
                    int turn = rng.Next(-1, 1);
                    if (turn == -1)
                    {
                        branch.Value.lower = false;
                        y = mainRowY - 1;
                    }
                    else
                    {
                        branch.Value.upper = false;
                        y = mainRowY + 1;
                    }
                }
                else
                {
                    if (branch.Value.upper)
                        y = mainRowY + 1;
                    else
                        y = mainRowY - 1;

                    available.Remove(branch);
                }

                if (usedChlor != chlor)
                {
                    AddElement(new Chlor(x, y));
                    usedChlor++;
                }
                else if (usedBrom != brom)
                {
                    AddElement(new Brom(x, y));
                    usedBrom++;
                }
                else if (usedIodine != iodine)
                {
                    AddElement(new Iodine(x, y));
                    usedIodine++;
                }
            }

            if (usedChlor != chlor || usedBrom != brom || usedIodine != iodine)
                return false;
            return true;
        }

        public int GenerateBranches(int availableCarbon)
        {
            x = 1;
            int maxBranch = 0;
            int occupiedBranches = 0;
            while (availableCarbon > 0 && x != mainRowLength)
            {
                int turn = rng.Next(-1, 2);
                if (turn != 0 && maxBranch > 0 && (x - 1) <= orderedBranch)
                {
                    bool occupied;
                    if (turn == -1)
                        occupied = availableBranches[x].lower;
                    else
                        occupied = availableBranches[x].upper;

                    if (!occupied && carbon - occupiedBranches == subElementBranches) 
                    {

                    }
                    else
                    {
                        var branch = rng.Next(1, Math.Min(availableCarbon, maxBranch));
                        int usedCarbon = CreateBranch(maxBranch, branch, turn);
                        availableCarbon -= usedCarbon;

                        if (usedCarbon > 0)
                        {
                            if (turn == -1 && availableBranches[x].lower)
                            {
                                availableBranches[x].lower = false;
                                occupiedBranches++;
                            }
                            else if (availableBranches[x].upper)
                            {
                                availableBranches[x].upper = false;
                                occupiedBranches++;
                            }
                        }
                    }
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

            return availableCarbon;
        }

        private int CreateBranch(int maxBranch, int carbon, int yMultiplier)
        {
            int usedCarbon = 0;
            for (int i = 1; i <= maxBranch; i++)
            {
                int y = mainRowY + yMultiplier * i;
                if (matrix[x, y] == null)
                {
                    AddElement(new Carbon(x, y));
                    usedCarbon++;

                    int newOrderedBranch = mainRowLength - x;
                    if (newOrderedBranch > orderedBranch)
                        orderedBranch = newOrderedBranch;
                }

                if (usedCarbon == carbon) break;
            }

            return usedCarbon;
        }

        private void AddElement<T>(T element) where T : Element
        {
            int x = element.x; int y = element.y;
            matrix[x, y] = element;

            if (y == mainRowY)
            {
                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x + 1, y), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x - 1, y), 1);
            }

            element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y + 1), 1);
            element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y - 1), 1);
        }
    }

    public class Branch
    {
        public bool upper = true;
        public bool lower = true;

        public bool OneAvailable() => upper || lower;
        public bool BothAvailable() => upper && lower;
    }

    public enum IsomerType
    {
        Alkane,
        Alkene,
        Alkyne,
        Alkadiene
    }
}
