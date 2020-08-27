using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : Element
{
    [SerializeField] private Utility LogicUtility;

    public void InputFilter(KeyCode key)
    {
        if (key == KeyCode.A || key == KeyCode.S
         || key == KeyCode.D || key == KeyCode.W)
        {
            MovePlayer(key);
        }

        if (key == KeyCode.Space)
        {
            CubeInteractionHandler();
        }

        if (key == KeyCode.Alpha1 || key == KeyCode.Alpha2
        || key == KeyCode.Alpha3)
        {
            UpdateHotBar(key);
        }
    }

    private void MovePlayer(KeyCode key)
    {
        int xDelta = 0;
        int yDelta = 0;

        int mapWidth = app.data.map.Width;
        int mapHeight = app.data.map.Height;

        float scaling = app.data.map.Scaling;

        Vector3 originalPos = app.input.player.transform.position;
        Vector3 posDelta;
        Vector3 newPos;

        switch (key)
        {
            case KeyCode.A:
                if (app.data.player.coord.X >   0) xDelta = -1;
                break;

            case KeyCode.D:
                if (app.data.player.coord.X < mapWidth - 1) xDelta = 1;
                break;

            case KeyCode.S:
                if (app.data.player.coord.Y > 0) yDelta = -1;
                break;

            case KeyCode.W:
                if (app.data.player.coord.Y < mapHeight - 1) yDelta = 1;
                break;

            default:
                Debug.Log("Somehow made it to default.");
                break;
        }

        app.data.player.coord.X += xDelta;
        app.data.player.coord.Y += yDelta;

        posDelta = new Vector3(xDelta * scaling, 0f, yDelta * scaling);

        newPos = originalPos + posDelta;

        app.input.player.transform.position = newPos;

    }

    private void UpdateHotBar(KeyCode key)
    {
        int selectedItem = app.data.player.SelectedItem;

        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedItem != 1)
        {
            //Dirt cleaner

            switch (selectedItem)
            {
                case 2:
                    app.data.ui.hotKey2.color = new Color(0, 0, 0, 255);
                    break;

                case 3:
                    app.data.ui.hotKey3.color = new Color(0, 0, 0, 255);
                    break;
            }

            app.data.ui.hotKey1.color = new Color(255, 255, 0, 255);

            app.data.player.SelectedItem = 1;


        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && selectedItem != 2)
        {
            //Water purifier

            switch (selectedItem)
            {
                case 1:
                    app.data.ui.hotKey1.color = new Color(0, 0, 0, 255);
                    break;

                case 3:
                    app.data.ui.hotKey3.color = new Color(0, 0, 0, 255);
                    break;
            }
            app.data.ui.hotKey2.color = new Color(255, 255, 0, 255);

            app.data.player.SelectedItem = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && selectedItem != 3)
        {
            switch (selectedItem)
            {
                case 1:
                    app.data.ui.hotKey1.color = new Color(0, 0, 0, 255);
                    break;

                case 2:
                    app.data.ui.hotKey2.color = new Color(0, 0, 0, 255);
                    break;
            }
            app.data.ui.hotKey3.color = new Color(255, 255, 0, 255);

            app.data.player.SelectedItem = 3;
        }

    }

    private void UpdateMap()
    {
        int mapWidth = app.data.map.Width;
        int mapHeight = app.data.map.Height;

        GameObject[,] map = app.data.map.Level;

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
                        bool northSpreadable = LogicUtility.CanSpread(x, y + 1, SourceBlock.Tree);
                        bool eastSpreadable = LogicUtility.CanSpread(x + 1, y, SourceBlock.Tree);
                        bool southSpreadble = LogicUtility.CanSpread(x, y - 1, SourceBlock.Tree);
                        bool westSpreadable = LogicUtility.CanSpread(x - 1, y, SourceBlock.Tree);

                        PlantSpread(x, y, northSpreadable, eastSpreadable, southSpreadble, westSpreadable);
                    }
                    else if (map[x, y].tag == "Polluted Dirt")
                    {
                        bool northSpreadable = LogicUtility.CanSpread(x, y + 1, SourceBlock.Junk);
                        bool eastSpreadable = LogicUtility.CanSpread(x + 1, y, SourceBlock.Junk);
                        bool southSpreadable = LogicUtility.CanSpread(x, y - 1, SourceBlock.Junk);
                        bool westSpreadable = LogicUtility.CanSpread(x - 1, y, SourceBlock.Junk);

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
                        float scaling = app.data.map.Scaling;

                        GameObject newCube = Instantiate(app.data.map.PollutedCube, 
                                                        new Vector3(x * scaling, -1f, y * scaling), 
                                                        Quaternion.identity);

                        newCube.GetComponent<Cube>().SetCoords((int)(x * scaling), (int)(y * scaling));

                        app.data.map.PollutedDirtList.Add(newCube);

                        Destroy(map[x, y]);

                        map[x, y] = newCube;
                    }
                }
            }
        }
    }

    private void CubeInteractionHandler()
    {
        int playerX = app.data.player.coord.X;
        int playerY = app.data.player.coord.Y;
        int selectedItem = app.data.player.SelectedItem;
        float scaling = app.data.map.Scaling;


        GameObject temp = app.data.map.Level[playerX, playerY];

        if (temp.tag == "Junk")
        {
            GameObject pollutedCube = app.data.map.PollutedCube;

            GameObject newCube = Instantiate(pollutedCube, 
                                            new Vector3(playerX * scaling, -1f, playerY * scaling), 
                                            Quaternion.identity);

            newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

            app.data.map.PollutedDirtList.Add(newCube);
            app.data.map.JunkList.Remove(app.data.map.Level[playerX, playerY]);

            Destroy(app.data.map.Level[playerX, playerY]);

            app.data.map.Level[playerX, playerY] = newCube;
            
            app.data.player.Junk++;
        }
        else if (temp.tag == "Mine" && temp.GetComponent<Mine>().IsMineable())
        {
            app.data.map.Level[playerX, playerY].GetComponent<Mine>().MineResource();

            app.data.player.Ore++;
        }
        else if (temp.tag == "Polluted Dirt")
        {
            if (selectedItem == 1)
            {
                if (app.data.player.DirtCleaners > 0)
                {
                    GameObject dirtCube = app.data.map.DirtCube;

                    GameObject newCube = Instantiate(dirtCube, 
                                                    new Vector3(playerX * scaling, -1f, playerY * scaling), 
                                                    Quaternion.identity);

                    newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

                    //UpdatePollutedDirtList(map[playerX, playerY], true);
                    app.data.map.PollutedDirtList.Remove(app.data.map.Level[playerX, playerY]);

                    Destroy(app.data.map.Level[playerX, playerY]);

                    app.data.map.Level[playerX, playerY] = newCube;

                    app.data.player.DirtCleaners--;
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
                if (app.data.player.Saplings > 0)
                {
                    GameObject treeCube = app.data.map.TreeCube;

                    GameObject newCube = Instantiate(treeCube, 
                                                    new Vector3(playerX * scaling, -1f, playerY * scaling), 
                                                    Quaternion.identity);

                    newCube.GetComponent<Cube>().SetCoords((int)(playerX * scaling), (int)(playerY * scaling));

                    Destroy(app.data.map.Level[playerX, playerY]);

                    app.data.map.Level[playerX, playerY] = newCube;

                    app.data.player.Saplings--;
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
                if (app.data.player.WaterPurifiers > 0)
                {
                    //Clean the water
                }
            }
        }

        UpdatePollutionPercent();
    }

    private void PlantSpread(int X, int Y, bool N, bool E, bool S, bool W)
    {
        GameObject grassCube = app.data.map.GrassCube;

        Coord northCoord = new Coord(X, Y + 1);
        Coord eastCoord = new Coord(Y + 1, Y);
        Coord southCoord = new Coord(X, Y - 1);
        Coord westCoord = new Coord(X - 1, Y);

        if (N && LogicUtility.CalculateGrassSpread(northCoord.X, northCoord.Y, 25f))
        {
            GameObject temp = Instantiate(grassCube, 
                                        app.data.map.Level[northCoord.X, northCoord.Y].transform.position, 
                                        Quaternion.identity);

            Destroy(app.data.map.Level[northCoord.X, northCoord.Y].gameObject);
            app.data.map.Level[northCoord.X, northCoord.Y] = temp;
            temp.GetComponent<Cube>().SetCoords(northCoord.X, northCoord.Y);
        }

        if (E && LogicUtility.CalculateGrassSpread(eastCoord.X, eastCoord.Y, 25f))
        {
            GameObject temp = Instantiate(grassCube, 
                                        app.data.map.Level[eastCoord.X, eastCoord.Y].transform.position, 
                                        Quaternion.identity);

            Destroy(app.data.map.Level[eastCoord.X, eastCoord.Y].gameObject);
            app.data.map.Level[eastCoord.X, eastCoord.Y] = temp;
            temp.GetComponent<Cube>().SetCoords(eastCoord.X, eastCoord.Y);
        }

        if (S && LogicUtility.CalculateGrassSpread(southCoord.X, southCoord.Y, 25f))
        {
            GameObject temp = Instantiate(grassCube, 
                                        app.data.map.Level[southCoord.X, southCoord.Y].transform.position, 
                                        Quaternion.identity);

            Destroy(app.data.map.Level[southCoord.X, southCoord.Y].gameObject);
            app.data.map.Level[southCoord.X, southCoord.Y] = temp;
            temp.GetComponent<Cube>().SetCoords(southCoord.X, southCoord.Y);
        }

        if (W && LogicUtility.CalculateGrassSpread(westCoord.X, westCoord.Y, 25f))
        {
            GameObject temp = Instantiate(grassCube, 
                                        app.data.map.Level[westCoord.X, westCoord.Y].transform.position, 
                                        Quaternion.identity);

            Destroy(app.data.map.Level[westCoord.X, westCoord.Y].gameObject);
            app.data.map.Level[westCoord.X, westCoord.Y] = temp;
            temp.GetComponent<Cube>().SetCoords(westCoord.X, westCoord.Y);
        }
    }

    private void PollutionSpread(int X, int Y, bool N, bool E, bool S, bool W)
    {
        GameObject pollutedCube = app.data.map.PollutedCube;
        GameObject[,] map = app.data.map.Level;

        Coord northCoord = new Coord(X, Y + 1);
        Coord eastCoord = new Coord(Y + 1, Y);
        Coord southCoord = new Coord(X, Y - 1);
        Coord westCoord = new Coord(X - 1, Y);

        if (N && LogicUtility.CalculatePollutionSpread(5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[northCoord.X, northCoord.Y].transform.position, Quaternion.identity);
            Destroy(map[northCoord.X, northCoord.Y].gameObject);
            map[northCoord.X, northCoord.Y] = temp;
            app.data.map.PollutedDirtList.Add(temp);
            temp.GetComponent<Cube>().SetCoords(northCoord.X, northCoord.Y);
        }

        if (E && LogicUtility.CalculatePollutionSpread(5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[eastCoord.X, eastCoord.Y].transform.position, Quaternion.identity);
            Destroy(map[eastCoord.X, eastCoord.Y].gameObject);
            map[eastCoord.X, eastCoord.Y] = temp;
            app.data.map.PollutedDirtList.Add(temp);
            temp.GetComponent<Cube>().SetCoords(eastCoord.X, eastCoord.Y);
        }

        if (S && LogicUtility.CalculatePollutionSpread(5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[southCoord.X, southCoord.Y].transform.position, Quaternion.identity);
            Destroy(map[southCoord.X, southCoord.Y].gameObject);
            map[southCoord.X, southCoord.Y] = temp;
            app.data.map.PollutedDirtList.Add(temp);
            temp.GetComponent<Cube>().SetCoords(southCoord.X, southCoord.Y);
        }

        if (W && LogicUtility.CalculatePollutionSpread(5f))
        {
            GameObject temp = Instantiate(pollutedCube, map[eastCoord.X, eastCoord.Y].transform.position, Quaternion.identity);
            Destroy(map[eastCoord.X, eastCoord.Y].gameObject);
            map[eastCoord.X, eastCoord.Y] = temp;
            app.data.map.PollutedDirtList.Add(temp);
            temp.GetComponent<Cube>().SetCoords(eastCoord.X, eastCoord.Y);
        }
    }

    private void UpdatePollutionPercent()
    {
        int dirtCount = app.data.map.PollutedDirtList.Count;
        int enemyCount = app.data.map.EnemiesList.Count;
        int dirtyWaterCount = app.data.map.PollutedWaterList.Count;
        int junkCount = app.data.map.JunkList.Count;

        float pollutionItems = dirtCount + enemyCount + dirtyWaterCount + junkCount;

        float percent = pollutionItems / (float) app.data.map.InitialPollution;

        app.data.map.RemainingPollution = percent;
        app.data.ui.remainingPollution.value = percent;
    }

    public void SetUpMap()
    {

        app.input.player.transform.position = new Vector3(0f, 1f, 0f);


        JSONObject mapInfo = new JSONObject(CreateJSON(SceneManager.GetActiveScene().buildIndex.ToString()));

        int mapWidth = (int)mapInfo.GetField("MapWidth").n;
        int mapHeight = (int)mapInfo.GetField("MapHeight").n;

        app.data.map.Width = mapWidth;
        app.data.map.Height = mapHeight;

        float scaling = app.data.map.Scaling;

        app.data.map.Level = new GameObject[mapWidth, mapHeight];

        mapInfo = mapInfo.GetField("MapData");

        for (int i = 0; i < mapInfo.list.Count; i++)
        {
            int x = (int)mapInfo.list[i].GetField("x").n;
            int y = (int)mapInfo.list[i].GetField("y").n;
            string type = mapInfo.list[i].GetField("Type").str;

            GameObject temp = null;

            switch (type)
            {
                case "Junk":
                    temp = Instantiate(app.data.map.JunkCube, 
                                        new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), 
                                        Quaternion.identity);

                    if (y + 1 < mapHeight)
                    {
                        if (app.data.map.Level[x, y + 1] == null)
                        {
                            GameObject dirtN = Instantiate(app.data.map.PollutedCube, 
                                                            new Vector3(x * scaling, (scaling / 2) * -1, (y + 1) * scaling),
                                                            Quaternion.identity);

                            app.data.map.Level[x, y + 1] = dirtN;
                        }
                    }

                    if (x + 1 < mapWidth)
                    {
                        if (app.data.map.Level[x + 1, y] == null)
                        {
                            GameObject dirtE = Instantiate(app.data.map.PollutedCube, 
                                                            new Vector3((x + 1) * scaling, (scaling / 2) * -1, y * scaling), 
                                                            Quaternion.identity);

                            app.data.map.Level[x + 1, y] = dirtE;
                        }
                    }

                    if (y - 1 > 0)
                    {
                        if (app.data.map.Level[x, y - 1] == null)
                        {
                            GameObject dirtS = Instantiate(app.data.map.PollutedCube, 
                                                            new Vector3(x * scaling, (scaling / 2) * -1, (y - 1) * scaling), 
                                                            Quaternion.identity);

                            app.data.map.Level[x, y - 1] = dirtS;
                        }
                    }

                    if (x - 1 > 0)
                    {
                        if (app.data.map.Level[x - 1, y] == null)
                        {
                            GameObject dirtW = Instantiate(app.data.map.PollutedCube, 
                                                            new Vector3((x - 1) * scaling, (scaling / 2) * -1, y * scaling), 
                                                            Quaternion.identity);

                            app.data.map.Level[x - 1, y] = dirtW;
                        }
                    }

                    break;

                case "Mine":
                    temp = Instantiate(app.data.map.MineCube, 
                                        new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), 
                                        Quaternion.identity);
                    break;

                case "Tree":
                    temp = Instantiate(app.data.map.TreeCube, 
                                        new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), 
                                        Quaternion.identity);
                    break;

                case "Dirt":
                    temp = Instantiate(app.data.map.DirtCube, 
                                        new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), 
                                        Quaternion.identity);
                    break;

                case "Polluted Dirt":
                    temp = Instantiate(app.data.map.PollutedCube, 
                                        new Vector3(x * scaling, (scaling / 2) * -1, y * scaling), 
                                        Quaternion.identity);
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

            app.data.map.Level[x, y] = temp;
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (app.data.map.Level[i, j] == null)
                {
                    GameObject temp = Instantiate(app.data.map.DirtCube, 
                                                    new Vector3(i * scaling, (scaling / 2) * -1, j * scaling), 
                                                    Quaternion.identity);

                    temp.GetComponent<Cube>().SetCoords((int)(i * scaling), (int)(j * scaling));

                    app.data.map.Level[i, j] = temp;
                }
            }
        }

        //SetUpWinCondition();

        Debug.Log("Map Height = " + app.data.map.Height);
        Debug.Log("Map Width = " + app.data.map.Width);

        InvokeRepeating("UpdateMap", 1f, 1f);
    }

    private string CreateJSON(string Level)
    {
        string filePath = Application.dataPath;
        string intermediatePath = "/Resources/";
        string fileName = "Level" + Level;

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

    IEnumerator loadScene(int index)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        //sceneAsync = scene;

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
            //SceneManager.MoveGameObjectToScene(player.gameObject, sceneToLoad);
            //SceneManager.MoveGameObjectToScene(canvas, sceneToLoad);
            //SceneManager.MoveGameObjectToScene(mainCamera, sceneToLoad);
            //SceneManager.MoveGameObjectToScene(UIManager, sceneToLoad);
            SceneManager.SetActiveScene(sceneToLoad);
            SceneManager.UnloadSceneAsync(index - 1);
        }

        Debug.Log("Scene activated!");

        app.input.player.gameObject.SetActive(true);
    }
}
