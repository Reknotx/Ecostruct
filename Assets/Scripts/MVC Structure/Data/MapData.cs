using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : Element
{
    [SerializeField] private GameObject _junkCube;
    [SerializeField] private GameObject _pollutedCube;
    [SerializeField] private GameObject _treeCube;
    [SerializeField] private GameObject _grassCube;
    [SerializeField] private GameObject _dirtCube;
    [SerializeField] private GameObject _mineCube;
    [SerializeField] private GameObject _cookie;


    public GameObject[,] Level;

    public GameObject JunkCube { get { return _junkCube; } }
    public GameObject PollutedCube { get { return _pollutedCube; } }
    public GameObject TreeCube { get { return _treeCube; } }
    public GameObject GrassCube { get { return _grassCube; } }
    public GameObject DirtCube { get { return _dirtCube; } }
    public GameObject MineCube { get { return _mineCube; } }
    public GameObject Cookie { get { return _cookie; } }

    public List<GameObject> JunkList { get; } = new List<GameObject>();
    public List<GameObject> PollutedDirtList { get; } = new List<GameObject>();
    public List<GameObject> PollutedWaterList { get; } = new List<GameObject>();
    public List<GameObject> EnemiesList { get; } = new List<GameObject>();
    public List<GameObject> CookiesList { get; } = new List<GameObject>();

    public int InitialPollution { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public float Scaling { get; set; }
    public float RemainingPollution { get; set; }

    private void Start()
    {
        Scaling = JunkCube.transform.localScale.x;
    }
}
