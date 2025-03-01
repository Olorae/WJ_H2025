using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsanityBar : MonoBehaviour
{
    public GameObject thisUI;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        thisUI.GetComponent<Slider>().value = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().insanity;
        thisUI.GetComponent<Slider>().maxValue = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().maxInsanity;
    }

    public void UpdateValue(float newValue)
    {
        
    }
}
