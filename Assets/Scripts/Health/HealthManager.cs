using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public GameObject thisUi;
    public Image healthBar;
    public float healthAmount;
    public float healthMax;
    public GameObject LinkedPlayer;

    private EnemyCharacter owner;

    public void takeDamage(float life, float maxLife)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0f, healthMax);
        healthBar.fillAmount = healthAmount / healthMax;
        
        gameObject.SetActive(true);
        Invoke("deactivateHealthBar", 5f);
    }

    private void deactivateHealthBar(){
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        owner = gameObject.GetComponent<EnemyCharacter>();
        healthMax = 100f;
        healthAmount = healthMax;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    
    void Update()
    {
       /* if (owner)
        {
            healthBar.fillAmount = owner.Life / owner.MaxLife;
        }*/
    }
    
}
