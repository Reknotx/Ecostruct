using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private bool mineable = true;

    public Material active;
    public Material inactive;

    public float rechargeRate;

    public bool IsMineable()
    {
        return mineable;
    }

    public void MineResource()
    {
        StartCoroutine(ReplenishTimer());
    }

    IEnumerator ReplenishTimer()
    {
        mineable = false;
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material = inactive;
        yield return new WaitForSeconds(rechargeRate);

        rend.material = active;
        mineable = true;
    }
}
