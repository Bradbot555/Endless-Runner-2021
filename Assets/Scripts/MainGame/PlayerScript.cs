using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject deathParticle; //Gameobject != particle I know, but I've tied the particle to an empty game object.
    private GameObject @object;

    public Animator animator;

    public static PlayerScript player;

    public float jumpHieght = 1f;
    private float rotationLowerLimit = -15f;
    private float rotationUpperLimit = 15f;

    public bool isProtected = false;
    public bool godMode = false;

    public float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        deathParticle.SetActive(false); //With this his death particle doesn't turn on accidently, it's probably not needed tho.
        if (player != null && player != this)
        {
            Destroy(gameObject);
        }
        player = this;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (godMode)
        {
            isProtected = true;
        }

        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.rotation.z < rotationUpperLimit)
            {
                transform.Rotate(0, 0, -horizontal);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.rotation.z > rotationLowerLimit)
            {
                transform.Rotate(0, 0, -horizontal);
            }
        }

        Quaternion target = Quaternion.Euler(15, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * 5);

        if (isProtected == true)
        {
            GetChildObject(transform, "ProtectionVerify");
            @object.SetActive(true);
        }
        else
        {
            GetChildObject(transform, "ProtectionVerify");
            @object.SetActive(false);
        }
    }

    public void Death() //Players death is tragic yes.
    {
        if (godMode)
        {
            return;
        }
        else
        {
            this.gameObject.tag = "Untagged";
            this.gameObject.isStatic = true;
            deathParticle.SetActive(true);
            animator.SetBool("isDead", true);
            Instantiate(deathParticle, transform, false);
            Destroy(gameObject, 1);
        }

    }
    public void GetChildObject(Transform parent, string name)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.name == name)
            {
                @object = child.gameObject;
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, name);
            }
        }
    }
}
