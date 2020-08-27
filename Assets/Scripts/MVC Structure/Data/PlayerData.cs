using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Element
{
    //Remove these two after done
    public int STARTING_CLEANERS_WHEN_TESTING;
    public int STARTING_SAPLINGS_WHEN_TESTING;

    public Coord coord;

    public int DrillLevel { get; set; } = 0;
    public int DirtCleaners { get; set; } = 0;
    public int WaterPurifiers { get; set; } = 0;
    public int CookieAmmo { get; set; } = 0;
    public int Saplings { get; set; } = 0;
    public int Junk { get; set; } = 0;
    public int Ore { get; set; } = 0;
    public int SelectedItem { get; set; } = 1;

    public void Awake()
    {
        coord = new Coord();
    }
}
