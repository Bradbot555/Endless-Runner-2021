using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static PickupManager PickupManager;
    public static PrefabManager PrefabManager;
    public static GameManager manager; //Singleton so we can set the speed and score for other scripts
    private bool doublePoints = false;
    private float pointDuration;

    public bool bossDead = false;
    public int bossesKilled = 0;

    [Header("Game Settings")]
    public float score;
    public int scoreAmount = 10;
    public float time;
    [Header("Map Settings")]
    public float speed = 15f;
    public float maxSpeed = 30f;
    // Start is called before the first frame update
    void Start()
    {
        if (manager != null && manager != this) //usual check
        {
            Destroy(this);
        }
        manager = this;
        time = 0;
        score = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ReloadScene();
        PointPickupActivate();
        if (Input.GetKey(KeyCode.Escape)) //Check to see if user presses Esc key, if they do go back to main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnEnable() //On enable for event calls
    {
        PlayerFail.Instance += PlayerFail_instance;
        PointAdd.Instance += PointAdd_Instance;
        PickupManager.pointPickup += PickupManager_pointPickup;
        BossScript.BossDeath += BossScript_BossDeath1;
    }

    private void OnDisable() // On disable for event calls
    {
        PlayerFail.Instance -= PlayerFail_instance;
        PointAdd.Instance -= PointAdd_Instance;
        PickupManager.pointPickup -= PickupManager_pointPickup;
        BossScript.BossDeath -= BossScript_BossDeath1;
    }

    private void AddScore(int amount) //Calls the score to be updated on the GUI
    {
        score += amount; //Adds to total score
        Debug.Log("This is the current score: | "+score); //Logs total score
        UIController.uI.AddScore(amount.ToString("F0")); //Updates the addscore text
        StartCoroutine(Wait(2));
        UIController.uI.SetScore(score.ToString("F0")); //Updates the total score text
    }

    private void TimeScore() //Calculates the score based on how long you lived for
    {
        time = Time.realtimeSinceStartup; //How long you lived for
        score += time; //Adds the time to the total score
        Debug.Log(score);
    }

    void GameOverLoad() //Loads the game over scene
    {
        StartCoroutine(LoadingScene(3)); //Fancy loading method to load the Gameover scene
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("GameOver")); //Puts the GameManager into the GameOver scene, so the score can be carried across.
    }
    private void PlayerFail_instance() //Event if the player fails
    {
        AddScore(scoreAmount); //Adds one more score to the player, to make them feel better
        TimeScore();// Calls to see how long they lived for
        GameOverLoad();//Game Over.
    }
    private void PointAdd_Instance() // When the player gets points call
    {
        StartCoroutine(SpeedUp()); //Speeds up the whole game
        if (doublePoints == true)
        {
            AddScore(scoreAmount * 2);
            Debug.Log("The Score added was: " + scoreAmount * 2 + " |Base score add: " + scoreAmount);
        }
        else
        {
            AddScore(scoreAmount); //Adds a certain amount of score
            Debug.Log("The Score added was: " + scoreAmount);
        }
        
    }
    private void PickupManager_pointPickup(PickupManager pickup, float duration)
    {
        Debug.LogWarning("Point pickup activated!");
        duration = Time.time + pickup.PointDuration;
        pointDuration = duration;
        Debug.LogWarning("Time for duration is currently: " + duration + "| Current time is: "+ Time.time);
        doublePoints = true;
        PointPickupActivate();
    }

    public IEnumerator Wait(int secs) //Simple wait method
    {
        yield return new WaitForSecondsRealtime(secs);
    }

    IEnumerator LoadingScene(int index) //One part for loading new scene
    {
        Scene GameOver = SceneManager.GetSceneByBuildIndex(index); //Gets the Gameover scene from it's index in Build Settings

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive); // Loads Gameover while MainGame is running

        while (!asyncLoad.isDone) //Checks to see if GameOver scene is loaded
        {
            yield return null;
        }

    }

    IEnumerator SpeedUp() //Does on the tin, speeds up the game.
    {
        yield return new WaitForSecondsRealtime(1); //Waits a second before speeding up
        speed += 0.5f; //Only adds half a speed for each call
        scoreAmount += 1; //Adds a full point for each call
        if (speed > maxSpeed) //checks to see speed cap
        {
            speed = maxSpeed;
            GetComponent<PlayerScript>().animator.SetBool("isMaxSpeed",true);
        }
    }

    void PointPickupActivate() 
    {
        if (Time.time >= pointDuration)
        {
            doublePoints = false;
            UIController.uI.doubleText.enabled = false;
        }
        else
        {
            UIController.uI.doubleText.enabled = true;
        }
    }

    void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.R)) //If the player wants to restart at any point, they can by pressing R
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    private void BossScript_BossDeath1(BossScript bossScript, bool dead)
    {
        bossesKilled++;
        AddScore(500 * bossesKilled);
        bossDead = dead;
    }

    
}
