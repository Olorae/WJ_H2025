using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerController Player = null;
    private float Speed = 2.0f;
    public Rigidbody2D Rigidbody2D;
    void Start()
    {
        // get Player
        Player = FindObjectOfType<PlayerController>();
        if (Player == null)
        {
            Debug.LogError("Player not found");
            Debug.Log("Player not found");
            return;
            // tODO: disabled
        }
        Debug.Log("Player : "+ Player.name +" found \\^o^/");
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Spawn()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 currentLocation = transform.position;
        Vector3 targetLocation = Player.transform.position;
        Vector3 newPosition = Vector3.MoveTowards(currentLocation, targetLocation, Speed * Time.deltaTime);
        Rigidbody2D.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
    }
}