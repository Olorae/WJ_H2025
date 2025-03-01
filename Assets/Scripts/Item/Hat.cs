using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : Item
{
    protected override void Initialize()
    {
        name = "Hat";
        foliePerSecondReduce = Random.Range(0, 100);
        bossSpawnChanceReduction = Random.Range(0, 100);
    }
}
