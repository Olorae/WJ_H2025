using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : ISubSystem
{
    private static SpawnManager SpawnSubsystemInstance;

    private static int nbTotalEnemy;
    private Camera mainCamera;

    public void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
    }

    public void SpawnEnemy()
    {
        // SceneView.currentDrawingSceneView.camera.pixelHeight
        /*
        if ( Vector2.Distance(a, b) < rangeMin && Vector2.Distance(a, b) > maxRange )
        {
        }
        */
        nbTotalEnemy++;
    }
}