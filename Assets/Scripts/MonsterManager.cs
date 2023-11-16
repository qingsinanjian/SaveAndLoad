using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private Animation ani;
    public AnimationClip idleClip;
    public AnimationClip dieClip;

    private void Awake()
    {
        ani = GetComponent<Animation>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            this.GetComponent<BoxCollider>().enabled = false;
            ani.clip = dieClip;
            ani.Play();
            StartCoroutine(DeActivate());
            UIManager.instance.AddScore();
        }
    }

    private void OnDisable()
    {
        ani.clip = idleClip;
    }

    IEnumerator DeActivate()
    {
        yield return new WaitForSeconds(0.85f);
        TargetManager.instance.UpdateMonsters();
    }
}
