using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    public TextMeshProUGUI proUGUI;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        StartCoroutine(Wait(2));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Controller();
        SetScore(GameManager.manager.score.ToString("F0")); //Set score for game over screen
    }

    void Controller()
    {
        if (Input.GetKey(KeyCode.Escape)) //Check to see if user presses Esc key, if they do go back to main menu
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.anyKeyDown) //If they press ANY key, game restarts
        {
            SceneManager.LoadScene("MainGame");
        }
    }

    IEnumerator Wait(int secs)
    {
        yield return new WaitForSecondsRealtime(secs);
    }
    public void SetScore(string score) //Setting the score
    {
        proUGUI.text = "Score: " + score;
    }
}
