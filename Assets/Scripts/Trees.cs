using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    public List<Material> stages = new List<Material>();

    public bool FullyGrown { get; set; }
    int xCor;
    int yCor;

    void Start()
    {
        xCor = gameObject.GetComponent<Cube>().GetX();
        yCor = gameObject.GetComponent<Cube>().GetY();

        gameObject.GetComponent<Renderer>().material = stages[0];
       
        StartCoroutine(Growth());
    }


    IEnumerator Growth()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material = stages[1];

        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Renderer>().material = stages[2];

        FullyGrown = true;
    }
}
