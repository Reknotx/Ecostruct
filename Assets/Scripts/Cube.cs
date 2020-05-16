using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int xCor;
    public int yCor;

    public void SetCoords(int x, int y)
    {
        xCor = x;
        yCor = y;
    }

    public int GetX()
    {
        return xCor;
    }

    public int GetY()
    {
        return yCor;
    }
}
