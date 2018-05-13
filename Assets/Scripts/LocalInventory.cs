using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInventory : MonoBehaviour {
    Dictionary<ItemType, int> carrying = new Dictionary<ItemType, int>();
	// Use this for initialization
	void Start () {
        carrying.Add(ItemType.passport, 0);
        carrying.Add(ItemType.food, 0);
        carrying.Add(ItemType.distraction, 0);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        if (coll.gameObject.GetComponent<CollectableItem>() != null)
        {
            CollectableItem item = coll.gameObject.GetComponent<CollectableItem>();
            carrying[item.GetItemType()] += 1;
            Debug.Log("Received " + item.GetItemType());

            Destroy(coll.gameObject);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
