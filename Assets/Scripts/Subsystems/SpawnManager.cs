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
        //TopWall.transform.position = new Vector3(TopWall.transform.position.x, upperCorner.y - offsetY, 0f);
        //BottomWall.transform.position = new Vector3(BottomWall.transform.position.x, lowerCorner.y + offsetY*1.5f, 0f);
        
        //LeftWall.transform.localScale = new Vector3(LeftWall.transform.localScale.x, heightCamera*2, 1f);
        //RightWall.transform.localScale = new Vector3(RightWall.transform.localScale.x, heightCamera*2, 1f);
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

        lowerCorner = new Vector3(CameraPosition.x - widthCamera - offsetX, CameraPosition.y - heightCamera - offsetX, 0);
        upperCorner = new Vector3(CameraPosition.x + widthCamera + offsetY, CameraPosition.y + heightCamera + offsetY, 0);
    }

    public void SpawnEnemy()
    {
        if (!bossIaAlive)
        {
            setCorners();

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

            Debug.Log("Spawn Position = " + SpawnPosition.x + ", " + SpawnPosition.y);

            // Spawn Enemy
            float folie = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().insanity;
            float chanceToSpawnFake = Random.Range(1, 100);
            float chanceToSpawnBoss = Random.Range(1, 100);

            if (folie >= InsanityToSpawnBoss && chanceToSpawnBoss <= (folie - InsanityToSpawnBoss) * 2)
            {
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand.Invoke();
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = true;
                FindObjectOfType<PortalScript>().GameObject().SetActive(false);
                bossIaAlive = true;
                // Spawn Boss 
                BossSpawned.Invoke();
                
               

                // TODO: check if stop works
                StopCoroutine(coroutine);
                Clone = Instantiate(EnemyBoss, SpawnPosition, Quaternion.identity);
            }
            else if (chanceToSpawnFake <= folie / 2)
            {
                // Spawn faux
                Clone = Instantiate(EnemyFake, SpawnPosition, Quaternion.identity);
            }
            else
            {
                // Spawn vrai
                Clone = Instantiate(EnemyReal, SpawnPosition, Quaternion.identity);
            }

            nbTotalEnemy++;
        }
    }

    public void StartSpawning()
    {
        SpawnEnemy();
        StartCoroutine(coroutine);
    }
}