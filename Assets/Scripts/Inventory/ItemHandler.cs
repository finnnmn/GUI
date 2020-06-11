using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public int ID;
    public bool collectable = true;

    public void PickUp()
    {
        if (collectable)
        {
            Inventory.AddItem(ItemData.CreateItem(ID, 1));
            GameObject.Destroy(this.gameObject);
        }
    }
}
