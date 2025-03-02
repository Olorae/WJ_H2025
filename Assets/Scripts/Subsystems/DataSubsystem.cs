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
        insanity = 0;
        maxInsanity = 120;
    }

    public void GainInsanity(float amountGained,bool hit)
    {
        
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();
        if (amountGained < 0)
        {
            insanity += amountGained;
        }
        else
        {
            if (hit)
            {
                insanity += Math.Max(amountGained - player.GetDefense(), 0);
            }
            else
            {
                insanity += Math.Max(amountGained - player.GetDefense(), 0);
            }
            
            
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
        Debug.Log("nbKil : " + nbKill);
    }
}
