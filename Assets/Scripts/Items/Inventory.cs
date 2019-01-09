using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Entity mEntity;
    public List<Item> items;

    private void Start()
    {
        mEntity = GetComponent<Entity>();
        items = new List<Item>();
    }



}
