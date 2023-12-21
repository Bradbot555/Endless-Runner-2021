using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropsScript : MonoBehaviour
{

    private float mySpeed;
    void Awake()
    {
        gameObject.tag = "BossDrop";
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void FixedUpdate()
    {
        mySpeed = GameManager.manager.speed;
        transform.Translate(Vector3.back * Time.fixedDeltaTime * mySpeed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has picked me up!");
            BossScript.instance.boss1Health--;
            BossScript.instance.stacks++;
            Destroy(gameObject);
        }
    }
}
