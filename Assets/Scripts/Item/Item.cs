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
    public string description;
    public string type;
    public float attackRange;
    public float attackDamage;
    public float mouvementSpeed;
    public float madnessDefense;
    public float madnessPerSecondReduce;
    public float bossSpawnChanceReduction;
    public Color rarityColor;
    public bool firstItem;
    public GameObject popUpPreFab;
    protected GameObject popUpInstance;
    protected bool isPickable = false;
    private int switchWorldCount = 0;
    public Sprite WeaponSprite;
    public Sprite HatSprite;
    public Sprite ShirtSprite;


    string Name
    {
        get { return name; }
        set { name = value; }
    }
    string Description
    {
        get { return description; }
        set { description = value; }
    }

    string Type
    {
        get { return type; }
        set { type = value; }
    }

    float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
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
        // GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += destroySwitchWorld;

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

        if (other.tag == "Player" && GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand.Equals(false))
        {
            other.GameObject().GetComponent<PlayerController>().pickableItem = this;
            isPickable = true;
            
            Vector3 vectorItemPosition = transform.position;
            vectorItemPosition.y = vectorItemPosition.y + 1.5f;
            
            popUpInstance = Instantiate(popUpPreFab, vectorItemPosition, Quaternion.identity);
            switch (type)
            {
                case "Weapon":
                    popUpInstance.transform.Find("PopUp/BackGround/Title").GetComponent<TextMeshProUGUI>().text = "Sword of";
                    popUpInstance.transform.Find("PopUp/BackGround/Description").GetComponent<TextMeshProUGUI>().text = description;
                    
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text = "Attack Damage:";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().text = compareStat(this,"Attack Damage").ToString();

                    
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text = $"Attack Range: {attackRange }";
                    break;
                case "Armor":
                    popUpInstance.transform.Find("PopUp/BackGround/Title").GetComponent<TextMeshProUGUI>().text = "Armor of";
                    popUpInstance.transform.Find("PopUp/BackGround/Description").GetComponent<TextMeshProUGUI>().text = description;
                    
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text = "Madness Defense:";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().text = compareStat(this,"Madness Defense").ToString();
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text = "Mouvement Speed:";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2/Stat2Number").GetComponent<TextMeshProUGUI>().text = compareStat(this,"Mouvement Speed").ToString();
                    break;
                case "Hat":
                    popUpInstance.transform.Find("PopUp/BackGround/Title").GetComponent<TextMeshProUGUI>().text = "Hat of";
                    popUpInstance.transform.Find("PopUp/BackGround/Description").GetComponent<TextMeshProUGUI>().text = description;
                    popUpInstance.transform.Find("PopUp/BackGround/Stat1").GetComponent<TextMeshProUGUI>().text = $"Madness Per Second Reduce: {MadnessPerSecondReduce} %";
                    popUpInstance.transform.Find("PopUp/BackGround/Stat2").GetComponent<TextMeshProUGUI>().text = $"Reduce Boss Chance: {BossSpawnChanceReduction}";
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
            FindObjectOfType<PlayerController>().EnableInsanity();
            GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand.Invoke();
            GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = true;
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
            if (switchWorldCount == 1)
            { 
                Destroy(this.gameObject);
                switchWorldCount = 0;
            }
            else
            {
                switchWorldCount++;
            }
           
            
        }
    }

    private void destroySwitchWorld()
    {
        if (FindObjectsOfType<Item>().Length > 0)
        {
            // Destroy(this.gameObject);

        }
    }


    private void OnDestroy()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= visible;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= invisible;
    }

    private float compareStat(Item ItemOnGround, String statName)
    {
        float differenceStat = 0;
        switch (ItemOnGround.type)
        {
            case "Weapon":
                if (statName == "Attack Damage")
                {
                    differenceStat = ItemOnGround.attackDamage - FindObjectOfType<PlayerController>().GetDamage() + FindObjectOfType<PlayerController>().baseDamage;
                    if(differenceStat > 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                    else if (differenceStat < 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                    }
                }
                else if (statName == "Attack Range")
                {
                    differenceStat = ItemOnGround.attackRange - FindObjectOfType<PlayerController>().weapon.attackRange;
                    if(differenceStat > 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                    else if (differenceStat < 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                    }
                }
                break;
            case "Hat":
                if (statName == "Madness Per Second Reduce")
                {
                    differenceStat = ItemOnGround.madnessPerSecondReduce - FindObjectOfType<PlayerController>().hat.madnessPerSecondReduce;
                    if(differenceStat > 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                    else if (differenceStat < 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                    }
                
                }
                else if (statName == "Reduce Boss Chance")
                {
                    differenceStat = ItemOnGround.bossSpawnChanceReduction - FindObjectOfType<PlayerController>().hat.bossSpawnChanceReduction;
                    if(differenceStat > 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                    else if (differenceStat < 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                    }
                }
                break;
            case "Armor":
                if (statName == "Madness Defense")
                {
                   differenceStat = ItemOnGround.madnessDefense - FindObjectOfType<PlayerController>().GetDefense() + FindObjectOfType<PlayerController>().baseDefense;
                   if(differenceStat > 0)
                   {
                       popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                   }
                   else if (differenceStat < 0)
                   {
                       popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                   }
                   else
                   {
                       popUpInstance.transform.Find("PopUp/BackGround/Stat1/Stat1Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                   }
                   
                }
                else if (statName == "Mouvement Speed")
                {
                    differenceStat = ItemOnGround.mouvementSpeed - FindObjectOfType<PlayerController>().GetMovementSpeed() + FindObjectOfType<PlayerController>().baseSpeed ;
                    if(differenceStat > 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat2/Stat2Number").GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                    else if (differenceStat < 0)
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat2/Stat2Number").GetComponent<TextMeshProUGUI>().color = Color.red;

                    }
                    else
                    {
                        popUpInstance.transform.Find("PopUp/BackGround/Stat2/Stat2Number").GetComponent<TextMeshProUGUI>().color = Color.white;

                    }
                }
                break;
            default:
                break;
        }
        return differenceStat;
    }
}

    