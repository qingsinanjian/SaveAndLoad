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

    public Toggle musicToggle;
    public AudioSource musicAudio;
    private bool musicOn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        shootNumText.text = shootNum.ToString();
        scoreText.text = score.ToString();
        MusicAudioSwitch();
    }

    public void AddShootNum()
    {
        shootNum++;
    }

    public void AddScore()
    {
        score++;
    }

    private void MusicAudioSwitch()
    {
        if(musicToggle.isOn)
        {
            musicOn = true;
            musicAudio.enabled = true;
        }
        else
        {
            musicOn = false;
            musicAudio.enabled = false;
        }
    }
}
