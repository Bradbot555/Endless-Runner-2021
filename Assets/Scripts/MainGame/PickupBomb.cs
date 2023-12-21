using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBomb : PickupManager
{
    // Start is called before the first frame update
    private int random, spawn;
    private int mySpawn;
    private float mySpeed;

    void Awake()
    {
        mySpawn = instance.BombSpawn;
        spawn = mySpawn;
        random = Random.Range(0,100);
        PickupSpawnChance();
        gameObject.tag = "Pickup";
        Type = TypePickup.BOMB;
    }

    private void FixedUpdate()
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
            Debug.Log("[PICKUP BOMB]" + "The chance and random numbers are in order: | %" + spawn + " | " + random);
            return;
        }
    }
}
