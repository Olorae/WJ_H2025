using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand += ToDeadLand;
        GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand += ToLivingLand;
    }

    private void ToDeadLand()
    {
        GetComponent<Image>().color = Color.gray;
    }
    private void ToLivingLand()
    {
        GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchDimension()
    {
        
    }
}
