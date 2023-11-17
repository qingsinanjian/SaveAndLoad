using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;
    public GameObject[] monsters;
    public GameObject activeMonster;
    private void Awake()
    {
        instance = this;
    }

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
        activeMonster = monsters[index];
        activeMonster.SetActive(true);
        activeMonster.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DeathTimer());
    }

    IEnumerator AliveTimer()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        ActiveMonster();
    }

    private void DeActivateMonster()
    {
        if (activeMonster != null)
        {
            activeMonster.GetComponent<BoxCollider>().enabled = false;
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        StartCoroutine(AliveTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(Random.Range(3, 8));
        DeActivateMonster();
    }

    public void UpdateMonsters()
    {
        StopAllCoroutines();
        if (activeMonster)
        {
            activeMonster.SetActive(false);
            activeMonster = null;
        }
        StartCoroutine(AliveTimer());
    }
}
