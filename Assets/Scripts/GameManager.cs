using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml;

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

    private Save CreateSaveGo()
    {
        Save save = new Save();
        foreach (GameObject targetGo in targetGos)
        {
            TargetManager targetManager = targetGo.GetComponent<TargetManager>();
            if(targetManager.activeMonster != null)
            {
                save.livingTargetPositions.Add(targetManager.targetPosition);
                int type = targetManager.activeMonster.GetComponent<MonsterManager>().monsterType;
                save.livingMonsterTypes.Add(type);
            }
        }
        save.shootNum = UIManager.instance.shootNum;
        save.score = UIManager.instance.score;

        return save;
    }

    private void SetGame(Save save)
    {
        foreach (var targetGo in targetGos)
        {
            targetGo.GetComponent<TargetManager>().UpdateMonsters();
        }

        for (int i = 0; i < save.livingTargetPositions.Count; i++)
        {
            int position = save.livingTargetPositions[i];
            int type = save.livingMonsterTypes[i];
            targetGos[position].GetComponent<TargetManager>().ActivateMonsterByType(type);
        }
        UIManager.instance.shootNum = save.shootNum;
        UIManager.instance.score = save.score;

        UnPause();
    }

    private void SaveByBin()
    {
        //序列化过程（将Save对象转换为字节流）
        Save save = CreateSaveGo();
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = File.Create(Application.dataPath + "/StreamingFile/" + "byBin.txt");
        //用二进制格式化程序来序列化Save对象
        bf.Serialize(fileStream, save);
        fileStream.Close();

        if(File.Exists(Application.dataPath + "/StreamingFile/" + "byBin.txt"))
        {
            UIManager.instance.ShowMessage("保存成功");
        }
    }

    private void LoadByBin()
    {
        if(File.Exists(Application.dataPath + "/StreamingFile/" + "byBin.txt"))
        {
            //反序列化过程
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.dataPath + "/StreamingFile/" + "byBin.txt", FileMode.Open);
            Save save = (Save)bf.Deserialize(fileStream);
            fileStream.Close();
            SetGame(save);
        }
        else
        {
            UIManager.instance.ShowMessage("存档文件不存在");
        }
    }

    private void SaveByXml()
    {
        Save save = CreateSaveGo();
        string filePath = Application.dataPath + "/StreamingFile" + "/byXml.txt";
        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveFile1");

        XmlElement target;
        XmlElement targetPosition;
        XmlElement monsterType;

        for (int i = 0; i < save.livingTargetPositions.Count; i++)
        {
            target = xmlDoc.CreateElement("target");
            targetPosition = xmlDoc.CreateElement("targetPosition");
            targetPosition.InnerText = save.livingTargetPositions[i].ToString();
            monsterType = xmlDoc.CreateElement("monsterType");
            monsterType.InnerText = save.livingMonsterTypes[i].ToString();

            target.AppendChild(targetPosition);
            target.AppendChild(monsterType);
            root.AppendChild(target);
        }

        XmlElement shootNum = xmlDoc.CreateElement("shootNum");
        shootNum.InnerText = save.shootNum.ToString();
        root.AppendChild(shootNum);

        XmlElement score = xmlDoc.CreateElement("score");
        score.InnerText = save.score.ToString();
        root.AppendChild(score);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if (File.Exists(filePath))
        {
            UIManager.instance.ShowMessage("保存成功");
        }
    }

    private void LoadByXml()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/byXml.txt";
        if (File.Exists(filePath))
        {
            Save save = new Save();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            //通过节点名称来获取元素，结果为XmlNodeList类型
            XmlNodeList targets = xmlDoc.GetElementsByTagName("target");
            //遍历所有target节点，并获得子节点和子节点的InnerText
            if(targets.Count > 0)
            {
                foreach (XmlNode target in targets)
                {
                    XmlNode targetPosition = target.ChildNodes[0];
                    int targetPositionIndex = int.Parse(targetPosition.InnerText);
                    save.livingTargetPositions.Add(targetPositionIndex);

                    XmlNode monsterType = target.ChildNodes[1];
                    int monsterTypeIndex = int.Parse(monsterType.InnerText);
                    save.livingMonsterTypes.Add(monsterTypeIndex);
                }
            }

            XmlNodeList shootNum = xmlDoc.GetElementsByTagName("shootNum");
            int shootNumCount = int.Parse(shootNum[0].InnerText);
            save.shootNum = shootNumCount; 

            XmlNodeList score = xmlDoc.GetElementsByTagName("score");
            int scoreCount = int.Parse(score[0].InnerText);
            save.score = scoreCount;

            SetGame(save);
        }
        else
        {
            UIManager.instance.ShowMessage("存档文件不存在");
        }
    }

    private void SaveByJson()
    {
        Save save = CreateSaveGo();
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        string saveJsonStr = JsonConvert.SerializeObject(save);
        File.WriteAllText(filePath, saveJsonStr);
        UIManager.instance.ShowMessage("保存成功");
    }

    private void LoadByJson()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        if(File.Exists(filePath))
        {
            Save save = JsonConvert.DeserializeObject<Save>(File.ReadAllText(filePath));
            SetGame(save);
        }
        else
        {
            UIManager.instance.ShowMessage("存档文件不存在");
        }
    }

    public void ContinueGame()
    {
        UnPause();
        UIManager.instance.ShowMessage("");
    }

    public void NewGame()
    {
        foreach (var targetGo in targetGos)
        {
            targetGo.GetComponent<TargetManager>().UpdateMonsters();
        }
        
        UIManager.instance.shootNum = 0;
        UIManager.instance.score = 0;
        UIManager.instance.ShowMessage("");
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

    public void SaveGame()
    {
        //SaveByBin();
        //SaveByJson();
        SaveByXml();
    }

    public void LoadGame()
    {
        //LoadByBin();
        //LoadByJson();
        LoadByXml();
        UIManager.instance.ShowMessage("");
    }
}
