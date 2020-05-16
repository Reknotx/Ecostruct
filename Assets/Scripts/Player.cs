using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int junk;
    public int ore;

    //Remove these two after done
    public int STARTING_CLEANERS_WHEN_TESTING;
    public int STARTING_SAPLINGS_WHEN_TESTING;

    public int xCor;
    public int yCor;

    public Text junkText;
    public Text oreText;
    public Text remainingCleaners;
    public Text remainingPurifiers;
    public Text remainingSaplings;


    private int drillLevel = 0;
    private int dirtCleaners = 0;
    private int waterPurifiers = 0;
    private int cookieAmmo = 0;
    private int saplings = 0;

    public void Start()
    {
        xCor = 0;
        yCor = 0;

        cookieAmmo = 10;
        dirtCleaners = 0;
        saplings = 0;

        remainingCleaners.text = dirtCleaners.ToString();
        remainingPurifiers.text = waterPurifiers.ToString();
        remainingSaplings.text = saplings.ToString();
    }

    public int GetJunk()
    {
        return junk;
    }

    public void AddJunk()
    {
        junk++;

        junkText.text = junk.ToString();
    }

    public void RecycleJunk()
    {
        junk = 0;
        junkText.text = junk.ToString();
    }

    public void buyItemWithJunk(int amount)
    {
        junk -= amount;

        junkText.text = junk.ToString();
    }

    public int GetOre()
    {
        return ore;
    }

    public void AddOre()
    {
        if (drillLevel == 0)
        {
            ore++;
        }
        else
        {
            ore = ore + (1 * (drillLevel * 2));
        }
        oreText.text = ore.ToString();
    }

    public void BuyItemWithOre(int amount)
    {
        ore -= amount;

        oreText.text = ore.ToString();
    }

    public void UpgradeDrill()
    {
        drillLevel++;
    }

    public void AddCleaner()
    {
        dirtCleaners++;
        remainingCleaners.text = dirtCleaners.ToString();
    }

    public int GetCleaners()
    {
        return dirtCleaners;
    }

    public void UseCleaner()
    {
        dirtCleaners--;
        remainingCleaners.text = dirtCleaners.ToString();
    }

    public void AddWaterPurifier()
    {
        waterPurifiers++;
        remainingPurifiers.text = waterPurifiers.ToString();
    }

    public int GetPurifiers()
    {
        return waterPurifiers;
    }

    public void UsePurifier()
    {
        waterPurifiers--;
        remainingPurifiers.text = waterPurifiers.ToString();
    }

    public void AddAmmo()
    {
        cookieAmmo++;
    }

    public int GetAmmo()
    {
        return cookieAmmo;
    }

    public void UseAmmo()
    {
        cookieAmmo--;
    }

    public void AddSapling()
    {
        saplings++;
        remainingSaplings.text = saplings.ToString();

    }

    public int GetSaplings()
    {
        return saplings;
    }

    public void UseSapling()
    {
        saplings--;
        remainingSaplings.text = saplings.ToString();

    }

    public void SetXCor(int xCor)
    {
        this.xCor = xCor;
    }

    public int GetXCor()
    {
        return xCor;
    }

    public void SetYCor(int yCor)
    {
        this.yCor = yCor;
    }

    public int GetYCor()
    {
        return yCor;
    }

}
