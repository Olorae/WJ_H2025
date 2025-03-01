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

    private bool runAway;

    private void bossSpawned()
    {
        runAway = true;
    }

    void Start()
    {
        // Initialisations
        Player = FindObjectOfType<PlayerController>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        followPlayer = true;
        TouchingPlayer = false;
        runAway = false;
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;

        FindObjectOfType<SpawnManager>().BossSpawned += bossSpawned;

        if (Player == null)
        {
            Debug.Log("Player not found");
            return;
            // TODO: disable
        }

        Debug.Log("Player : " + Player.name + " found \\^o^/");
    }

    private void FixedUpdate()
    {
        Vector3 currentLocation = transform.position;
        Vector3 targetLocation = Player.transform.position;
        
        if (followPlayer && !runAway)
        {
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, targetLocation, Speed * Time.deltaTime);
            Rigidbody2D.position = newPosition;
        }
        else if (runAway)
        {
            // TODO: if boss, don't run away
            
            // Calculate the opposite direction
            Vector3 oppositeDirection = (currentLocation - targetLocation).normalized;

            // Move away from the player
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, currentLocation + oppositeDirection, Speed * Time.deltaTime);
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
        while (true)
        {
            if (TouchingPlayer)
            {
                yield return new WaitForSeconds(waitTime);
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage);
                
                Debug.Log("touching player");
            }
            else
            {
                StopCoroutine("WaitAndPrint");
                //break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        followPlayer = true;
        TouchingPlayer = false;
    }

    private void OnDestroy()
    {
        GameManager.GetGameManager().GetSubsystem<ItemSpawner>().ItemSpawn(Player.WeaponPrefab,Player.HatPrefab,Player.ArmorPrefab,transform.position,transform.rotation);
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;

        FindObjectOfType<SpawnManager>().BossSpawned -= bossSpawned;
    }

    private void changeOpacity(float opacity)
    {
        Color spColor = SpriteRenderer.color;
        spColor.a = .5f;
        SpriteRenderer.color = spColor;
    }

    private void ToDeadLand()
    {
        followPlayer = false;
        changeOpacity(.5f);
        if (TouchingPlayer)
        {
            OnTriggerExit2D(Player.GetComponent<Collider2D>());
        }
    }

    private void ToLivingLand()
    {
        followPlayer = true;
        changeOpacity(1f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}