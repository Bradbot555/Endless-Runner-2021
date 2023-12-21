using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController uI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI addText;
    public TextMeshProUGUI doubleText;
    private Animator ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = addText.GetComponent<Animator>();
        if (uI != null && uI != this)
        {
            Destroy(gameObject);
        }
        uI = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetScore(string score)
    {
        scoreText.text = "Score: " + score;
    }
    public void AddScore(string add)
    {
        ani.Play("PointAdd(MainGame)",0,0.01f);
        addText.text = "+" + add;
    }

    public void DoublePoints(bool State)
    {
        if (State == true)
        {
            doubleText.enabled = true;
        }
        else
        {
            doubleText.enabled = false;
        }
    }

}
