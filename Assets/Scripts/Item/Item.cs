using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected string name;
    protected float attackSpeed;
    protected float attackDamage;
    protected float mouvementSpeed;
    protected float folieHitDefense;
    protected float foliePerSecondReduce;
    protected float bossSpawnChanceReduction;

    string Name
    {
        get { return name; }
        set { name = value; }
    }

    float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }

    float AttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    float MouvementSpeed
    {
        get { return mouvementSpeed; }
        set { mouvementSpeed = value; }
    }

    float FolieHitDefense
    {
        get { return folieHitDefense; }
        set { folieHitDefense = value; }
    }

    float FoliePerSecondReduce
    {
        get { return foliePerSecondReduce; }
        set { foliePerSecondReduce = value; }
    }

    float BossSpawnChanceReduction
    {
        get { return bossSpawnChanceReduction; }
        set { bossSpawnChanceReduction = value; }
    }
/*
    public void SpawnItemStatWindow()
    {
        if (FindObjectOfType<Player>())
        {
            
        }
    }
    */

}