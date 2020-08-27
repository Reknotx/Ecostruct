using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Player player;
    public GameObject junkCube;
    public GameObject pollutedCube;
    public GameObject treeCube;
    public GameObject grassCube;
    public GameObject dirtCube;
    public GameObject mineCube;
    public GameObject cookie;

    public GameObject canvas;
    public GameObject mainCamera;
    public GameObject UIManager;

    public Text hotKey1;
    public Text hotKey2;
    public Text hotKey3;


    public Text winText;

    public GameObject shopPanel;

    public Slider remainingPollution;
    public GameObject[,] map;

    //private bool shopWindowOpen = false;

    private bool allDirtIsGrass = false;
    private bool levelWon = false;

    private int mapHeight;
    private int mapWidth;
    private int selectedItem;
    private int startingPollutionItems;

    private float scaling;      
    private float remainingPollutionPercent;

    private List<GameObject> junkList = new List<GameObject>();
    private List<GameObject> pollutedDirtList = new List<GameObject>();
    private List<GameObject> pollutedWaterList = new List<GameObject>();
    private List<GameObject> enemiesList = new List<GameObject>();
    private List<GameObject> cookiesList = new List<GameObject>();

    private AsyncOperation sceneAsync;

    private void Start()
    {
        scaling = junkCube.transform.localScale.x;
        SetUpMap();
        selectedItem = 1;
        hotKey1.color = new Color(255, 255, 0, 255);
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelWon)
        {
            if (!shopPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && selectedItem != 1)
                {
                    //Dirt cleaner

                    switch (selectedItem)
                    {
                        case 2:
                            hotKey2.color = new Color(0, 0, 0, 255);
                            break;

                        case 3:
                            hotKey3.color = new Color(0, 0, 0, 255);
                            break;
                    }
                    selectedItem = 1;

                    hotKey1.color = new Color(255, 255, 0, 255);


                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && selectedItem != 2)
                {
                    //Water purifier

                    switch (selectedItem)
                    {
                        case 1:
                            hotKey1.color = new Color(0, 0, 0, 255);
                            break;

                        case 3:
                            hotKey3.color = new Color(0, 0, 0, 255);
                            break;
                    }
                    selectedItem = 2;
                    hotKey2.color = new Color(255, 255, 0, 255);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && selectedItem != 3)
                {
                    switch (selectedItem)
                    {
                        case 1:
                            hotKey1.color = new Color(0, 0, 0, 255);
                            break;

                        case 2:
                            hotKey2.color = new Color(0, 0, 0, 255);
                            break;
                    }
                    selectedItem = 3;
                    hotKey3.color = new Color(255, 255, 0, 255);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CubeInteractionHandler();
                }
                else if (Input.GetKeyDown(KeyCode.W) && player.GetYCor() < (mapHeight - 1))
                {
                    Vector3 playerPos = new Vector3(player.transform.position.x, 1f, (player.GetYCor() * scaling) + (1f * scaling));
                    player.SetYCor(player.GetYCor() + 1);
                    player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
                }
                else if (Input.GetKeyDown(KeyCode.D) && player.GetXCor() < (mapWidth - 1))
                {
                    Vector3 playerPos = new Vector3((player.GetXCor() * scaling) + (1f * scaling), 1f, player.transform.position.z);
                    player.SetXCor(player.GetXCor() + 1);
                    player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
                }
                else if (Input.GetKeyDown(KeyCode.S) && player.GetYCor() > 0f)
                {
                    Vector3 playerPos = new Vector3(player.transform.position.x, 1f, (player.GetYCor() * scaling) - (1f * scaling));
                    player.SetYCor(player.GetYCor() - 1);
                    player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
                }
                else if (Input.GetKeyDown(KeyCode.A) && player.GetXCor() > 0f)
                {
                    Vector3 playerPos = new Vector3((player.GetXCor() * scaling) - (1f * scaling), 1f, player.transform.position.z);
                    player.SetXCor(player.GetXCor() - 1);
                    player.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z);
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && player.GetAmmo() > 0)
                {
                    GameObject newCookie = Instantiate(cookie, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + scaling), Quaternion.identity);
                    newCookie.GetComponent<Cookie>().SetDirection(0, 1);
                    player.UseAmmo();
                    AddCookiesToList(newCookie);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) && player.GetAmmo() > 0)
                {
                    GameObject newCookie = Instantiate(cookie, new Vector3(player.transform.position.x + scaling, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                    newCookie.GetComponent<Cookie>().SetDirection(1, 0);
                    player.UseAmmo();
                    AddCookiesToList(newCookie);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) && player.GetAmmo() > 0)
                {
                    GameObject newCookie = Instantiate(cookie, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - scaling), Quaternion.identity);
                    newCookie.GetComponent<Cookie>().SetDirection(0, -1);
                    player.UseAmmo();
                    AddCookiesToList(newCookie);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && player.GetAmmo() > 0)
                {
                    GameObject newCookie = Instantiate(cookie, new Vector3(player.transform.position.x - scaling, player.transform.position.y, player.transform.position.z), Quaternion.identity);
                    newCookie.GetComponent<Cookie>().SetDirection(-1, 0);
                    player.UseAmmo();
                    AddCookiesToList(newCookie);
                }

                if (remainingPollutionPercent == 0f && CheckIfAllDirtGrass())
                {
                    levelWon = true;
                }
            }
        }
        else
        {
            CancelInvoke();
            if (!winText.gameObject.activeSelf)
            {
                winText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                levelWon = false;
                Application.Quit();
                //StartCoroutine(loadScene(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
    }

    private void UpdateMap()
    {

        //Handle spreading
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                try
                {
                    if ((map[x, y].tag == "Tree" && map[x, y].GetComponent<Trees>().FullyGrown) ||
                        (map[x, y].tag == "Grass" && map[x, y].GetComponent<Grass>().FullyGrown))
                    {
                        bool northSpreadable = CheckIfNorthSpreadableTile(x, y);
                        bool eastSpreadable = CheckIfEastSpreadableTile(x, y);
                        bool southSpreadble = CheckIfSouthSpreadableTile(x, y);
                        bool westSpreadable = CheckIfWestSpreadableTile(x, y);

                        PlantSpread(x, y, northSpreadable, eastSpreadable, southSpreadble, westSpreadable);
                    }
                    else if (map[x, y].tag == "Polluted Dirt")
                    {
                        bool northSpreadable = CheckIfPollutionSpreadNorth(x, y);
                        bool eastSpreadable = CheckIfPollutionSpreadEast(x, y);
                        bool southSpreadable = CheckIfPollutionSpreadSouth(x, y);
                        bool westSpreadable = CheckIfPollutionSpreadWest(x, y);

                        PollutionSpread(x, y, northSpreadable, eastSpreadable, southSpreadable, westSpreadable);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString() + " at x = " + x + " and y = " + y);
                }
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (map[x, y].tag == "Tree")
                {
                    int surroundingPollution = 0;

                    if ((y + 1 < mapHeight) && map[x, y + 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x + 1 < mapWidth) && (y + 1 < mapHeight) && map[x + 1, y + 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x + 1 < mapWidth) && map[x + 1, y].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x + 1 < mapWidth) && (y - 1 > 0) && map[x + 1, y - 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((y - 1 > 0) && map[x, y - 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x - 1 > 0) && (y - 1 > 0) && map[x - 1, y - 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x - 1 > 0) && map[x - 1, y].tag == "Polluted Dirt") { surroundingPollution++; }

                    if ((x - 1 > 0) && (y + 1 < mapHeight) && map[x - 1, y + 1].tag == "Polluted Dirt") { surroundingPollution++; }

                    if (surroundingPollution >= 3)
                    {
                        GameObject newCube = Instantiate(pollutedCube, new Vector3(x * scaling, -1f, y * scaling), Quaternion.identity);

                        newCube.GetComponent<Cube>().SetCoords((int)(x * scaling), (int)(y * scaling));
                        UpdatePollutedDirtList(newCube, false);

                        Destroy(map[x, y]);

                        map[x, y] = newCube;
                    }
                }
            }
        }
    }

    private void CubeInteractionHandler()
    {
        int playerX = player.GetXCor();
        int playerY = player.GetYCor();

        GameObject temp = map[playerX, playerY];

        if (temp.tag == "Junk")
        {
            GameObject newCube = Instantiate(pollutedCube, new Vector3(playerX * scaling, -1f, playerY * scaling), Quaternion.identity);

            newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

            UpdatePollutedDirtList(newCube, false);
            UpdateJunkList(map[playerX, playerY], true);
            Destroy(map[playerX, playerY]);

            map[playerX, playerY] = newCube;

            player.AddJunk();
        }
        else if (temp.tag == "Mine" && temp.GetComponent<Mine>().IsMineable())
        {
            map[playerX, playerY].GetComponent<Mine>().MineResource();

            player.AddOre();
        }
        else if (temp.tag == "Polluted Dirt")
        {
            if (selectedItem == 1)
            {
                if (player.GetCleaners() > 0)
                {
                    GameObject newCube = Instantiate(dirtCube, new Vector3(playerX * scaling, -1f, playerY * scaling), Quaternion.identity);

                    newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

                    UpdatePollutedDirtList(map[playerX, playerY], true);
                    Destroy(map[playerX, playerY]);

                    map[playerX, playerY] = newCube;

                    player.UseCleaner();
                }
                else
                {
                    Debug.Log("You are out of cleaners");
                }
            }
            else
            {
                Debug.Log("You need to use a dirt cleaner here.");
            }
        }
        else if (temp.tag == "Dirt")
        {
            if (selectedItem == 3)
            {
                if (player.GetSaplings() > 0)
                {
                    GameObject newCube = Instantiate(treeCube, new Vector3(playerX * scaling, -1f, playerY * scaling), Quaternion.identity);

                    newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

                    Destroy(map[playerX, playerY]);

                    map[playerX, playerY] = newCube;

                    player.UseSapling();
                }
                else
                {
                    Debug.Log("You are out of saplings");
                }
            }
            else
            {
                Debug.Log("You can plant a sapling on dirt.");
            }
        }
        else if (temp.tag == "Polluted Water")
        {
            if (selectedItem == 2)
            {
                if (player.GetPurifiers() > 0)
                {
                    //Clean the water
                }
            }
        }
    }

    private void PlantSpread(int xCor, int yCor, bool North, bool East, bool South, bool West)
    {
        if (North && CalculateWillSpread(xCor, yCor + 1, 25f))
        {
            GameObject temp = Instantiate(grassCube, map[xCor, yCor + 1].transform.position, Quaternion.identity);
            Destroy(map[xCor, yCor + 1].gameObject);
            map[xCor, yCor + 1] = temp;
            temp.GetComponent<Cube>().SetCoords(xCor, yCor + 1);
        }

        if (East && CalculateWillSpread(xCor + 1, yCor, 25f))
        {
            GameObject temp = Instantiate(grassCube, map[xCor + 1, yCor].transform.position, Quaternion.identity);
            Destroy(map[xCor + 1, yCor].gameObject);
            map[xCor + 1, yCor] = temp;
            temp.GetComponent<Cube>().SetCoords(xCor + 1, yCor);
        }

        if (South && CalculateWillSpread(xCor, yCor - 1, 25f))
        {
            GameObject temp = Instantiate(grassCube, map[xCor, yCor - 1].transform.position, Quaternion.identity);
            Destroy(map[xCor, yCor - 1].gameObject);
            map[xCor, yCor - 1] = temp;
            temp.GetComponent<Cube>().SetCoords(xCor, yCor - 1);
        }

        if (West && CalculateWillSpread(xCor - 1, yCor, 25f))
        {
            GameObject temp = Instantiate(grassCube, map[xCor - 1, yCor].transform.position, Quaternion.identity);
            Destroy(map[xCor - 1, yCor].gameObject);
            map[xCor - 1, yCor] = temp;
            temp.GetComponent<Cube>().SetCoords(xCor - 1, yCor);
        }
    }

    private void PollutionSpread(int xCor, int yCor, bool North, bool East, bool South, bool West)
    {
        if (North && CalculatePollutionSpread(xCor, yCor + 1, 5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[xCor, yCor + 1].transform.position, Quaternion.identity);
            Destroy(map[xCor, yCor + 1].gameObject);
            map[xCor, yCor + 1] = temp;
            UpdatePollutedDirtList(temp, true);
            temp.GetComponent<Cube>().SetCoords(xCor, yCor + 1);
        }

        if (East && CalculatePollutionSpread(xCor + 1, yCor, 5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[xCor + 1, yCor].transform.position, Quaternion.identity);
            Destroy(map[xCor + 1, yCor].gameObject);
            map[xCor + 1, yCor] = temp;
            UpdatePollutedDirtList(temp, true);
            temp.GetComponent<Cube>().SetCoords(xCor + 1, yCor);
        }

        if (South && CalculatePollutionSpread(xCor, yCor - 1, 5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[xCor, yCor - 1].transform.position, Quaternion.identity);
            Destroy(map[xCor, yCor - 1].gameObject);
            map[xCor, yCor - 1] = temp;
            UpdatePollutedDirtList(temp, true);
            temp.GetComponent<Cube>().SetCoords(xCor, yCor - 1);
        }

        if (West && CalculatePollutionSpread(xCor - 1, yCor, 5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[xCor - 1, yCor].transform.position, Quaternion.identity);
            Destroy(map[xCor - 1, yCor].gameObject);
            map[xCor - 1, yCor] = temp;
            UpdatePollutedDirtList(temp, true);
            temp.GetComponent<Cube>().SetCoords(xCor - 1, yCor);
        }
    }

    private bool CalculateWillSpread(int xCor, int yCor, float percentChance)
    {
        float baseChance = percentChance;
        float numCleanWaterTiles = CheckIfNearCleanWater(xCor, yCor);
        float numDirtyWaterTiles = CheckIfNearPollutedWater(xCor, yCor);

        float chance = baseChance + ( Mathf.Pow(baseChance * (numCleanWaterTiles / 5), 2) / (numDirtyWaterTiles + 1) );

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

    private bool CalculatePollutionSpread(int xCor, int yCor, float percentChance)
    {
        int RNG = NumberBetween(0, 100);

        if ((int)Mathf.Floor(percentChance) >= RNG)
        {
            return true;
        }

        return false;
    }

    private bool CheckIfNorthSpreadableTile(int xCor, int yCor)
    {
        if (yCor + 1 < mapHeight && map[xCor, yCor + 1].tag == "Dirt"
            && CheckifNearTree(xCor, yCor + 1))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfEastSpreadableTile(int xCor, int yCor)
    {
        if (xCor + 1 < mapWidth && map[xCor + 1, yCor].tag == "Dirt"
            && CheckifNearTree(xCor + 1, yCor))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfSouthSpreadableTile(int xCor, int yCor)
    {
        if (yCor > 0 && map[xCor, yCor - 1].tag == "Dirt"
            && CheckifNearTree(xCor, yCor - 1))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfWestSpreadableTile(int xCor, int yCor)
    {
        if (xCor > 0 && map[xCor - 1, yCor].tag == "Dirt"
            && CheckifNearTree(xCor - 1, yCor))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfPollutionSpreadNorth(int xCor, int yCor)
    {
        if (yCor + 1 < mapHeight && (map[xCor, yCor + 1].tag == "Dirt" || map[xCor, yCor + 1].tag == "Grass")
            && CheckIfNearJunk(xCor, yCor + 1))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfPollutionSpreadEast(int xCor, int yCor)
    {
        if (xCor + 1 < mapWidth && (map[xCor + 1, yCor].tag == "Dirt" || map[xCor + 1, yCor].tag == "Grass")
            && CheckIfNearJunk(xCor + 1, yCor))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfPollutionSpreadSouth(int xCor, int yCor)
    {
        if (yCor > 0 && (map[xCor, yCor - 1].tag == "Dirt" || map[xCor, yCor - 1].tag == "Grass")
            && CheckIfNearJunk(xCor, yCor - 1))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfPollutionSpreadWest(int xCor, int yCor)
    {
        if (xCor > 0 && (map[xCor - 1, yCor].tag == "Dirt" || map[xCor - 1, yCor].tag == "Grass")
            && CheckIfNearJunk(xCor - 1, yCor))
        {
            return true;
        }
        return false;
    }

    private int CheckIfNearCleanWater(int xCor, int yCor)
    {
        int cleanWaterCount = 0;

        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (
                   (xCor - i >= 0 && yCor - j >= 0 && map[xCor - i, yCor - j].tag == "Water" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor - j, 3)) ||
                   (xCor + i < mapWidth && yCor + j < mapHeight && map[xCor + i, yCor + j].tag == "Water" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor + j, 3)) ||
                   (xCor - i >= 0 && yCor + j < mapHeight && map[xCor - i, yCor + j].tag == "Water" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor + j, 3)) ||
                   (xCor + i < mapWidth && yCor - j >= 0 && map[xCor + i, yCor - j].tag == "Water" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor - j, 3))
                   )
                {
                    cleanWaterCount++;
                }
            }
        }
        return cleanWaterCount;
    }

    private int CheckIfNearPollutedWater(int xCor, int yCor)
    {
        int dirtyWaterCount = 0;
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (
                   (xCor - i >= 0 && yCor - j >= 0 && map[xCor - i, yCor - j].tag == "Dirty Water" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor - j, 3)) ||
                   (xCor + i < mapWidth && yCor + j < mapHeight && map[xCor + i, yCor + j].tag == "Dirty Water" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor + j, 3)) ||
                   (xCor - i >= 0 && yCor + j < mapHeight && map[xCor - i, yCor + j].tag == "Dirty Water" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor + j, 3)) ||
                   (xCor + i < mapWidth && yCor - j >= 0 && map[xCor + i, yCor - j].tag == "Dirty Water" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor - j, 3))
                   )
                {
                    dirtyWaterCount++;
                }
            }
        }
        return dirtyWaterCount;
    }

    /**
     * 
     * In order for a plant block to spread to a new tile, the new dirt block
     * would have to be three blocks or less from a tree
     */
    private bool CheckifNearTree(int xCor, int yCor)
    {
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                if (
                   (xCor - i >= 0 && yCor - j >= 0 && map[xCor - i, yCor - j].tag == "Tree" && map[xCor - i, yCor - j].GetComponent<Trees>().FullyGrown && CheckIfWithinRange(xCor, yCor, xCor - i, yCor - j, 3)) ||
                   (xCor + i < mapWidth && yCor + j < mapHeight && map[xCor + i, yCor + j].tag == "Tree" && map[xCor + i, yCor + j].GetComponent<Trees>().FullyGrown && CheckIfWithinRange(xCor, yCor, xCor + i, yCor + j, 3)) ||
                   (xCor - i >= 0 && yCor + j < mapHeight && map[xCor - i, yCor + j].tag == "Tree" && map[xCor - i, yCor + j].GetComponent<Trees>().FullyGrown && CheckIfWithinRange(xCor, yCor, xCor - i, yCor + j, 3)) ||
                   (xCor + i < mapWidth && yCor - j >= 0 && map[xCor + i    , yCor - j].tag == "Tree" && map[xCor + i, yCor - j].GetComponent<Trees>().FullyGrown && CheckIfWithinRange(xCor, yCor, xCor + i, yCor - j, 3))
                   )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckIfNearJunk(int xCor, int yCor)
    {
        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                if (
                   (xCor - i >= 0 && yCor - j >= 0 && map[xCor - i, yCor - j].tag == "Junk" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor - j, 1)) ||
                   (xCor + i < mapWidth && yCor + j < mapHeight && map[xCor + i, yCor + j].tag == "Junk" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor + j, 1)) ||
                   (xCor - i >= 0 && yCor + j < mapHeight && map[xCor - i, yCor + j].tag == "Junk" && CheckIfWithinRange(xCor, yCor, xCor - i, yCor + j, 1)) ||
                   (xCor + i < mapWidth && yCor - j >= 0 && map[xCor + i, yCor - j].tag == "Junk" && CheckIfWithinRange(xCor, yCor, xCor + i, yCor - j, 1))
                   )
                {
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * This function is called when looking within a range of a one block in a radius,
     * and needing to know if the range is acceptable for certain logic checks
     * 
     * This function should only be called within the searching methods so that bools are
     * only determined and passed once
     * 
     * Ex. plant block is the x and y, tree block is the xDelta and yDelta
     * 
     * int xCor - the x coordinate of the original block
     * int yCor - the y coordinate of the original block
     * int xDelta - the x coordinate of the block we are looking at
     * int yDelta - the y coordinate of the block we are looking at
     * int range - The range to which we need to be from the block at xDelta and yDelta
     */
    private bool CheckIfWithinRange(int xCor, int yCor, int xDelta, int yDelta, int range)
    {
        float xDistance = Mathf.Pow(xCor - xDelta, 2f);
        float yDistance = Mathf.Pow(yCor - yDelta, 2f);

        float distance = Mathf.Floor(Mathf.Sqrt(xDistance + yDistance));

        if (distance <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIfAllDirtGrass()
    {

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject temp = map[x, y];
                if (temp.tag == "Dirt" || temp.tag == "Polluted Dirt" || temp.tag == "Junk")
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void SetUpMap()
    {

        player.transform.position = new Vector3(0f, 1f, 0f);


        JSONObject mapInfo = new JSONObject(CreateJSON(SceneManager.GetActiveScene().buildIndex.ToString()));

        mapWidth = (int)mapInfo.GetField("MapWidth").n;
        mapHeight = (int)mapInfo.GetField("MapHeight").n;

        map = new GameObject[mapWidth, mapHeight];

        mapInfo = mapInfo.GetField("MapData");

        for (int i = 0; i < mapInfo.list.Count; i++)
        {
            int x = (int)mapInfo.list[i].GetField("x").n;
            int y = (int)mapInfo.list[i].GetField("y").n;
            string type = mapInfo.list[i].GetField("Type").str;

            GameObject temp = null;

            switch(type)
            {
                case "Junk":
                    temp = Instantiate(junkCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    if (y + 1 < mapHeight)
                    {
                        if (map[x, y + 1] == null)
                        {
                            GameObject dirtN = Instantiate(pollutedCube, new Vector3(x * scaling, (scaling / 2) * -1, (y + 1) * scaling), Quaternion.identity);
                            map[x, y + 1] = dirtN;
                        }
                    }

                    if (x + 1 < mapWidth)
                    {
                        if (map[x + 1, y] == null)
                        {
                            GameObject dirtE = Instantiate(pollutedCube, new Vector3((x + 1) * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                            map[x + 1, y] = dirtE;
                        }
                    }
                    
                    if (y - 1 > 0)
                    {
                        if (map[x, y - 1] == null)
                        {
                            GameObject dirtS = Instantiate(pollutedCube, new Vector3(x * scaling, (scaling / 2) * -1, (y - 1) * scaling), Quaternion.identity);
                            map[x, y - 1] = dirtS;
                        }
                    }

                    if (x - 1 > 0)
                    {
                        if (map[x - 1, y] == null)
                        {
                            GameObject dirtW = Instantiate(pollutedCube, new Vector3((x - 1) * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                            map[x - 1, y] = dirtW;
                        }
                    }

                    break;

                case "Mine":
                    temp = Instantiate(mineCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                case "Tree":
                    temp = Instantiate(treeCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                case "Dirt":
                    temp = Instantiate(dirtCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                case "Polluted Dirt":
                    temp = Instantiate(pollutedCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                case "Dirty Water":
                    //temp = Instantiate(pollutedWaterCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                case "Water":
                    //temp = Instantiate(waterCube, new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), Quaternion.identity);
                    break;

                default:
                    Debug.LogError("Incorrect name in file at item " + i + ". Name written is " + type);
                    break;
            }

            temp.GetComponent<Cube>().SetCoords((int)(x * scaling), (int)(y * scaling));

            map[x, y] = temp;
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (map[i, j] == null)
                {
                    GameObject temp = Instantiate(dirtCube, new Vector3(i * scaling, (scaling / 2) * -1, j * scaling), Quaternion.identity);

                    temp.GetComponent<Cube>().SetCoords((int)(i * scaling), (int)(j * scaling));

                    map[i, j] = temp;
                }
            }
        }

        SetUpWinCondition();

        InvokeRepeating("UpdateMap", 1f, 1f);
    }

    void SetUpWinCondition()
    {
        SetUpJunkList();
        SetUpPollutedDirtList();
        SetUpPollutedWaterList();
        SetUpEnemiesList();

        startingPollutionItems = junkList.Count + pollutedDirtList.Count + pollutedWaterList.Count;

        UpdateRemainingPollutionPercent();
    }

    void AddCookiesToList(GameObject cookie)
    {
        cookiesList.Add(cookie);
    }

    void SetUpJunkList()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Junk");

        foreach (GameObject junk in temp)
        {
            junkList.Add(junk);
        }
    }

    void SetUpPollutedDirtList()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Polluted Dirt");

        foreach (GameObject polluted in temp)
        {
            pollutedDirtList.Add(polluted);
        }
    }

    void SetUpPollutedWaterList()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Polluted Water");

        foreach (GameObject water in temp)
        {
            pollutedWaterList.Add(water);
        }
    }

    void SetUpEnemiesList()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in temp)
        {
            enemiesList.Add(enemy);
        }
    }


    /*
     * GameObject junk: item to add or remove from the list
     * bool remove: if true remove the item from the list and destroy it in
     * the world, if false add it to the list
     * 
     */
    void UpdateJunkList(GameObject junk, bool remove)
    {
        if (remove)
        {
            junkList.Remove(junk);
        }
        else
        {
            junkList.Add(junk);
        }
        UpdateRemainingPollutionPercent();
    }

    void UpdatePollutedDirtList(GameObject pollutedDirt, bool remove)
    {
        if (remove)
        {
            pollutedDirtList.Remove(pollutedDirt);
        }
        else
        {
            pollutedDirtList.Add(pollutedDirt);
        }
        UpdateRemainingPollutionPercent();
    }

    void UpdatePollutedWaterList(GameObject dirtyWater, bool remove)
    {
        if (remove)
        {
            pollutedWaterList.Remove(dirtyWater);
        }
        else
        {
            pollutedWaterList.Add(dirtyWater);
        }
        UpdateRemainingPollutionPercent();
    }

    void UpdateRemainingPollutionPercent()
    {
        float remainingPollutionItems = pollutedDirtList.Count + enemiesList.Count + pollutedWaterList.Count + junkList.Count;

        if (remainingPollutionItems >= startingPollutionItems)
        {
            remainingPollutionPercent = 1f;
        }
        else
        {
            remainingPollutionPercent = remainingPollutionItems / (float)startingPollutionItems;

        }
        remainingPollution.value = remainingPollutionPercent;
    }

    private string CreateJSON(string Level)
    {
        string filePath = Application.dataPath;
        string intermediatePath = "/Resources/";
        string fileName =  "Level" + Level;

        StringBuilder builder = new StringBuilder();
        
        try
        {
            StreamReader streamReader = new StreamReader(filePath + intermediatePath + fileName + ".JSON");

            while (!streamReader.EndOfStream)
            {
                builder.Append(streamReader.ReadLine());
            }
            //currentJSONFile = filePath + intermediatePath + fileName;
            streamReader.Close();
        }
        catch
        {
            Debug.LogError(filePath + intermediatePath + fileName + ".JSON could not be opened");
        }

        return builder.ToString();
    }

    private int NumberBetween(int minimumValue, int maximumValue)
    {
        RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        byte[] randomNumber = new byte[1];

        _generator.GetBytes(randomNumber);

        double asciiValueOfRandChar = Convert.ToDouble(randomNumber[0]);

        double multiplier = Math.Max(0, (asciiValueOfRandChar / 255d) - 0.00000000001d);

        int range = maximumValue - minimumValue + 1;

        double randomValueInRange = Math.Floor(multiplier * range);

        return (int)(minimumValue + randomValueInRange);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUpMap();
    }

    IEnumerator loadScene(int index)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        sceneAsync = scene;

        //Wait until we are done loading the scene
        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
            yield return null;
        }

        Debug.Log("Done Loading Scene");

        scene.allowSceneActivation = true;

        while (!scene.isDone)
        {
            // wait until it is really finished
            yield return null;
        }

        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(index);

        if (sceneToLoad.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);
            SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad);
            SceneManager.MoveGameObjectToScene(canvas, sceneToLoad);
            //SceneManager.MoveGameObjectToScene(mainCamera, sceneToLoad);
            SceneManager.MoveGameObjectToScene(UIManager, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
            SceneManager.UnloadSceneAsync(index - 1);
        }

        Debug.Log("Scene activated!");

        player.gameObject.SetActive(true);

    }
}