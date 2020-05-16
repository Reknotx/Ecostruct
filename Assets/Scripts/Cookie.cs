using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    Vector3 direction;

    public void SetDirection(int xDir, int yDir)
    {
        direction = new Vector3(xDir, 0f, yDir);
    }
}
