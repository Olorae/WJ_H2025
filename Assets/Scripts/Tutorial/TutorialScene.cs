using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : MonoBehaviour
{
    public EnemyCharacter Boss;
    public EnemyCharacter RealEnemy;
    public EnemyCharacter FakeEnemy;
    public PlayerController Player;
    
    void Start()
    {
    }

    public void deletePlayer()
    {
        Destroy(Player.gameObject);
        Destroy(Player);
        
        Destroy(RealEnemy.gameObject);
        Destroy(RealEnemy);
        
        Destroy(FakeEnemy.gameObject);
        Destroy(FakeEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
