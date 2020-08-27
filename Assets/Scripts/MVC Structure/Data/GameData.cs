using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction
{
    North,
    East,
    South,
    West
}

public enum SourceBlock
{
    Tree,
    Junk
}

public enum WaterType
{
    clean,
    polluted
}

public class GameData : Element
{
    public PlayerData player;
    public MapData map;
    public UIData ui;

    public GameObject canvas;
    public GameObject mainCamera;
    public GameObject UIManager;

    public GameObject shopPanel;

    //private bool shopWindowOpen = false;

    private bool allDirtIsGrass = false;
    private bool levelWon = false;

    //private int mapHeight;
    //private int mapWidth;
    //private int selectedItem;

    //private float scaling;
    //private float remainingPollutionPercent;

    //private List<GameObject> junkList = new List<GameObject>();
    //private List<GameObject> pollutedDirtList = new List<GameObject>();
    //private List<GameObject> pollutedWaterList = new List<GameObject>();
    //private List<GameObject> enemiesList = new List<GameObject>();
    //private List<GameObject> cookiesList = new List<GameObject>();

    private AsyncOperation sceneAsync;
}
