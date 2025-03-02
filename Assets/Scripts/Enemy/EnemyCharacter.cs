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
    public float InitialSpeed;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    private CapsuleCollider2D CapsuleCollider2D;
    public bool RealEnemy;
    public bool followPlayer;
    private bool TouchingPlayer;
    public float Life;
    public float MaxLife;
    public float Damage;
    private HealthManager healthManager;
    public float pushForce; // = 500f;
    private bool bossIsComing;
    public Animator animator;
    private float previousLife = 0f;
    
    private IEnumerator coroutine;

    private bool runAway;

    private void bossSpawned()
    {
        runAway = true;
        bossIsComing = true;
    }

    void Start()
    {
        // Initialisations
        Player = FindObjectOfType<PlayerController>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        healthManager = FindObjectOfType<HealthManager>();
        animator = GetComponent<Animator>();
        followPlayer = true;
        TouchingPlayer = false;
        runAway = false;
        InitialSpeed = Speed;
        bossIsComing = false;
        float facteurDeCroissanceVieEnnemie = 10f;
        previousLife = MaxLife;
        Life = previousLife + (GameManager.GetGameManager().GetSubsystem<DataSubsystem>().nbKill * facteurDeCroissanceVieEnnemie); // Peut etre faire un fontion log pour plus calme au début
                                                                                                                              // et plus intense a la fin 
        Debug.Log(GameManager.GetGameManager().GetSubsystem<DataSubsystem>().nbKill);
        
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;

        FindObjectOfType<SpawnManager>().BossSpawned += bossSpawned;

        followPlayer = GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand;
        if (Player == null)
        {
           // Debug.Log("Player not found");
            return;
            // TODO: disable
        }

        //Debug.Log("Player : " + Player.name + " found \\^o^/");
    }

    private void FixedUpdate()
    {
        Vector3 currentLocation = transform.position;
        Vector3 targetLocation = Player.transform.position;

        if ((followPlayer && !runAway) || CompareTag("Boss"))
        {
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, targetLocation, Speed * Time.deltaTime);
            rigidbody2D.position = newPosition;
        }
        else if (runAway)
        {
            if (bossIsComing)
            {
                // TODO: something pour que les ennemis regarde derrière eux en partant
            }

            // Calculate the opposite direction
            Vector3 oppositeDirection = (currentLocation - targetLocation).normalized;

            // Move away from the player
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, currentLocation + oppositeDirection,
                Speed * Time.deltaTime);
            rigidbody2D.position = newPosition;
        }

        Rotate(currentLocation, targetLocation);
    }

    private void Rotate(Vector3 currentLocation, Vector3 targetLocation)
    {
        if ((targetLocation - currentLocation).x >= 0)
        {
            transform.rotation = new Quaternion(0f, 0f, transform.rotation.z, transform.rotation.w);
            healthManager.transform.rotation = new Quaternion(0f, 0f, healthManager.transform.rotation.z,
                healthManager.transform.rotation.w);
        }
        else
        {
            transform.rotation = new Quaternion(0f, 180f, transform.rotation.z, transform.rotation.w);
            healthManager.transform.rotation = new Quaternion(0f, 180f, healthManager.transform.rotation.z,
                healthManager.transform.rotation.w);
        }
    }

    private void PushedBackOver()
    {
        runAway = false;
        Speed = InitialSpeed;
    }

    public bool Attacked(float damage)
    {
        if (RealEnemy)
        {
            //Debug.Log("Real enemy attacked");
            
            Life -= damage;
            healthManager.takeDamage(Life,MaxLife);
            animator.SetTrigger("Hit");
            runAway = true;
            Speed = pushForce;
            Invoke("PushedBackOver", .05f);
            /*
            Vector2 force = -(Player.transform.position - transform.position) * 1f;
            rigidbody2D.AddForce(force);
            */
        }
        else
        {
            Life = 0;
            healthManager.takeDamage(Life,MaxLife);
            GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,false);
        }

        //Debug.Log("Life = " + Life);
        
        return Life <= 0;
    }

    private void damageInflicted()
    {
        if (GameManager.GetGameManager().GetSubsystem<DataSubsystem>().insanity <= 0 && CompareTag("Boss"))
        {
            Debug.Log("Player is dead");

            // TODO: change scene
        }
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
                
                InvokeRepeating("DamagePlayer", .1f, 1f); // Démarre après 1s, se répète toutes les 1s

                /*
                if (RealEnemy)
                {
                    Player.OnHit((CompareTag("Boss")) ? -Damage : Damage);

                    damageInflicted();
                    //GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,true);
                    coroutine = WaitAndPrint(5.0f);
                    //StartCoroutine(coroutine);
                }
                else
                {
                    GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(-Damage / 2, false);
                    Destroy(this.GameObject());
                    Destroy(this);
                }
                */
            }
        }
    }

    private void DamagePlayer()
    {
        if (!TouchingPlayer)
        {
            CancelInvoke("DamagePlayer");
        }
        else
        {
            if (RealEnemy)
            {
                Player.OnHit((CompareTag("Boss")) ? -Damage : Damage);
                damageInflicted();
                
                //GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage,true);
            }
            else
            {
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(-Damage / 2, false);
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
                GameManager.GetGameManager().GetSubsystem<DataSubsystem>().GainInsanity(Damage, true);

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
            if (Life > 0)
            {
                followPlayer = true;
                TouchingPlayer = false;
            }
        }
    }

    private void OnDestroy()
    {
        // FindObjectOfType<SpawnManager>().BossSpawned -= bossSpawned;

        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;
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

    public void Die()
    {
        if (RealEnemy)
        {
            GameManager.GetGameManager().GetSubsystem<ItemSpawner>().ItemSpawn(Player.WeaponPrefab, Player.HatPrefab,
                Player.ArmorPrefab, transform.position, transform.rotation);
        }

        Destroy(this.GameObject());
        Destroy(this);
        GameManager.GetGameManager().GetSubsystem<DataSubsystem>().EnemyKilled();
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}