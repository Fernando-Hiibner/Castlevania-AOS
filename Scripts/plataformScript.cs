using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plataformScript : MonoBehaviour
{
    PlatformEffector2D myPlatform;
    public float restartTime;
    float waitTime;
    bool down;
   
    // Start is called before the first frame update
    void Start()
    {
        myPlatform = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(down == true)
        {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0f)
            {
                down = false;
                myPlatform.rotationalOffset = 0f;
            }
        }
    }
    public void Rotate()
    {
        myPlatform.rotationalOffset = 180f;
        waitTime = restartTime;
        down = true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerController myPC = other.transform.root.GetComponent<playerController>();
            myPC.onPlatform = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController myPC = other.transform.root.GetComponent<playerController>();
            myPC.onPlatform = false;
        }
    }
}
