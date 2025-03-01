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
    private CapsuleCollider2D CapsuleCollider2D;
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
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        followPlayer = true;
        TouchingPlayer = false;
        runAway = false;
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;

        FindObjectOfType<SpawnManager>().BossSpawned += bossSpawned;

        followPlayer = GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand;
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
        
        if ((followPlayer && !runAway) || CompareTag("Boss"))
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
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,false);
        }

        Debug.Log("Life = " + Life);
        if (Life <= 0)
        {
            if (RealEnemy)
            {
                GameManager.GetGameManager().GetSubsystem<ItemSpawner>().ItemSpawn(Player.WeaponPrefab,Player.HatPrefab,Player.ArmorPrefab,transform.position,transform.rotation);
            }
            
        }
        return Life <= 0;
    }

    // Enemy touched player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand)
        {
            if (other.CompareTag("Player"))
            {
                followPlayer = false;
                TouchingPlayer = true;

                if (RealEnemy)
                {
                    Player.OnHit(Damage);
                    //GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,true);
                    coroutine = WaitAndPrint(5.0f);
                    //StartCoroutine(coroutine);
                }
                else
                {
                    GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(-Damage/2,false);
                    Destroy(this.GameObject());
                    Destroy(this);
                }
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
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,true);
                
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
        if (GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand)
        {
            followPlayer = true;
            TouchingPlayer = false;
        }
        
    }

    private void OnDestroy()
    {
        
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;

        FindObjectOfType<SpawnManager>().BossSpawned -= bossSpawned;
    }

    private void changeOpacity(float opacity)
    {
        Color spColor = SpriteRenderer.color;
        spColor.a = opacity;
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