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

    private void Awake()
    {
        instance = this;
        if (PlayerPrefs.HasKey("MusicOn"))
        {
            if(PlayerPrefs.GetInt("MusicOn") == 1)
            {
                musicToggle.isOn = true;
                musicAudio.enabled = true;
            }
            else
            {
                musicToggle.isOn = false;
                musicAudio.enabled = false;
            }
        }
        else
        {
            musicToggle.isOn = true;
            musicAudio.enabled = true;
        }
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

    public void MusicAudioSwitch()
    {
        if(musicToggle.isOn)
        {
            musicAudio.enabled = true;
            PlayerPrefs.SetInt("MusicOn", 1);
        }
        else
        {
            musicAudio.enabled = false;
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        PlayerPrefs.Save();
    }
}
