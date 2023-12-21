using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public enum LevelCurrent { LVL1, LVL2, LVL2BOSS, LVL3, LVL3BOSS }
    private LevelCurrent Lvl;

    public static PrefabManager prefabManager;
    public GameObject Boss1;
    public GameObject Boss2;
    [Header("Prefab spawn types")]
    public GameObject[] prefabSpawnsLVL1;
    public GameObject[] prefabSpawnsLVL2; //An array of prefabs to spawn from
    public GameObject[] prefabLVL2BossMap;
    public GameObject[] prefabSpawnsLVL3;
    public GameObject[] prefabLVL3BossMap;
    [Header("Current Prefabs")]
    public List<GameObject> currentPrefabs = new List<GameObject>(); //A list of active prefabs

    private float spawnZ = 0f; //The location where the new Prefabs will spawn
    private float prefabLength = 50f; //How long each Prefab is
    public int prefabAmount = 5; //How many should be on screen at any given point
    float timer;


    public LevelCurrent LVL { get => Lvl; set => Lvl = value; }

    void Start()
    {
        this.LVL = LevelCurrent.LVL1;
        timer = Time.time + 60f;
        if (prefabManager != null && prefabManager != this) //Singleton check
        {
            Destroy(gameObject);
        }
        prefabManager = this;
        //SpawnPrefab(prefabLVL2BossMap,0); //Creates an empty tunnel for player to start in
                                        //currentPrefabs.RemoveAt(0);

    }
    void FixedUpdate()
    {
        CheckSpawn(prefabAmount);
        if (Time.time == timer)
        {
            StartCoroutine(LevelProgression());
        }
    }

    private IEnumerator LevelProgression()
    {
        timer = Time.time + 60f;
        switch (Lvl)
        {
            case LevelCurrent.LVL1:
                Debug.LogWarning("Going to level 2!");
                Lvl = LevelCurrent.LVL2;
                yield return new WaitForSecondsRealtime(1f);
                break;
            case LevelCurrent.LVL2:
                Lvl = LevelCurrent.LVL2BOSS;
                Boss1.SetActive(true);
                yield return new WaitForSecondsRealtime(1f);
                break;
            case LevelCurrent.LVL2BOSS:
                if (GameManager.manager.bossDead)
                {
                    Lvl = LevelCurrent.LVL3;
                }
                yield return new WaitForSecondsRealtime(1f);
                break;
            case LevelCurrent.LVL3:
                Lvl = LevelCurrent.LVL3BOSS;
                Boss2.SetActive(true);
                yield return new WaitForSecondsRealtime(1f);
                break;
            case LevelCurrent.LVL3BOSS:
                randomLevel(Random.Range(1, 4));
                yield return new WaitForSecondsRealtime(1f);
                break;
            default:
                Debug.LogError("Level has overflowed! Please contact head developer or admin!");
                //Lvl = LevelCurrent.LVL2;
                break;
        }
    }

    private void SpawnPrefab(GameObject[] prefabSpawner, int prefabIndex = 0) //Spawning in the new Tiles/Prefabs
    {
        GameObject go; //Temp gameobject for Prefab
        go = Instantiate(prefabSpawner[prefabIndex]) as GameObject; //Setting Object as a random prefab from array
        go.transform.SetParent(transform); //Prefab is now a child of PrefabManager
        go.transform.position = new Vector3(0, 8, 0); //Need to set hight so player isn't falling into deathbox below
        go.transform.localPosition = Vector3.forward * spawnZ;
        currentPrefabs.Add(go); //Adds prefab to list to see how many are on screen right now
        spawnZ = (prefabLength * currentPrefabs.Count); //setting the new distance for the next prefab to spawn in
    }

    private void CheckSpawn(int MaxSpawn)
    {
        if (currentPrefabs.Count < MaxSpawn) //Checks to see if new Prefabs need to spawn in
        {
            switch (Lvl)
            {
                case LevelCurrent.LVL1:
                    SpawnPrefab(prefabSpawnsLVL1, Random.Range(0, prefabSpawnsLVL1.Length));
                    break;
                case LevelCurrent.LVL2:
                    SpawnPrefab(prefabSpawnsLVL2, Random.Range(0, prefabSpawnsLVL2.Length)); //Spawning a random Prefab
                    break;
                case LevelCurrent.LVL2BOSS:
                    SpawnPrefab(prefabLVL2BossMap, Random.Range(0, prefabLVL2BossMap.Length));
                    break;
                case LevelCurrent.LVL3:
                    SpawnPrefab(prefabSpawnsLVL3, Random.Range(0, prefabSpawnsLVL3.Length));
                    break;
                case LevelCurrent.LVL3BOSS:
                    SpawnPrefab(prefabSpawnsLVL3, Random.Range(0, prefabSpawnsLVL3.Length));
                    break;
                default:
                    SpawnPrefab(prefabLVL2BossMap, 0);
                    Debug.LogError("Map Spawner is not working correctly!");
                    break;
            }
        }
    }

    private void randomLevel(int rand)
    {
        switch (rand)
        {
            case 1:
                Lvl = LevelCurrent.LVL1;
                break;
            case 2:
                Lvl = LevelCurrent.LVL2;
                break;
            case 3:
                Lvl = LevelCurrent.LVL3;
                break;
        }

    }

}
