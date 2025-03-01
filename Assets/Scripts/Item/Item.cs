using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Item : MonoBehaviour
{
    public string name;
    protected float attackSpeed;
    protected float attackDamage;
    protected float mouvementSpeed;
    protected float folieHitDefense;
    protected float foliePerSecondReduce;
    protected float bossSpawnChanceReduction;
    public GameObject popUpPreFab;
    protected GameObject popUpInstance;

    
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

    public void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            popUpInstance = Instantiate(popUpPreFab,transform.position,Quaternion.identity);
            switch (name)
            {
                case "Weapon":
                    popUpInstance.transform.Find("PopUp/BackGround/Name").GetComponent<TextMeshProUGUI>().text = $"Name: {Name}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text = $"Attack Damage: {AttackDamage}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text = $"Attack Speed: {AttackSpeed}";
                    break;
                case "Armor":
                    popUpInstance.transform.Find("PopUp/BackGround/Name").GetComponent<TextMeshProUGUI>().text = $"Name: {Name}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text = $"Madness Defense: {AttackDamage}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text = $"Mouvement Speed: {AttackSpeed}";
                    break;
                case "Hat":
                    break;
                default:
                    break;
            }
           
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (popUpInstance != null)
            { 
                Destroy(popUpInstance);
            }
        }
    }
}

    