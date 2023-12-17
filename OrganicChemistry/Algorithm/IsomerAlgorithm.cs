using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using OrganicChemistry.Chemistry.Elements;
using OrganicChemistry.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganicChemistry.Algorithm
{
    public class IsomerAlgorithm(int carbon, int chlor, int brom, int iodine, string isomerType)
    {
        public Random rng = new();
        public Element[,] matrix = new Element[0, 0];
        public int carbon = carbon;
        public int chlor = chlor;
        public int brom = brom;
        public int iodine = iodine;
        public string isomerType = isomerType;

        public int mainRowY, mainRowLength;
        public int x, y;

        public int orderedBranch;
        public Dictionary<int, Branch> availableBranches = [];
        public int numberOfSubElements = chlor + brom + iodine, subElementBranches;

        public List<int> unsearchedConfigurations = [];

        public async Task Start()
        {
            bool canBeGenerated = false;
            int minCarbon = 1;
            switch (isomerType)
            {
                case "Alkane":
                    canBeGenerated = (int)Math.Ceiling(numberOfSubElements / 2.0 - 1) <= carbon;
                    break;
                case "Alkene":
                    minCarbon = 2;
                    canBeGenerated = (int)Math.Ceiling(numberOfSubElements / 2.0) <= carbon;
                    break;
                case "Alkyne":
                    minCarbon = 2;
                    canBeGenerated = (int)Math.Ceiling(numberOfSubElements / 2.0 + 1) <= carbon;
                    break;
                case "Alkadiene":
                    canBeGenerated = (int)Math.Ceiling(numberOfSubElements / 2.0 + 1) <= carbon;
                    minCarbon = 3;
                    break;
            }

            if (!canBeGenerated)
            {
              //  var box = MessageBoxManager.GetMessageBoxStandard("Error", "Impossible configuration detected", ButtonEnum.Ok, Icon.Error);
//                var result = await box.ShowAsync();
                return;
            }

            for (int i = minCarbon; i <= carbon; i++)
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
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Error", "Impossible configuration detected", ButtonEnum.Ok, Icon.Error);
                var result = await box.ShowAsync();
                return;
            }
        }

        private bool CreateConfiguration()
        {
            y = mainRowY;
            availableBranches.Clear();

            int availableCarbon = carbon - mainRowLength;
            switch (isomerType)
            {
                case "Alkane":
                    for (int x = 1; x <= mainRowLength; x++)
                    {
                        AddElement(new Carbon(x, y));
                        availableBranches.Add(x, new Branch());
                    }
                    break;
                case "Alkene":
                    int con = rng.Next(2, 2 + mainRowLength / 2);

                    for (int x = 1; x <= mainRowLength; x++)
                    {
                        if (x == con)
                        {
                            AddElement(new Carbon(x, y), 2);
                            availableBranches.Add(x, new Branch());
                            continue;
                        }

                        AddElement(new Carbon(x, y));
                        availableBranches.Add(x, new Branch());
                    }
                    break;
                case "Alkyne":
                    for (int x = 1; x <= mainRowLength; x++)
                    {
                        if (x == 2)
                        {
                            AddElement(new Carbon(x, y), 3);
                            availableBranches.Add(x, new Branch());
                            continue;
                        }

                        AddElement(new Carbon(x, y));
                        availableBranches.Add(x, new Branch());
                    }
                    break;
                case "Alkadiene":
                    int con1 = rng.Next(2, 2 + mainRowLength / 2);
                    int con2 = con1;
                    while (con2 == con1)
                    {
                        con2 = rng.Next(2, 3 + mainRowLength - con1);
                    }

                    for (int x = 1; x <= mainRowLength; x++)
                    {
                        if (x == con1 || x == con2)
                        {
                            AddElement(new Carbon(x, y), 2);
                            availableBranches.Add(x, new Branch());
                            continue;
                        }

                        AddElement(new Carbon(x, y));
                        availableBranches.Add(x, new Branch());
                    }
                    break;
            }

            int iteration = 0;
            while (availableCarbon > 0 && iteration < 1000)
            {
                availableCarbon = GenerateBranches(availableCarbon);
                iteration++;
            }

            if (availableCarbon > 0)
                return false;

            if (GenerateHalogens())
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    for (int y = 0; y < matrix.GetLength(1); y++)
                    {
                        matrix[x, y]?.GenerateFormula(new Avalonia.Visual());
                    }
                }

                return true;
            }
            else
                return false;
        }

        private int GenerateBranches(int availableCarbon)
        {
            x = 1;
            int maxBranch = 0;
            int occupiedBranches = 0;
            while (availableCarbon > 0 && x != mainRowLength)
            {
                int turn = rng.Next(-1, 2);
                if (turn != 0 && maxBranch > 0 && (x - 1) <= orderedBranch)
                {
                    bool available;
                    if (turn == -1)
                        available = availableBranches[x].lower;
                    else
                        available = availableBranches[x].upper;

                    if (!(!available && carbon - occupiedBranches == subElementBranches)) 
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

        private bool GenerateHalogens()
        {
            List<KeyValuePair<int, Branch>> available = availableBranches.Where(b => b.Value.OneAvailable()).ToList();
            available.Add(new KeyValuePair<int, Branch>(1, new Branch(left: true)));
            available.Add(new KeyValuePair<int, Branch>(mainRowLength, new Branch(right: true)));

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

                if (!CanConnect(matrix[x, mainRowY]))
                {
                    available.Remove(branch);
                    continue;
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
                else if (branch.Value.OneAvailable())
                {
                    if (branch.Value.upper)
                        y = mainRowY + 1;
                    else
                        y = mainRowY - 1;

                    available.Remove(branch);
                }
                else if (branch.Value.left)
                {
                    x--;
                    y = mainRowY;
                    available.Remove(branch);
                }
                else
                {
                    x++;
                    y = mainRowY;
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

        private int CreateBranch(int maxBranch, int carbon, int yMultiplier)
        {
            int usedCarbon = 0;
            if (!CanConnect(matrix[x, mainRowY])) 
                return 0;

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

        private void AddElement<T>(T element, int strengthWithPrev) where T : Element
        {
            int x = element.x; int y = element.y;
            matrix[x, y] = element;

            if (y == mainRowY)
            {
                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x + 1, y), 1);
                element.ConnectTo(MatrixUtil.TryGet(ref matrix, x - 1, y), strengthWithPrev);
            }

            element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y + 1), 1);
            element.ConnectTo(MatrixUtil.TryGet(ref matrix, x, y - 1), 1);
        }

        private static bool CanConnect(Element element, int strength = 1)
        {
            return element.AvalableValency - strength >= 0;
        }
    }

    public class Branch
    {
        public bool upper = true;
        public bool lower = true;

        public bool left;
        public bool right;

        public Branch(bool left = false, bool right = false) 
        {
            this.left = left;
            this.right = right;

            if (left || right)
            {
                upper = false;
                lower = false;
            }
        }

        public bool OneAvailable() => upper || lower;
        public bool BothAvailable() => upper && lower;
    }
}
