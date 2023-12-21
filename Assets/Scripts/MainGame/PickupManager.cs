using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;
    public delegate void Pickup(PickupManager pickupManager, float duration);
    public static event Pickup pointPickup;
    public enum TypePickup { POINT, BOMB, PROTECT }
    private TypePickup type;
    public GameObject[] pickupObjects;

    private float speed, rotateSpeed;

    [Header("Pickup Settings")]
    public int maxPickups;

    public List<GameObject> pickups = new List<GameObject>();
    public List<GameObject> bombList = new List<GameObject>();
    public TypePickup Type { get => type; set => type = value; }
    
    [SerializeField]
    [Range(0,100)]
    private int bombSpawn;
    [SerializeField]
    [Range(0,100)]
    private int shieldSpawn;
    [SerializeField]
    [Range(0,100)]
    private int pointSpawn;
    [SerializeField]
    [Range(0f,10f)]
    private float pointDuration;

    public int BombSpawn { get { return bombSpawn; } set { bombSpawn = value; } }
    public int ShieldSpawn { get { return shieldSpawn; } set => shieldSpawn = value; }
    public int PointSpawn { get { return pointSpawn; } set => pointSpawn = value; }

    public float PointDuration { get { return pointDuration; } set => pointDuration = value; }


    private void Start()
    {
        speed = GameManager.manager.speed;
        rotateSpeed = speed * 4;
        if (instance != null && instance != this) //usual check
        {
            //Destroy(this);
        }
        else
        {
            instance = this;
        }
        //pointSpawn = 20;
        //bombSpawn = 5;
        //shieldSpawn = 10;
        maxPickups = 3;
        Debug.LogWarning("The spawn chances for pickups are: Bomb|" + bombSpawn + " |Shield|" + ShieldSpawn + " |Point|" + PointSpawn);
    }


    private void FixedUpdate()
    {
        CheckMaxPickups(maxPickups);
        //SpawnInCircle(Random.Range(0, pickupObjects.Length));
        if (Input.GetKey(KeyCode.A))
        {
           transform.Rotate(new Vector3(0, 0, 1) * Time.fixedDeltaTime * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
           transform.Rotate(new Vector3(0, 0, -1) * Time.fixedDeltaTime * rotateSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has picked me up!");
            CheckPickupType();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Pickup"))
        {
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
    private void CheckMaxPickups(int max)
    {
        pickups = GameObject.FindGameObjectsWithTag("Pickup").ToList();
        if (pickups.Count > max)
        {
            CancelInvoke("SpawnInCircle");
            return;
        }
        else
        {
            InvokeRepeating("SpawnInCircle", 0.1f, 5f);
        }
    }

    private void CheckPickupType()
    {
        switch (type)
        {
            case TypePickup.BOMB:
                {
                    Debug.Log("Pickup bomb type was used!");
                    GetChildObject(PrefabManager.prefabManager.transform, "OCGroup");
                    if (bombList.Count == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < bombList.Count; i++)
                    {
                        Destroy(bombList[i].gameObject); //Removes all the obstacles on screen
                    }
                    GameObject.FindGameObjectsWithTag("Pickup").ToList().ForEach(s => Destroy(s.gameObject)); //Removes all the pickups on screen as well
                    bombList.Clear();
                    break;
                }

            case TypePickup.PROTECT:
                PlayerScript.player.isProtected = true;
                break;
            default:
                pointPickup(this, PointDuration);
                break;
        }
    }

    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                bombList.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
    }

    private void SpawnInCircle()
    {
        int pickupIndex = Random.Range(0, pickupObjects.Length);
        GameObject pickup;
        pickup = Instantiate(pickupObjects[pickupIndex]);
        float r = 8.5f;    // distance from center
        float angle = Random.Range(0, Mathf.PI * 2);    // Random angle in radians
                                                        // sin and cos need value in radians
                                                        // full turn aroud circle in radians equal 2*PI ~6.283185 rad
        Vector2 pos2d = new Vector2(Mathf.Sin(angle) * r, Mathf.Cos(angle) * r);
        pickup.transform.position = new Vector3(pos2d.x, 8 + (pos2d.y), Random.Range(220f,360f));
        pickup.transform.SetParent(transform);
        pickup.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        if (SpawnCheck(pickup.transform.position, 5f))
        {
            pickups.Add(pickup);
            return;
        }
        else
        {
            Destroy(pickup.gameObject);
        }


    }

    private bool SpawnCheck(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        bool isClear = true;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Obstacle") || hitCollider.CompareTag("Pickup"))
            {
                isClear = false;
            }
            else
            {
                isClear = true;
            }
        }
        return isClear;
    }

}
