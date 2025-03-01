using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public IAPlayer playerInput;
    public float baseSpeed = 5f;
    public Rigidbody2D rb;

    private InputAction move;

    private void Awake()
    {
        playerInput = new IAPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        move = playerInput.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveValue.x * baseSpeed, moveValue.y * baseSpeed);

    }
}
