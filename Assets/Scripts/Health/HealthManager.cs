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
        healthAmount -= life;
        healthAmount = Mathf.Clamp(healthAmount, 0f, healthMax);
        healthBar.fillAmount = life / maxLife;
        
        this.GameObject().SetActive(true);
        Invoke("deactivateHealthBar", 5f);
    }

    private void deactivateHealthBar(){
        this.GameObject().SetActive(false);
    }

    private void Awake()
    {
        Invoke("deactivateHealthBar", .1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        owner = gameObject.GetComponent<EnemyCharacter>();
        healthMax = 100f;
        healthAmount = healthMax;
        
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
