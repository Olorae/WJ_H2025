using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    protected override void Initialize()
    {
        name = "Weapon";
        attackDamage = Random.Range(0, 100);
        attackSpeed = Random.Range(0, 100);
    }
}
