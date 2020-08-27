using System;
using System.Security.Cryptography;
using UnityEngine;

public class Utility : Element
{
    public bool CanSpread(int X, int Y, SourceBlock source)
    {
        if (X < 0 || X > app.data.map.Width) return false;
        if (Y < 0 || Y > app.data.map.Height) return false;
        if ((app.data.map.Level[X, Y].tag == "Dirt"
            || (app.data.map.Level[X, Y].tag == "Grass" && source == SourceBlock.Junk))
            && CheckIfNearSource(X, Y, source))
        {
            return true;
        }
        return false;
    }

    public bool CalculateGrassSpread(int X, int Y, float percentChance)
    {
        float baseChance = percentChance;
        float numCleanWaterTiles = CheckIfNearWater(X, Y, WaterType.clean);
        float numDirtyWaterTiles = CheckIfNearWater(X, Y, WaterType.polluted);

        float chance = baseChance + (Mathf.Pow(baseChance * (numCleanWaterTiles / 5), 2) / (numDirtyWaterTiles + 1));

        int RNG = NumberBetween(0, 99);

        if ((int)Mathf.Floor(chance) >= RNG)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CalculatePollutionSpread(float percentChance)
    {
        int RNG = NumberBetween(0, 100);

        if ((int)Mathf.Floor(percentChance) >= RNG)
        {
            return true;
        }

        return false;
    }

    /**
     * <summary>Pass in the values of the tile we want to spread to.</summary>
     * 
     * <returns>True if pollution can spread.</returns>
     * 
     * To North: x, y + 1
     * To East: x + 1, y
     * To South: x, y - 1
     * To West: x - 1, y
     * 
     * So if the source is at (1, 1)
     * and we want to see if we can spread East, pass in
     * X = 2, Y = 1
     */
    //public bool CheckIfPollutionSpread(int X, int Y)
    //{
    //    if (X < 0 || X > app.data.map.Width) return false;
    //    if (Y < 0 || Y > app.data.map.Height) return false;

    //    if ((app.data.map.Level[X, Y].tag == "Dirt" || app.data.map.Level[X, Y].tag == "Grass")
    //        && CheckIfNearSource(X, Y, SourceBlock.Junk))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private bool CheckIfNearSource(int X, int Y, SourceBlock source)
    {
        int mapWidth = app.data.map.Width;
        int mapHeight = app.data.map.Height;

        GameObject[,] map = app.data.map.Level;

        if (source == SourceBlock.Tree)
        {
            //Source block is tree

            for (int i = -3; i <= 3; i++)
            {
                if (X - Math.Abs(i) < 0 || X + i > mapWidth) continue;

                for (int j = -3; j <= 3; j++)
                {
                    if (Y - Math.Abs(j) < 0 || Y + j > mapHeight) continue;

                    //Because the for loops start off as negative, adding
                    //the numbers when they are negs is the same as subtracting
                    GameObject temp = map[X + i, Y + j];
                    if (temp.tag == "Tree")
                    {
                        if (temp.GetComponent<Trees>().FullyGrown && CheckIfInRange(X, Y, X + i, Y + j, 3))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        else
        {
            //Source block is Junk
            for (int i = -2; i <= 2; i++)
            {
                if (X - Math.Abs(i) < 0 || X + i > mapWidth) continue;

                for (int j = -2; j <= 2; j++)
                {
                    if (Y - Math.Abs(j) < 0 || Y + j > mapHeight) continue;

                    //Because the for loops start off as negative, adding
                    //the numbers when they are negs is the same as subtracting
                    GameObject temp = map[X + i, Y + j];
                    Debug.Log("Temp = " + temp.ToString());
                    if (temp.tag == "Junk" && CheckIfInRange(X, Y, X + i, Y + j, 1))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public int CheckIfNearWater(int X, int Y, WaterType water)
    {
        string waterType = "";

        int mapWidth = app.data.map.Width;
        int mapHeight = app.data.map.Height;
        int waterCount = 0;

        GameObject[,] map = app.data.map.Level;

        if (water == WaterType.clean) { waterType = "Water"; }
        else { waterType = "Dirty Water"; }

        for (int i = -3; i <= 3; i++)
        {
            if (X - Math.Abs(i) < 0 || X + i > mapWidth) continue;

            for (int j = -3; j < 3; j++)
            {
                if (Y - Math.Abs(j) < 0 || Y + j > mapHeight) continue;

                GameObject temp = map[X + i, Y + j];
                if (temp.tag == waterType && CheckIfInRange(X, Y, X + i, Y + j, 3))
                {
                    waterCount++;
                }
            }
        }
        return waterCount;
    }

    public bool CheckIfInRange(int X, int Y, int xDelta, int yDelta, int range)
    {
        float xDistance = Mathf.Pow(X - xDelta, 2f);
        float yDistance = Mathf.Pow(Y - yDelta, 2f);

        float distance = Mathf.Floor(Mathf.Sqrt(xDistance + yDistance));

        if (distance <= range) { return true; }
        else { return false; }
    }


    public int NumberBetween(int min, int max)
    {
        RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        byte[] randomNumber = new byte[1];

        _generator.GetBytes(randomNumber);

        double asciiValueOfRandChar = Convert.ToDouble(randomNumber[0]);

        double multiplier = Math.Max(0, (asciiValueOfRandChar / 255d) - 0.00000000001d);

        int range = max - min + 1;

        double randomValueInRange = Math.Floor(multiplier * range);

        return (int)(min + randomValueInRange);
    }
}
