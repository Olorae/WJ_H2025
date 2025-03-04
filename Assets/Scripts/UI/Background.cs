using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Sprite livingBackground;
    
    
    private int count = 0;

    public Sprite deadBackground;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;
    }

    private void ToDeadLand()
    {
        if (deadBackground != null && GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = deadBackground;
        }
        
    }
    private void ToLivingLand()
    {
        GetComponent<Image>().sprite = livingBackground;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchDimension()
    {
        
    }

    private void OnDisable()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;
    }
    private void OnDestroy()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand -= ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand -= ToLivingLand;
    }
}
