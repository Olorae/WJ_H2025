using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private CircleCollider2D portalHitBox;
    public Sprite DeadLand;
    public Sprite LivingLand;
    
    // Start is called before the first frame update
    private void Awake()
    {
        portalHitBox = GetComponent<CircleCollider2D>();
        this.GameObject().SetActive(false);
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
                GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().SetMusic( GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().DeadMusic);
                GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX( GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PortailSFX);
                GetComponent<SpriteRenderer>().sprite = LivingLand;

            }
            else
            {
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().ToLivingLand.Invoke();
                GameManager.GetGameManager().GetSubsystem<DimensionManager>().inLivingLand = true;
                GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().SetMusic( GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().LivingMusic);
                GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PlaySFX( GameManager.GetGameManager().GetSubsystem<SoundPlayerSubsystem>().PortailSFX);
                GetComponent<SpriteRenderer>().sprite = DeadLand;
            }
        }
        
    }

    void Start()
    {
        Camera mainCamera = FindObjectOfType<Camera>(); //GameObject.
        float heightCamera = mainCamera.orthographicSize;
        float widthCamera = heightCamera * mainCamera.aspect;
        
        Vector3 CameraPosition = mainCamera.transform.position;
        Vector3 upperCorner = new Vector3(CameraPosition.x + widthCamera - (GetComponent<SpriteRenderer>().bounds.size.x/2) , CameraPosition.y + heightCamera - (GetComponent<SpriteRenderer>().bounds.size.y/2) , 0);
        
        transform.position = upperCorner;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
