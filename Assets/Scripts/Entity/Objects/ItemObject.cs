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
        if(mItemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public override void EntityUpdate()
    {

        base.EntityUpdate();

    }

}
