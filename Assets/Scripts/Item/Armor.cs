using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{ 
    protected override void Initialize()
    {
        name = "Armor test";
        type = "Armor";
        mouvementSpeed = Random.Range(0, 100);
        madnessDefense = Random.Range(0, 100);
    }
}
