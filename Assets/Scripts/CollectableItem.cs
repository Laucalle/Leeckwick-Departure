using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { passport, food, distraction }

public class CollectableItem : MonoBehaviour {
    [SerializeField]
    ItemType myType;


    // Use this for initialization
    public ItemType GetItemType()
    {
        return myType;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
