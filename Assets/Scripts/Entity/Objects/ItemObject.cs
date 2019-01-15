using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Entity {

    public Item mItemData;

    public override void EntityInit()
    {
        base.EntityInit();
        body.mIsKinematic = false;

    }

    public void SetItem(Item data)
    {
        mItemData = data;
        GetComponent<SpriteRenderer>().sprite = data.sprite;
    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

    }

}
