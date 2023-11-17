using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public bool isPaused = true;
    public GameObject menuGo;
    public GameObject[] targetGos;

    private void Awake()
    {
        _instance = this;
        Pause();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        menuGo.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    private void UnPause()
    {
        isPaused = false;
        menuGo.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void ContinueGame()
    {
        UnPause();
    }

    public void NewGame()
    {
        foreach (var targetGo in targetGos)
        {
            targetGo.GetComponent<TargetManager>().UpdateMonsters();
        }
        
        UIManager.instance.shootNum = 0;
        UIManager.instance.score = 0;
        UnPause();
    }

    public void QuitGame()
    {
        // 在编辑器中检查是否在播放模式中
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            // 在编辑器中停止播放模式
            EditorApplication.isPlaying = false;
        }
#endif

        // 在构建后的应用程序中调用Application.Quit()来退出游戏
        Application.Quit();
    }
}
