using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Hat : Item
{
    protected override void Initialize()
    {
        name = "Hat test";
        type = "Hat";
        description = GetRandomDescription("Assets/EquipmentText/HatOf.txt");

        
        int objectRarity = Random.Range(1, 100);
        if (objectRarity < 90) // Normal item stat 
        {
            rarity = 0;
            madnessPerSecondReduce = Random.Range(0, 20);
            bossSpawnChanceReduction = Random.Range(0, 10);
            rarityColor = new Color(0, 145, 18);
        }
        else if (objectRarity <= 98 ) // Good item 
        {
            rarity = 1;
            madnessPerSecondReduce = Random.Range(20, 40);
            bossSpawnChanceReduction = Random.Range(10, 20);
            rarityColor = new Color(138, 65, 120);
        }
        else // Prefect item 
        {
            rarity = 2;
            madnessPerSecondReduce = 40;
            bossSpawnChanceReduction = 20;
            rarityColor = new Color(210, 141, 0);
        }
        

        GetComponent<SpriteRenderer>().color = rarityColor;
        GetComponent<SpriteRenderer>().sprite = HatSprite;

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
