using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsanityBar : MonoBehaviour
{
    public GameObject thisUI;

    
    public Sprite BossBar;

    public Image background;

    public Image fill;

    public Image Skull;
    // Start is called before the first frame update
    void Start()
    {
        thisUI.SetActive(false);
        FindObjectOfType<SpawnManager>().BossSpawned += bossSpawned;
    }

    // Update is called once per frame
    void Update()
    {
        thisUI.GetComponent<Slider>().value = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().insanity;
        thisUI.GetComponent<Slider>().maxValue = GameManager.GetGameManager().GetSubsystem<DataSubsystem>().maxInsanity;
        
    }

    public void bossSpawned()
    {
        
       background.color = Color.gray;
       fill.color = Color.red;
       Skull.color = Color.red;
    }
    public void UpdateValue(float newValue)
    {
        
    }
}
