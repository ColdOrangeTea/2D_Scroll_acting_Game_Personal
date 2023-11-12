using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField]
    private List<Loot> lootlist = new List<Loot>();
    public GameObject droppedThingPrefab;

    Loot GetDroppingThing()
    {
        int random_num = Random.Range(1, 101); // 1-100
        List<Loot> possible_things = new List<Loot>();
        foreach (Loot thing in lootlist)
        {
            if (random_num <= thing.attribute.DropChance)
            {
                possible_things.Add(thing);
            }
        }
        if (possible_things.Count > 0)
        {
            Loot droppedThing = possible_things[Random.Range(0, possible_things.Count)];
            return droppedThing;
        }
        Debug.Log("No loot Dropped.");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedThing = GetDroppingThing();
        if (droppedThing != null)
        {
            GameObject lootGameObject = Instantiate(droppedThingPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedThing.attribute.ThingSprite;
            float dropForce = 10f;
            // Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            Vector2 dropDirection = Vector2.down;

            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }
}
