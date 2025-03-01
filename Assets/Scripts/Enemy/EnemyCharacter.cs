using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerController Player = null;
    public float Speed;
    public Rigidbody2D Rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    public CircleCollider2D CircleCollider2D;
    public bool RealEnemy;
    private bool followPlayer;
    private bool TouchingPlayer;
    public float Life;
    public float Damage;
    private IEnumerator coroutine;
    
    void Start()
    {
        // Initialisations
        Player = FindObjectOfType<PlayerController>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        followPlayer = true;
        TouchingPlayer = false;
        
        if (Player == null)
        {
            Debug.Log("Player not found");
            return;
            // TODO: disable
        }
        
        Debug.Log("Player : "+ Player.name +" found \\^o^/");
    }

    private void FixedUpdate()
    {
       if (followPlayer == true)
        {
            Vector3 currentLocation = transform.position;
            Vector3 targetLocation = Player.transform.position;
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, targetLocation, Speed * Time.deltaTime);
            Rigidbody2D.position = newPosition;
        }
    }
    
    public bool Attacked(float damage)
    {
        if (RealEnemy)
        {
            Life -= damage;
        }
        else
        {
            Life = 0;
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage);
        }
        Debug.Log("Life = " + Life);
        return Life <= 0;
        
    }

    // Enemy touched player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            followPlayer = false;
            TouchingPlayer = true;

            if (RealEnemy)
            {
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage);
                coroutine = WaitAndPrint(5.0f);
                StartCoroutine(coroutine);
            }
            else
            {
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(-Damage/2);
                Destroy(this.GameObject());
                Destroy(this);
            }
        }
    }
    
    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (TouchingPlayer) {
        yield return new WaitForSeconds(waitTime);
        // TODO: Augmenter la folie du joueur
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        followPlayer = true;
        TouchingPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}