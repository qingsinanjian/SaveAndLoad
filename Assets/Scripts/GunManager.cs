using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public float maxYRotation = 120f;
    public float minYRotation = 0.0f;

    public float maxXRotation = 60f;
    public float minXRotation = 0.0f;

    private float shootTime = 1;
    private float shootTimer = 0f;

    public GameObject bulletGo;
    public Transform firePosition;
    public float force = 2000;
    private AudioSource bulletSound;

    private void Awake()
    {
        bulletSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(GameManager._instance.isPaused)
        {
            return;
        }

        shootTimer += Time.deltaTime;
        if(shootTimer > shootTime)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameObject bulletCurrent = Instantiate(bulletGo, firePosition.position, Quaternion.identity);
                bulletCurrent.GetComponent<Rigidbody>().AddForce(transform.forward * force);
                this.GetComponent<Animation>().Play();
                shootTimer = 0;
                bulletSound.Play();
                UIManager.instance.AddShootNum();

            }
        }

        float xPosPrecent = Input.mousePosition.x / Screen.width;
        float yPosPrecent = Input.mousePosition.y / Screen.height;

        float xAngle = -Mathf.Clamp(yPosPrecent * maxXRotation, minXRotation, maxXRotation) + 15;
        float yAngle = Mathf.Clamp(xPosPrecent * maxYRotation, minYRotation, maxYRotation) - 60;

        transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
    }
}
