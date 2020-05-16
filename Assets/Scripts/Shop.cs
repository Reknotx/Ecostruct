using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button openShop, closeShop, buySapling, buyCleaner, 
        buyPurifier, upgradeDrill, buyAmmo;

    public Text saplingCostText, cleanerCostText, purifierCostText,
                ammoCostText, drillUpgradeCostText;

    public int saplingCost, cleanerCost, purifierCost,
               ammoCost, drillUpgradeCost;

    public GameObject shopPanel;

    public Player player;

    private void Start()
    {
        shopPanel.SetActive(false);

        openShop.onClick.AddListener(OpenShop);

        closeShop.onClick.AddListener(CloseShop);

        buySapling.onClick.AddListener(() => BuyUpgrade(1));

        buyCleaner.onClick.AddListener(() => BuyUpgrade(2));

        buyPurifier.onClick.AddListener(() => BuyUpgrade(3));

        buyAmmo.onClick.AddListener(() => BuyUpgrade(4));

        upgradeDrill.onClick.AddListener(() => BuyUpgrade(5));

        saplingCostText.text = saplingCost.ToString();
        cleanerCostText.text = cleanerCost.ToString();
        purifierCostText.text = purifierCost.ToString();
        ammoCostText.text = ammoCost.ToString();
        drillUpgradeCostText.text = drillUpgradeCost.ToString();
    }

    public void OpenShop()
    {
        Debug.Log("Open Shop");
        Time.timeScale = 0f;
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        Debug.Log("Close Shop");
        Time.timeScale = 1f;
        shopPanel.SetActive(false);
    }

    public void BuyUpgrade(int index)
    {
        switch(index)
        {
            case 1:
                if (player.GetOre() >= saplingCost)
                {
                    player.AddSapling();
                    player.BuyItemWithOre(saplingCost);
                }
                break;

            case 2:
                if (player.GetOre() >= cleanerCost)
                {
                    player.AddCleaner();
                    player.AddCleaner();
                    player.AddCleaner();
                    player.BuyItemWithOre(cleanerCost);
                }
                break;

            case 3:
                if (player.GetOre() >= purifierCost)
                {
                    player.AddWaterPurifier();
                    player.BuyItemWithOre(purifierCost);
                }
                break;

            case 4:
                if (player.GetJunk() >= ammoCost)
                {
                    player.AddAmmo();
                    player.buyItemWithJunk(ammoCost);
                }
                break;

            case 5:
                if (player.GetJunk() >= drillUpgradeCost)
                {
                    player.UpgradeDrill();
                    player.buyItemWithJunk(drillUpgradeCost);
                }
                break;
        }
    }
}
