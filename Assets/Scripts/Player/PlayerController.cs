
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
    public Item armor;
    public Item weapon;
    public Item hat;
    public Item pickableItem;
    public  GameObject WeaponPrefab;
    public  GameObject HatPrefab;
    public  GameObject ArmorPrefab;
    

    private void Awake()
    {
        ObjectsInHitBox = new();
        playerInput = new IAPlayer();
        rb = GetComponent<Rigidbody2D>();
        WeapondHitBox = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;
        
        StopCoroutine = false;
        
        coroutine = WaitAndPrint(1f);
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand.Invoke();
    }
    private IEnumerator coroutine;
    private IEnumerator WaitAndPrint(float waitTime)
    {
        Debug.Log("wait and print");
        while (true) {
            yield return new WaitForSeconds(waitTime);
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(1);
        }
    }
    private void ToLivingLand()
    {
        Debug.Log("in living land");
        StopCoroutine(coroutine);
    }

    private void ToDeadLand()
    {
        Debug.Log("in dead land");
        StartCoroutine(coroutine);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        rb.velocity = new Vector2(moveValue.x * baseSpeed, moveValue.y * GetMovementSpeed());

    }

    public void Attack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
        List<GameObject> toDestroy = new();
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
            ObjectsInHitBox.Remove(gObject);
           Destroy(gObject); 
        }
        
        Debug.Log("Attack");
        
    }

    public void Pickup(InputAction.CallbackContext obj)
    {
        if (!GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand && pickableItem != null)
        {
            switch (pickableItem.type)
            {
            case "Hat":
                hat = pickableItem;
                pickableItem.ItemPickedUp();
                break;
            case "Armor":
                armor = pickableItem;
                pickableItem.ItemPickedUp();
                break;
            case "Weapon":
                weapon = pickableItem;
                pickableItem.ItemPickedUp();
                break;
            default:
                break;
            }
        Debug.Log(pickableItem);
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
        return baseDamage + ((hat != null)? hat.mouvementSpeed : 0) + ((armor != null)? armor.mouvementSpeed : 0) + ((weapon != null)? weapon.mouvementSpeed : 0) ;
    }
}
