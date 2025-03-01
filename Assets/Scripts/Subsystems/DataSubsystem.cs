using System;
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

    public DataSubsystem()
    {
        insanity = 50;
        maxInsanity = 120;
       

    }

    public void GainInsanity(float amountGained)
    {Debug.Log("gain insanity");
        if (amountGained < 0)
        {
            insanity += amountGained;
        }
        else
        {
            Debug.Log("defense" +GameObject.FindObjectOfType<PlayerController>().GetDefense());
            insanity += Math.Max(amountGained - GameObject.FindObjectOfType<PlayerController>().GetDefense(), 0);
        }
        
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
