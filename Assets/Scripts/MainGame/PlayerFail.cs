using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFail : MonoBehaviour
{
    public delegate void Collide();
    public static event Collide Instance;
    bool isOn = true;

    private void OnTriggerEnter(Collider other) //Incase the object is a trigger type.
    {
        Debug.Log("something hit me! |TriggerCheck|");
        if (!isOn)
        {
            return;
        }
        else if (other.CompareTag("Player")) //looks to see if the player did enter trigger
        {
            Debug.Log("Player has hit fail point! |" + gameObject.name + "|");
            if (PlayerScript.player.isProtected == true)
            {
                Debug.Log("Player has defied death!");
                PlayerScript.player.isProtected = false;
                isOn = false;
                StartCoroutine(wait(2f));
                return;
            }
            PlayerScript.player.Death(); //Calls player death
            Instance();

        }
        else
        {
            Debug.Log("Another object other than Player has hit me! |" + other.name + "|"); //Sees what other object interacted with it
        }
    }

    //private void OnCollisionEnter(Collision collision) //If the player NEEDS to collide and it can't be a trigger.
    //{
    //    Debug.Log("something hit me! |ColliderCheck|");
    //    if (collision.gameObject.CompareTag("Player")) //Looks to see if the player did collide
    //    {
    //        Debug.Log("Player has hit fail point! |" + gameObject.name + "|"); //Debug.log to see where the player died is correct
    //        if (PlayerScript.player.isProtected == true)
    //        {
    //            Debug.Log("Player has defied death!");
    //            PlayerScript.player.isProtected = false;
    //            return;
    //        }
    //        PlayerScript.player.Death(); //Call player death
    //        Instance();
    //    }
    //    else
    //    {
    //        Debug.Log("Another object other than Player has hit me! |" + collision.gameObject.name + "|");
    //    }
    //} //90% of this script is debug.log lol

    IEnumerator wait(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        isOn = true;
    }
}
