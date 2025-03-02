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
        
        healthBar.fillAmount = life / maxLife;
    }
    
    // Start is called before the first frame update
    void Start()
    {

        owner = LinkedPlayer.GetComponent<EnemyCharacter>();
        owner.Life = owner.MaxLife;
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
