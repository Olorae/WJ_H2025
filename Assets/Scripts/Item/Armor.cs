using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Armor : Item
{ 
    protected override void Initialize()
    {
        name = "Armor test";
        type = "Armor";
        description = GetRandomDescription("Assets/EquipmentText/ArmorOf.txt");
        
        int objectRarity = Random.Range(1, 100);
        if (objectRarity < 90) // Normal item stat 
        {
            rarity = 0;
            mouvementSpeed = Random.Range(0, 5);
            madnessDefense = Random.Range(0, 25);
            rarityColor = new Color(0, 145, 18);
        }
        else if (objectRarity <= 98 ) // Good item 
        {
            rarity = 1;
            mouvementSpeed = Random.Range(5, 10);
            madnessDefense = Random.Range(25, 50);
            rarityColor = new Color(138, 65, 120);
        }
        else // Prefect item 
        {
            rarity = 2;
            mouvementSpeed = 10;
            madnessDefense = 50;
            rarityColor = new Color(210, 141, 0);
        }
        GetComponent<SpriteRenderer>().color = rarityColor;
        GetComponent<SpriteRenderer>().sprite = ShirtSprite;
       
    }

    private string GetRandomDescription(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                return lines[Random.Range(0, lines.Length)];
            }
        }

        return "No description available";
    }
}
