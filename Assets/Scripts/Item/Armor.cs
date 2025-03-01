using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{ 
    protected override void Initialize()
    {
        name = "Armor";
        mouvementSpeed = Random.Range(0, 100);
        folieHitDefense = Random.Range(0, 100);
    }
}
