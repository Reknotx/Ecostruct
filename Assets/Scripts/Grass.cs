using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public Color stage1;
    public Color stage2;
    public Color stage3;

    bool fullyGrown = false;
    int xCor;
    int yCor;

    void Start()
    {
        xCor = gameObject.GetComponent<Cube>().GetX();
        yCor = gameObject.GetComponent<Cube>().GetY();

        gameObject.GetComponent<Renderer>().material.color = stage1;

        StartCoroutine(Growth());
    }

    public bool CheckIfFullGrown()
    {
        return fullyGrown;
    }

    IEnumerator Growth()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material.color = stage2;

        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material.color = stage3;

        fullyGrown = true;
    }
}
