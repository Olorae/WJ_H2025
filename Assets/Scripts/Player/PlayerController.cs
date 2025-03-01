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
        
        coroutine = WaitAndPrint(0.5f);
       
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
        ObjectsInHitBox.Remove(other.GameObject());
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
        rb.velocity = new Vector2(moveValue.x * baseSpeed, moveValue.y * baseSpeed);

    }

    public void Attack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
        List<GameObject> toDestroy = new();
        foreach (GameObject gObject in ObjectsInHitBox)
        {
            if (gObject.tag.Equals("Enemy"))
            {
                toDestroy.Add(gObject);
                //TODO: remove health from enemy and add to destroy list if health equals 0

            }
        }

        foreach (GameObject gObject in toDestroy)
        {
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
}
