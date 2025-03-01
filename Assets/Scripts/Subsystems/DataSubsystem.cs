using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//example class on how to use the plugin
public class DataSubsystem : ISubSystem
{
    private static DataSubsystem DataSubsystemInstance;

    public float money;
    public float nbKill;
    public float insanity;
    public float maxInsanity;
    private bool StopCoroutine;

    public DataSubsystem()
    {
        insanity = 50;
        maxInsanity = 120;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;
        
    }
    
    private IEnumerator coroutine;
    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (!StopCoroutine) {
        yield return new WaitForSeconds(waitTime);
        GainInsanity(1);
        }
    }

    public void GainInsanity(float amountGained)
    {
        insanity += amountGained;
    }

    private void ToLivingLand()
    {
        
    }

    private void ToDeadLand()
    {
        coroutine = WaitAndPrint(5.0f);
       // StartCoroutine(coroutine);
    }

    public static ISubSystem GetSubSystem()
    {

        if (DataSubsystemInstance == null) {
            DataSubsystemInstance = new DataSubsystem();
        }

        return DataSubsystemInstance;
    }

    public void EnemyKilled()
    {
        nbKill++;
    }
}
