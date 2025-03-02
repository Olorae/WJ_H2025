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
        if (objectRarity < 60) // Normal item stat 
        {
            rarity = 0;
            madnessPerSecondReduce = Random.Range(0, 20);
            bossSpawnChanceReduction = Random.Range(0, 10);
            GetComponent<SpriteRenderer>().color = rarityColor= new Color(167, 149, 143);
        }
        else if (objectRarity <= 90 ) // Good item 
        {
            rarity = 1;
            madnessPerSecondReduce = Random.Range(20, 40);
            bossSpawnChanceReduction = Random.Range(10, 20);
            GetComponent<SpriteRenderer>().color = rarityColor=new Color(141, 200, 200);
        }
        else // Prefect item 
        {
            rarity = 2;
            madnessPerSecondReduce = 40;
            bossSpawnChanceReduction = 20;
            GetComponent<SpriteRenderer>().color = rarityColor=new Color(168, 141, 200);
        }
        
        
        GetComponent<SpriteRenderer>().sprite = HatSprite;
        //GetComponent<SpriteRenderer>().color = rarityColor;
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
