using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : ISubSystem
{
    private static ItemSpawner ItemSpawnerInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static ISubSystem GetSubSystem()
    {

        if (ItemSpawnerInstance == null) {
            ItemSpawnerInstance = new ItemSpawner();
        }

        return ItemSpawnerInstance;
    }
    public GameObject ItemSpawn(GameObject WeaponPrefab,GameObject HatPrefab,GameObject ArmorPrefab,Vector3 position, Quaternion rotation)
    {
        Item itemToSpawn;
        float chance = Random.Range(0f, 1f);
       
        if(0.5f < chance && chance <= 0.667f)
        {
            // Spawn Casque
            GameObject casqueSpawned = GameObject.Instantiate(HatPrefab, position, rotation);
            return casqueSpawned;
        }
        else if (0.667f < chance && chance <= 0.834f)
        {
            // spawn Armure
            GameObject armorSpawned = GameObject.Instantiate(ArmorPrefab, position, rotation);
            return armorSpawned;
        }
        else if (0.834f < chance && chance <= 1f)
        {
            // spawn Weapon 
            GameObject weaponSpawned = GameObject.Instantiate(WeaponPrefab, position, rotation);
            return weaponSpawned;
        }
        else
        { 
            return null;
        }
       
    }
}
