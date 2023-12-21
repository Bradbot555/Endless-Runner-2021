using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProtect : PickupManager
{
    private int random, spawn;
    private int mySpawn;
    private float mySpeed;


    void Awake()
    {
        mySpawn = instance.ShieldSpawn;
        random = Random.Range(0, 100);
        spawn = mySpawn;
        PickupSpawnChance();

        

        gameObject.tag = "Pickup";
        Type = TypePickup.PROTECT;
    }
    void FixedUpdate()
    {
        mySpeed = GameManager.manager.speed;
        transform.Translate(Vector3.back * Time.fixedDeltaTime * mySpeed);
        
    }

    void PickupSpawnChance()
    {
        if (random >= spawn)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[PICKUP SHIELD]" + "The chance and random numbers are in order: | %" + spawn + " | " + random);
            return;
        }
    }
}
