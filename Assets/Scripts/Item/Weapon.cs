using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Random = UnityEngine.Random;

public class Weapon : Item
{
    protected override void Initialize()
    {
        name = "Sword of";
        type = "Weapon";
        description = GetRandomDescription("Assets/EquipmentText/SwordOf.txt");
        
        int objectRarity = Random.Range(1, 100);
        if (objectRarity < 90) // Normal item stat 
        {
            attackDamage = Random.Range(0, 20);
            attackSpeed = Random.Range(0, 50);
        }
        else if (objectRarity <= 98 ) // Good item 
        {
            attackDamage = Random.Range(20, 40);
            attackSpeed = Random.Range(50, 100);
        }
        else // Prefect item 
        {
            attackDamage = 40;
            attackSpeed = 100;
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
