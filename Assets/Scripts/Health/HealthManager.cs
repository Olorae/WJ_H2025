using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public GameObject thisUi;
    public Image healthBar;
    public float healthAmount;
    public float healthMax;

    private EnemyCharacter owner;

    public void takeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0f, healthMax);
        healthBar.fillAmount = healthAmount / healthMax;
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
        //TODO: remplacer par owner.MaxLife
        healthBar.fillAmount = owner.Life / 100;
    }
    
}
