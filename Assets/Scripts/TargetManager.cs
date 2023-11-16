using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] monsters;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var monster in monsters)
        {
            monster.SetActive(false);
            monster.GetComponent<BoxCollider>().enabled = false;
        }
        //ActiveMonster();
        StartCoroutine(AliveTimer());
    }

    private void ActiveMonster()
    {
        int index = Random.Range(0, monsters.Length);
        monsters[index].SetActive(true);
        monsters[index].GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator AliveTimer()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        ActiveMonster();
    }
}
