using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponScriptableObject")]
public class weaponsScriptableObject : ScriptableObject
{
    //name and description
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
}
