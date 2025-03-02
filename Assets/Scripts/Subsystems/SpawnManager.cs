using System;
using System.Collections;
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

    private IEnumerator coroutine;

    public void Start()
    {
        mainCamera = FindObjectOfType<Camera>();

        // Start function WaitAndPrint as a coroutine.

        coroutine = WaitAndPrint(5.0f);
        //StartCoroutine(coroutine);
        
        bossIaAlive = false;
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (!bossIaAlive)
        {
            //SceneView sceneView = mainCamera.GetComponent<SceneView>();
            float heightCamera = mainCamera.orthographicSize;
            float widthCamera = heightCamera * mainCamera.aspect;

            Vector3 CameraPosition = mainCamera.transform.position;

            Vector3 lowerCorner = new Vector3(CameraPosition.x - widthCamera, CameraPosition.y - heightCamera, 0);
            Vector3 upperCorner = new Vector3(CameraPosition.x + widthCamera, CameraPosition.y + heightCamera, 0);

            float randomX = Random.Range(lowerCorner.x - offset, upperCorner.x + offset);
            float randomY;
            // if X is in the screen
            if (randomX > lowerCorner.x && randomX < upperCorner.x)
            {
                randomY = (Random.value > 0.5)
                    ? Random.Range(lowerCorner.y - offset, lowerCorner.y)
                    : Random.Range(upperCorner.y + offset, upperCorner.y);
            }
            else
            {
                // X is out of the screen
                randomY = Random.Range(lowerCorner.y - offset, upperCorner.y + offset);
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