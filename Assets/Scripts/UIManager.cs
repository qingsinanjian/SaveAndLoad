using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text shootNumText;
    public Text scoreText;

    public int shootNum;
    public int score;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        shootNumText.text = shootNum.ToString();
        scoreText.text = score.ToString();
    }

    public void AddShootNum()
    {
        shootNum++;
    }

    public void AddScore()
    {
        score++;
    }
}
