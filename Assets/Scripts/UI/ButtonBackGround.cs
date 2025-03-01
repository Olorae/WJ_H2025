using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ButtonBackGround : MonoBehaviour
{
    public int i;
    public Image button ;
    private Image backgroundCurrent;
    public Sprite backgroundHover;
    public Sprite backgroundPressed;
    public Sprite backgroundNormal;
    
    // Start is called before the first frame update
    void Start()
    {
        if (button == null)
        {
            Debug.LogError("buttonGameObject n'est pas assigné dans l'Inspector !");
            return;
        }

        backgroundCurrent = button.GetComponent<Image>();

        if (backgroundCurrent == null)
        {
            Debug.LogError("L'objet assigné à buttonGameObject ne contient pas de composant Image !");
            return;
        }
        backgroundCurrent = button;
        backgroundCurrent.sprite = backgroundNormal; // Initialisation avec l'image normale    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HoverButton()
    {
        backgroundCurrent.sprite = backgroundHover;
    }
    public void UnhoverButton()
    {
        backgroundCurrent.sprite = backgroundNormal;
        
    }

    public void ClickedButton()
    {
        backgroundCurrent.sprite = backgroundPressed;
    }
}
