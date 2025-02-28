using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private static GameManager GameManagerInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //get with this function to ensure that GameManagerInstance always exists only once
    public static GameManager GetGameManager()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = new GameManager();
        }
       
        return GameManagerInstance;
    }
    
    public T GetSubsystem<T>() where T : ISubSystem
    {
        return (T)typeof(T).GetMethod("GetSubSystem")?.Invoke(null,null);
    }
}
//this is an interface class which every subsystem should inherit from
public interface ISubSystem
{

    public static ISubSystem GetSubsystem()
    {
        return null;
    }

}