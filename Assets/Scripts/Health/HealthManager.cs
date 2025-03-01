using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    
    public Image healthBar;
    public float healthAmount;
    public float healthMax;

    public void takeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0f, healthMax);
        healthBar.fillAmount = healthAmount / healthMax;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        healthMax = 100f;
        healthAmount = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            takeDamage();
        }
    }
}
