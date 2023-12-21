using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAdd : MonoBehaviour
{

    //Honestly feels like I could do this somewhere else and not use a script, but I dunno where.
    public delegate void AddPoint();
    public static event AddPoint Instance;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has passed!");
            Instance();
        }
        else
        {
            return;
        }
    }
}
