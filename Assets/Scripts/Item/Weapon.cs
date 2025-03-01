using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Weapon : Item
{
    protected override void Initialize()
    {
        name = "Épée Test";
        type = "Weapon";
        attackDamage = Random.Range(0, 100);
        attackSpeed = Random.Range(0, 100);
    }
}
