using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCharacter : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerController Player = null;
    public float Speed;
    public Rigidbody2D Rigidbody2D;
    public SpriteRenderer SpriteRenderer;
    public CircleCollider2D CircleCollider2D;

    private bool followPlayer;
    void Start()
    {
        // Initialisations
        Player = FindObjectOfType<PlayerController>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        followPlayer = true;
        
        if (Player == null)
        {
            Debug.Log("Player not found");
            return;
            // TODO: disable
        }
        
        Debug.Log("Player : "+ Player.name +" found \\^o^/");
    }

    private void FixedUpdate()
    {
       if (followPlayer == true)
        {
            Vector3 currentLocation = transform.position;
            Vector3 targetLocation = Player.transform.position;
            Vector3 newPosition = Vector3.MoveTowards(currentLocation, targetLocation, Speed * Time.deltaTime);
            Rigidbody2D.position = newPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter");
        followPlayer = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit");
        followPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}