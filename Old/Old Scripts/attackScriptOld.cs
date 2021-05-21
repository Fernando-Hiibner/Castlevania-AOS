using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScriptOld : MonoBehaviour
{
    public static attackScriptOld instance;
    //Important Player Components
    [HideInInspector]
    public Animator myAnim;
    public GameObject currentWeapon;

    public Transform attackPos;

    //Attack Variables
    [HideInInspector]
    public bool canReceiveInput;
    [HideInInspector]
    public bool InputReceived;


    // Start is called before the first frame update
    void Awake(){
        instance = this;
        canReceiveInput = true;
    }
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && playerControllerOld.instance.isGrounded){
            Attack();
        }   
    }
    public void Attack(){
        if(canReceiveInput){
            InputReceived = true;
            playerControllerOld.instance.attacking = true;
            canReceiveInput = false;
        }else{
            return;
        }
    }
    public void InputManager(){
        if(!canReceiveInput){
            canReceiveInput = true;
        }else{
            canReceiveInput = false;
        }
    }
}
