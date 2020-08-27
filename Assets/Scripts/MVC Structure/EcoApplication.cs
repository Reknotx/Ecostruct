using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public EcoApplication app { get { return EcoApplication.Instance; } }
}

public class EcoApplication : MonoBehaviour
{
    public static EcoApplication Instance;

    public GameData data;
    public GameLogic logic;
    public GameInput input;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        GameSetup();
    }

    private void GameSetup()
    {
        logic.SetUpMap();
    }
}
