using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBags : MonoBehaviour
{
    [SerializeField]
    private List<ThingAttribute> lootlist = new List<ThingAttribute>();
    public GameObject Prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetDroppingThing()
    {
        int random_num = Random.Range(1, 101); // 1-100
        List<ThingAttribute> possible_things = new List<ThingAttribute>();
        foreach(ThingAttribute thing in lootlist)
        {
            if(random_num<= thing.DropChance)
            {
                possible_things.Add(thing);
            }
        }
        if(possible_things.Count > 0)
        {
            ThingAttribute thing = possible_things[Random.Range(0,possible_things.Count)];
        }
    }
}
