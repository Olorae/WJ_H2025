using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager SpawnSubsystemInstance;

    public bool tutorialScene;

    private static int nbTotalEnemy;
    private Camera mainCamera;
    public EnemyCharacter EnemyFake;
    public EnemyCharacter EnemyReal;
    public EnemyCharacter EnemyBoss;
    private EnemyCharacter Clone;
    private Vector3 SpawnPosition = new Vector3(0, 0, 0);
    public float offset = 10f;
    public float InsanityToSpawnBoss = 50;
    public Action BossSpawned;
    public bool bossIaAlive;

    private float heightCamera;
    private float widthCamera;
    private Vector3 CameraPosition;
    private float offsetX;
    private float offsetY;
    private Vector3 lowerCorner;
    private Vector3 upperCorner;

    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject TopWall;
    public GameObject BottomWall;

    private IEnumerator coroutine;

    public void Start()
    {
        mainCamera = FindObjectOfType<Camera>();

        // Start function WaitAndPrint as a coroutine.

        coroutine = WaitAndPrint(5.0f);
        //StartCoroutine(coroutine);
        
        bossIaAlive = false;

        setCorners();
        
        LeftWall.transform.position = new Vector3(lowerCorner.x + offsetX, LeftWall.transform.position.y, 0f);
        RightWall.transform.position = new Vector3(upperCorner.x, RightWall.transform.position.y, 0f);
        
        TopWall.transform.localScale = new Vector3(widthCamera*2, TopWall.transform.localScale.y, 1f);
        BottomWall.transform.localScale = new Vector3(widthCamera*2, BottomWall.transform.localScale.y, 1f);
        
        //TopWall.transform.rotation = quaternion.identity;
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();
        }
    }

    private void setCorners()
    {
        heightCamera = mainCamera.orthographicSize;
        widthCamera = heightCamera * mainCamera.aspect;

        CameraPosition = mainCamera.transform.position;

        offsetX = widthCamera / offset;
        offsetY = heightCamera / offset;

        lowerCorner = new Vector3(CameraPosition.x - widthCamera - offsetX, CameraPosition.y - heightCamera - offsetY, 0);
        upperCorner = new Vector3(CameraPosition.x + widthCamera + offsetX, CameraPosition.y + heightCamera + offsetY, 0);
    }

    private void setSpawnPosition()
    {
        float randomX = Random.Range(lowerCorner.x - offsetX, upperCorner.x + offsetX);
        float randomY;
        // if X is in the screen
        if (randomX > lowerCorner.x && randomX < upperCorner.x)
        {
            randomY = (Random.value > 0.5)
                ? Random.Range(lowerCorner.y - offsetY, lowerCorner.y)
                : Random.Range(upperCorner.y + offsetY, upperCorner.y);
        }
        else
        {
            // X is out of the screen
            randomY = Random.Range(lowerCorner.y - offsetY, upperCorner.y + offsetY);
        }
        SpawnPosition = new Vector3(randomX, randomY, 0);
    }

    public void SpawnEnemy()
    {
        if (!bossIaAlive)
        {
            setCorners();
            setSpawnPosition();
            
            // Spawn Enemy
            float folie = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().insanity;
            float chanceToSpawnFake = Mathf.Clamp((50f - (8f / 9f) * folie), 0f, 50f);
            float chanceToSpawnBoss = Random.Range(1, 100);
            float reduceChanceSpawn = 0;

            if (FindObjectOfType<PlayerController>().hat != null )
            {
                reduceChanceSpawn = FindObjectOfType<PlayerController>().hat.bossSpawnChanceReduction;
                Debug.Log("REduce chance spawn : " + reduceChanceSpawn);
            }
            /*
            else if (FindObjectOfType<PlayerController>().weapon != null)
            {
                reduceChanceSpawn = FindObjectOfType<PlayerController>().weapon.bossSpawnChanceReduction;
                Debug.Log("REduce chance spawn : " + reduceChanceSpawn);
            }
            */
            
            // Spawn Boss
            if (folie >= InsanityToSpawnBoss &&
                chanceToSpawnBoss + reduceChanceSpawn <= (folie - InsanityToSpawnBoss) * 2)
            {
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand.Invoke();
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = true;
                FindObjectOfType<PortalScript>().GameObject().SetActive(false);
                bossIaAlive = true;
                BossSpawned.Invoke();
                GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().SetMusic( GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().BossMusic);
                
               

                // TODO: check if stop works
                StopCoroutine(coroutine);
                Clone = Instantiate(EnemyBoss, SpawnPosition, Quaternion.identity);
            }
            else if (nbTotalEnemy > chanceToSpawnFake)
            {
                // Spawn faux
                Clone = Instantiate(EnemyFake, SpawnPosition, Quaternion.identity);
                nbTotalEnemy = 0;
            }
            else
            {
                // Spawn vrai
                Clone = Instantiate(EnemyReal, SpawnPosition, Quaternion.identity);
                nbTotalEnemy++;
            }
        }
    }

    public void StartSpawning()
    {
        if (!tutorialScene)
        {
            SpawnEnemy();
            StartCoroutine(coroutine);
        }
    }
}