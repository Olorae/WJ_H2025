using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Image = UnityEngine.UI.Image;

public class ButtonBackGround : MonoBehaviour
{
    public int i;
    public Image button ;
    public TextMeshProUGUI text;
    private Image backgroundCurrent;
    public Sprite backgroundHover;
    public Sprite backgroundPressed;
    public Sprite backgroundNormal;
    private Color colorHover = new Color(0.7137255f, 0.8980393f, 0.8313726f, 1f);
    
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
        text.color = colorHover;
        backgroundCurrent.sprite = backgroundHover;
        
    }
    public void UnhoverButton()
    {
        text.color = Color.white;
        backgroundCurrent.sprite = backgroundNormal;
        
    }

    public void ClickedButton()
    {
        backgroundCurrent.sprite = backgroundPressed;
    }
}
