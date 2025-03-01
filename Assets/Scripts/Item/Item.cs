using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Experimental.AI;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour
{
    public string name;
    public string type;
    public float attackSpeed;
    public float attackDamage;
    public float mouvementSpeed;
    public float madnessDefense;
    public float madnessPerSecondReduce;
    public float bossSpawnChanceReduction;
    public bool firstItem;
    public GameObject popUpPreFab;
    protected GameObject popUpInstance;
    protected bool isPickable = false;


    string Name
    {
        get { return name; }
        set { name = value; }
    }

    string Type
    {
        get { return type; }
        set { type = value; }
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

    float MadnessDefense
    {
        get { return madnessDefense; }
        set { madnessDefense = value; }
    }

    float MadnessPerSecondReduce
    {
        get { return madnessPerSecondReduce; }
        set { madnessPerSecondReduce = value; }
    }

    float BossSpawnChanceReduction
    {
        get { return bossSpawnChanceReduction; }
        set { bossSpawnChanceReduction = value; }
    }

    public void Start()
    {
        Initialize();
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += visible;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += invisible;
        if (GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand)
        {
           invisible();
        }
        else
        {
           visible();
        }
    }

    protected virtual void Initialize()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" &&
            GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand.Equals(false))
        {
            other.GameObject().GetComponent<PlayerController>().pickableItem = this;
            isPickable = true;
            popUpInstance = Instantiate(popUpPreFab, transform.position, Quaternion.identity);
            switch (type)
            {
                case "Weapon":
                    popUpInstance.transform.Find("PopUp/BackGround/Name").GetComponent<TextMeshProUGUI>().text =
                        $"Name: {Name}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text =
                        $"Attack Damage: {AttackDamage}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text =
                        $"Attack Speed: {AttackSpeed}";
                    break;
                case "Armor":
                    popUpInstance.transform.Find("PopUp/BackGround/Name").GetComponent<TextMeshProUGUI>().text =
                        $"Name: {Name}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text =
                        $"Madness Defense: {MadnessDefense}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text =
                        $"Mouvement Speed: {MouvementSpeed}";
                    break;
                case "Hat":
                    popUpInstance.transform.Find("PopUp/BackGround/Name").GetComponent<TextMeshProUGUI>().text =
                        $"Name: {Name}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text =
                        $"Madness Per Second Reduce: {MadnessPerSecondReduce}";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text =
                        $"Reduce Boss Chance: {BossSpawnChanceReduction}";
                    break;
                default:
                    break;
            }

        }

    }

    public void ItemPickedUp()
    {
        Destroy(popUpInstance);
        if (firstItem)
        {
            FindObjectOfType<SpawnManager>().StartSpawning();
        }
        Item itemToDestroy = this.GetComponent<Item>();
        Destroy(itemToDestroy.gameObject);
        Destroy(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" &&
            GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand.Equals(false))
        {
            other.GameObject().GetComponent<PlayerController>().pickableItem = null;
            isPickable = false;
            if (popUpInstance != null)
            {
                Destroy(popUpInstance);
            }
        }
    }

    private void visible()
    {
        if (FindObjectsOfType<Item>().Length > 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color tempColor = sr.color;
            tempColor.a = 1f;
            sr.color = tempColor;
        }
    }

    private void invisible()
    {
        if (FindObjectsOfType<Item>().Length > 0)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color tempColor = sr.color;
            tempColor.a = 0.5f;
            sr.color = tempColor;
        }
    }


    private void OnDestroy()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= visible;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= invisible;
    }
}

    