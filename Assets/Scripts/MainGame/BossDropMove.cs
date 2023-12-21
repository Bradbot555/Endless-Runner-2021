using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropMove : MonoBehaviour
{
    // Start is called before the first frame update
    private float mySpeed;
    private void FixedUpdate()
    {
        mySpeed = GameManager.manager.speed;
        transform.Translate(Vector3.back * Time.fixedDeltaTime * mySpeed);

    }
}
