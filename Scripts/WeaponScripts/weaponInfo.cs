using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponInfo : MonoBehaviour
{
    public new string name;
    public string description;

    //Sell
    public int sellPrice;

    //ID
    public int WeaponID;

    //Atribute and type
    public string weaponType;
    public string[] weaponAttributes;

    //Stats
    public int ATT;
    public int DEF;

    public int STR;
    public int CON;
    public int INT;
    public int LCK;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
