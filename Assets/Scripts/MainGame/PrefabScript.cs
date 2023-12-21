using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    const string OBS_TAG = "OCGroup"; //I forgot the tag like four times while working, so made a const

    [Header("Tube Settings")] //Nice and neat to work with!
    public float speed;
    public float rotateSpeed;

    [Header("Obstacle Spin Settings")] //Nice and neat, just how we like it!
    public bool isSpinning = false; //To see if the obstacles will be spinning or not
    public float spinSpeed = 0.2f; //How fast do they spin, if they are spinning
    public List<GameObject> OCGroups = new List<GameObject>(); //A list of the child groups that control the obstacles

    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = speed * 4; //Gotta rotate fast!
        if (OCGroups == null || OCGroups.Count == 0) //Check to see if the child array is empty
        {
            FindObjectwithTag(OBS_TAG); //Fancy stuff right here
            //return;
        }
        int check = Random.Range(0, 2);
        if (check == 1)
        {
            isSpinning = true;
        }
        else
        {
            isSpinning = false;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = GameManager.manager.speed;
        transform.Translate(Vector3.back * Time.fixedDeltaTime * speed); //The speed of the tunnel going towards the player
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, 0, 1) * Time.fixedDeltaTime * rotateSpeed); //Lazy way of rotating the tube, I know...
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 0, -1) * Time.fixedDeltaTime * rotateSpeed);
        }
       // speed = GameManager.gameManager.speed;
        for (int i = 0; i < OCGroups.Count; i++)
        {
            if (OCGroups[i] == null)
            {
                OCGroups.RemoveAt(i);
                break;
            }
            else if (isSpinning == true)
            {
                OCGroups[i].transform.Rotate(0, 0, spinSpeed, Space.Self);
            }
        }
    }
    public void FindObjectwithTag(string _tag)
    {
        OCGroups.Clear();
        Transform parent = transform;
        GetChildObject(parent, _tag);
    }
    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                OCGroups.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
    }

}
