using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField]
    private bool Boss1;

    public static BossScript instance;
    public delegate void Boss(BossScript bossScript, bool dead);
    public static event Boss BossDeath;

    private float speed;
    public bool rotateThis = false;
    public float rotateSpeed;

    public GameObject[] BossDrops;
    public GameObject[] BossAttacks;
    public List<GameObject> Drops = new List<GameObject>();
    public List<GameObject> Attacks = new List<GameObject>();

    private Animator ani;
    private PrefabManager prefabManager;

    public int boss1Health;
    public int boss2Health;
    public int stacks;
    private float timer;
    void Start()
    {
        boss1Health = 5;
        boss2Health = 5;
        instance = this;
        if (Boss1)
        {
            stacks = 0;
            ani = gameObject.GetComponent<Animator>();
            ani.Play("Boss1Beginning");
            StartCoroutine(DisableAnimations(2f));
        }
        else
        {
            timer = Time.time + 60f;
            StartCoroutine(Attack());
        }
        speed = 15f;
        rotateSpeed = speed * 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckMaxDrops(5);
        if (rotateThis)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(-1, 0, 0) * Time.fixedDeltaTime * rotateSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(1, 0, 0) * Time.fixedDeltaTime * rotateSpeed);
            }
        }

        if (Time.time == timer)
        {
            boss2Health -= 5;
        }

        if (boss1Health <= 0 || boss2Health <= 0)
        {
            Death();
        }
        if (stacks >= 3)
        {
            stacks = 3;
        }
    }
    private void SpawnInCircle()
    {
        if (Boss1)
        {
            int pickupIndex = UnityEngine.Random.Range(0, BossDrops.Length);
            GameObject drop;
            drop = Instantiate(BossDrops[pickupIndex]);
            float r = 8.5f;    // distance from center
            float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);    // Random angle in radians
                                                                        // sin and cos need value in radians
                                                                        // full turn aroud circle in radians equal 2*PI ~6.283185 rad
            Vector2 pos2d = new Vector2(Mathf.Sin(angle) * r, Mathf.Cos(angle) * r);
            drop.transform.position = new Vector3(pos2d.x, 8 + (pos2d.y), UnityEngine.Random.Range(150f, 220f));
            drop.transform.SetParent(transform);
            drop.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            if (SpawnCheck(drop.transform.position, 5f))
            {
                Drops.Add(drop);
                return;
            }
            else
            {
                Destroy(drop.gameObject);
            }
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
    private void CheckMaxDrops(int max)
    {
        Drops = GameObject.FindGameObjectsWithTag("BossDrop").ToList();
        if (Drops.Count > max)
        {
            CancelInvoke("SpawnInCircle");
            return;
        }
        else
        {
            InvokeRepeating("SpawnInCircle", 0.1f, 5f);
        }
    }

    private IEnumerator Attack()
    {
        if (Attacks.Count >= stacks)
        {
            yield return null;
        }
        else
        {
            int pickupIndex = UnityEngine.Random.Range(0, BossAttacks.Length);
            GameObject attack;
            attack = Instantiate(BossAttacks[pickupIndex]);
            float r = 8.5f;    // distance from center
            float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);    // Random angle in radians
                                                                        // sin and cos need value in radians
                                                                        // full turn aroud circle in radians equal 2*PI ~6.283185 rad
            Vector2 pos2d = new Vector2(Mathf.Sin(angle) * r, Mathf.Cos(angle) * r);
            attack.transform.position = new Vector3(pos2d.x, 8 + (pos2d.y), UnityEngine.Random.Range(150f, 220f));
            attack.transform.SetParent(transform);
            attack.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (Boss1)
        {
            StartCoroutine(DisableAnimations(15f));
        }
        yield return new WaitForSecondsRealtime(5f);
    }

    private IEnumerator DisableAnimations(float num)
    {
        yield return new WaitForSecondsRealtime(num);
        StartCoroutine(Attack());
        ani.enabled = false;
    }

    private void Death()
    {
        BossDeath(this, true);
        boss1Health = 5;
        boss2Health = 5;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.LogWarning("Boss2 Stole pickup!");
            Destroy(other.gameObject);
        }
    }
}
