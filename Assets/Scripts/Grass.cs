using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public Color stage1;
    public Color stage2;
    public Color stage3;

    public bool FullyGrown { get; set; }
    int xCor;
    int yCor;

    void Start()
    {
        xCor = gameObject.GetComponent<Cube>().GetX();
        yCor = gameObject.GetComponent<Cube>().GetY();

        gameObject.GetComponent<Renderer>().material.color = stage1;

        StartCoroutine(Growth());
    }

    IEnumerator Growth()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material.color = stage2;

        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material.color = stage3;

        FullyGrown = true;
    }
}
