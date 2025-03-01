
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public IAPlayer playerInput;
    public float baseSpeed = 5f;
    public float baseDamage = 5;
    public float baseDefense = 0;
    public Rigidbody2D rb;
    private BoxCollider2D WeapondHitBox;
    private InputAction move;
    private List<GameObject> ObjectsInHitBox;
    private Animator animator;
    private bool StopCoroutine;
    public bool InsanityEnabled;
    public bool HitElapsed;
    public ItemData armor;
    public ItemData weapon;
    public ItemData hat;
    public Item pickableItem;
    public  GameObject WeaponPrefab;
    public  GameObject HatPrefab;
    public  GameObject ArmorPrefab;
    public GameObject portalRef;
    public GameObject insanityBarRef;
    private float EndHitCooldown;
    private float CoolDownTime = 2f;

    private void Awake()
    {
        ObjectsInHitBox = new();
        playerInput = new IAPlayer();
        rb = GetComponent<Rigidbody2D>();
        WeapondHitBox = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;
        InsanityEnabled = false;
        StopCoroutine = false;
        
        coroutine = WaitAndPrint(1f);
        HitElapsed = true;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand.Invoke();
    }
   
    
    private IEnumerator coroutine;
    private IEnumerator WaitAndPrint(float waitTime)
    {
        //Debug.Log("wait and print");
        while (true) {
            yield return new WaitForSeconds(waitTime);
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(0.5f,false);
        }
    }
    private void ToLivingLand()
    {
        StopCoroutine(coroutine);
    }

    private void ToDeadLand()
    {
        if (InsanityEnabled)
        {
            StartCoroutine(coroutine);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (EndHitCooldown != 0 && EndHitCooldown < Time.time)
        {
            HitElapsed = true;
            EndHitCooldown = 0;
        }
    }

    public void OnHit(float damage)
    {
        Debug.Log(HitElapsed);
        if (HitElapsed)
        {
            HitElapsed = false;
            EndHitCooldown = Time.time + CoolDownTime;
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(damage,true);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ObjectsInHitBox.Add(other.GameObject());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null)
        {
            ObjectsInHitBox.Remove(other.GameObject());
        }
        
    }

    private void OnEnable()
    {
        move = playerInput.Player.Move;
        move.Enable();
        playerInput.Player.Attack.performed += Attack;
        playerInput.Player.Attack.Enable();
        playerInput.Player.Pickup.performed += Pickup;
        playerInput.Player.Pickup.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        //Debug.Log(GetMovementSpeed());
        rb.velocity = new Vector2(moveValue.x * GetMovementSpeed(), moveValue.y * GetMovementSpeed());

    }

    public void Attack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
        List<GameObject> toDestroy = new();
        Debug.Log("attack");
        foreach (GameObject gObject in ObjectsInHitBox)
        {
            if (gObject != null)
            {
                if (gObject.tag.Equals("Enemy"))
                {
                    //TODO: give knockback to enemy
                    if (gObject.GetComponent<EnemyCharacter>().Attacked(GetDamage()))
                    {
                        toDestroy.Add(gObject);
                    }

                }
            }
            
        }

        foreach (var gObject in toDestroy)
        {
            gObject.GetComponent<EnemyCharacter>().animator.SetTrigger("Death");
            gObject.GetComponent<EnemyCharacter>().followPlayer = false;
            ObjectsInHitBox.Remove(gObject);
           //Destroy(gObject); 
        }
        
        //Debug.Log(GetDamage());
        
    }

    public void Pickup(InputAction.CallbackContext obj)
    {
        if (!GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand && pickableItem != null)
        {
            switch (pickableItem.type)
            {
            case "Hat":
                Debug.Log("hat");
                hat = new ItemData();
                hat.attackDamage = pickableItem.attackDamage;
                hat.mouvementSpeed = pickableItem.mouvementSpeed;
                hat.madnessDefense = pickableItem.madnessDefense;
                hat.attackSpeed = pickableItem.attackSpeed;
                hat.bossSpawnChanceReduction = pickableItem.bossSpawnChanceReduction;
                hat.madnessPerSecondReduce = pickableItem.madnessPerSecondReduce;
                hat.type = pickableItem.type;
                hat.name = pickableItem.name;
                pickableItem.ItemPickedUp();
                break;
            case "Armor":
                Debug.Log("armor");
                armor = new ItemData();
                armor.attackDamage = pickableItem.attackDamage;
                armor.mouvementSpeed = pickableItem.mouvementSpeed;
                armor.madnessDefense = pickableItem.madnessDefense;
                armor.attackSpeed = pickableItem.attackSpeed;
                armor.bossSpawnChanceReduction = pickableItem.bossSpawnChanceReduction;
                armor.madnessPerSecondReduce = pickableItem.madnessPerSecondReduce;
                armor.type = pickableItem.type;
                armor.name = pickableItem.name;
                //armor = pickableItem;
                pickableItem.ItemPickedUp();
                break;
            case "Weapon":
                Debug.Log("weapon");
                weapon = new ItemData();
                weapon.attackDamage = pickableItem.attackDamage;
                weapon.mouvementSpeed = pickableItem.mouvementSpeed;
                weapon.madnessDefense = pickableItem.madnessDefense;
                weapon.attackSpeed = pickableItem.attackSpeed;
                weapon.bossSpawnChanceReduction = pickableItem.bossSpawnChanceReduction;
                weapon.madnessPerSecondReduce = pickableItem.madnessPerSecondReduce;
                weapon.type = pickableItem.type;
                weapon.name = pickableItem.name;
                //weapon = pickableItem;
                pickableItem.ItemPickedUp();
                break;
            default:
                break;
            }
        
        }
        else
        {
            Debug.Log("Cant Pick Up in the living");
        }
    }

    public float GetDefense()
    {
        return baseDefense + ((hat != null)? hat.madnessDefense : 0) + ((armor != null)? armor.madnessDefense : 0) + ((weapon != null)? weapon.madnessDefense : 0) ;
    }

    public float GetDamage()
    {
        return baseDamage + ((hat != null)? hat.attackDamage : 0) + ((armor != null)? armor.attackDamage : 0) + ((weapon != null)? weapon.attackDamage : 0) ;
    }

    public float GetMovementSpeed()
    {
        
        return baseSpeed + ((hat != null)? hat.mouvementSpeed : 0) + ((armor != null)? armor.mouvementSpeed : 0) + ((weapon != null)? weapon.mouvementSpeed : 0) ;
    }

    private void OnDestroy()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;
    }

    public void EnableInsanity()
    {
       Debug.Log("INSANITYENABLED");
       portalRef.SetActive(true);
       insanityBarRef.SetActive(true);
       
        InsanityEnabled = true;
    }
}
