using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private CircleCollider2D portalHitBox;
    
    // Start is called before the first frame update
    private void Awake()
    {
        portalHitBox = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("enter portal");
        if (other.GameObject().tag.Equals("Player"))
        {
            if (GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand)
            {
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToDeadLand.Invoke();
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = false;

            }
            else
            {
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand.Invoke();
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = true;
            }
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
