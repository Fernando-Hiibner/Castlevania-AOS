using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackController : MonoBehaviour
{

    public GameObject equipedWeapon;
    [HideInInspector]
    playerController myPC;
    [HideInInspector]
    public GameObject instantiatedWeapon;
    weaponInfo weaponInfoScript;
    public Transform attackPos;
    public Transform attackDamagePos;
    public LayerMask enemyLayerMask;
    public Vector2 attackRange; 

    string weaponType;
    string[] weaponAttributes;

    void Start()
    {
        
    }

    public void LightAttack()
    {
        myPC = GetComponent<playerController>();
        if (myPC.facingRight)
        {
            instantiatedWeapon = Instantiate(equipedWeapon, attackPos.position, Quaternion.Euler(0,0,0));
        }
        else
        {
            instantiatedWeapon = Instantiate(equipedWeapon, attackPos.position, Quaternion.Euler(0, 180, 0));
        }
        instantiatedWeapon.transform.parent = attackPos;
        Animator weaponAnimator = attackPos.GetComponentInChildren<Animator>();
        weaponAnimator.Play("lAtk");
    }
    public void CrouchedLightAttack()
    {
        myPC = GetComponent<playerController>();
        if (myPC.facingRight)
        {
            instantiatedWeapon = Instantiate(equipedWeapon, attackPos.position, Quaternion.Euler(0,0,0));
        }
        else
        {
            instantiatedWeapon = Instantiate(equipedWeapon, attackPos.position, Quaternion.Euler(0, 180, 0));
        }
        instantiatedWeapon.transform.parent = attackPos;
        Animator weaponAnimator = attackPos.GetComponentInChildren<Animator>();
        weaponAnimator.Play("cAtk");
    }
}
