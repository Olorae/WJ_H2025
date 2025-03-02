using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : Item
{
    protected override void Initialize()
    {
        name = "Sword of";
        type = "Weapon";
        description = GetRandomDescription("Assets/EquipmentText/SwordOf.txt");
        if (firstItem)
        {
            attackDamage = 15;
            rarityColor = new Color(241,228,149,1);
        }
        else
        {
            int objectRarity = Random.Range(1, 100);
            if (objectRarity < 60) // Normal item stat 
            {
                rarity = 0;
                attackDamage = Random.Range(15, 25);
                attackRange = Random.Range(0, 50);
                rarityColor = new Color(0, 145, 18);
            }
            else if (objectRarity <= 90 ) // Good item 
            {
                rarity = 1;
                attackDamage = Random.Range(25, 45);
                attackRange = Random.Range(50, 100);
                rarityColor = new Color(141, 200, 200);
            }
            else // Prefect item 
            {
                rarity = 2;
                attackDamage = 50;
                attackRange = 100;
                rarityColor = new Color(210, 141, 0);
            } 
            
            GetComponent<SpriteRenderer>().sprite = WeaponSprite;
            GetComponent<SpriteRenderer>().color = rarityColor;
        }

       




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
