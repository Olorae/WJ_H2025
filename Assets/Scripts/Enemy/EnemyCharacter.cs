using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerController Player = null;
    public float Speed = 2.0f;
    public Rigidbody2D Rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    public CircleCollider2D CircleCollider2D;
    void Start()
    {
        // get Player
        Player = FindObjectOfType<PlayerController>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        
        if (Player == null)
        {
            Debug.Log("Player not found");
            return;
            // tODO: disable
        }
        
        Debug.Log("Player : "+ Player.name +" found \\^o^/");
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