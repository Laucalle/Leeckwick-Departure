using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalInventory : MonoBehaviour {
    Dictionary<ItemType, int> carrying = new Dictionary<ItemType, int>();
    public Text display;
	// Use this for initialization
	void Start () {
        carrying.Add(ItemType.passport, 0);
        carrying.Add(ItemType.food, 0);
        carrying.Add(ItemType.distraction, 0);
        UpdateDisplay();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        if (coll.gameObject.GetComponent<CollectableItem>() != null)
        {
            CollectableItem item = coll.gameObject.GetComponent<CollectableItem>();
            carrying[item.GetItemType()] += 1;

            Debug.Log("Received " + item.GetItemType());

            Destroy(coll.gameObject);
            UpdateDisplay();
        }

    }

    public int GetFood()
    {
        return carrying[ItemType.food];
    }

    public int GetPassports()
    {
        return carrying[ItemType.passport];
    }


    void UpdateDisplay()
    {
        display.text = "Food: " + carrying[ItemType.food] + "\n Passports: " + carrying[ItemType.passport] + "\n Distractions: " + carrying[ItemType.distraction];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
