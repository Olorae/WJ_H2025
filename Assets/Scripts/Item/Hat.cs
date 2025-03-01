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
            madnessPerSecondReduce = Random.Range(0, 20);
            bossSpawnChanceReduction = Random.Range(0, 10);
        }
        else if (objectRarity <= 98 ) // Good item 
        {
            madnessPerSecondReduce = Random.Range(20, 40);
            bossSpawnChanceReduction = Random.Range(10, 20);
        }
        else // Prefect item 
        {
            madnessPerSecondReduce = 40;
            bossSpawnChanceReduction = 20;
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
