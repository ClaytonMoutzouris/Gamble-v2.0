using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoDisplay : MonoBehaviour
{
    public List<AmmoIconObject> ammoObjects;
    AmmoType ammoType;
    public GridLayoutGroup layout;
    public List<Sprite> ammoSprites;
    public Sprite currentIcon;
    public AmmoIconObject prefab;
    public FuelDisplay fuelDisplay;

    public void SetAmmoType(AmmoType type)
    {
        ammoType = type;
        switch (ammoType)
        {
            case AmmoType.Pistol:
                layout.cellSize = new Vector2(5, 5);
                layout.spacing = new Vector2(0,0);
                currentIcon = ammoSprites[0];
                fuelDisplay.gameObject.SetActive(false);

                break;
            case AmmoType.Launcher:
                currentIcon = ammoSprites[1];
                layout.cellSize = new Vector2(5, 5);
                fuelDisplay.gameObject.SetActive(false);
                layout.spacing = new Vector2(0, 0);

                break;
            case AmmoType.Automatic:
                layout.cellSize = new Vector2(1, 5);
                layout.spacing = new Vector2(1, 0);

                currentIcon = ammoSprites[2];
                fuelDisplay.gameObject.SetActive(false);

                break;
            case AmmoType.Thrower:
                currentIcon = null;
                fuelDisplay.gameObject.SetActive(true);

                break;
            case AmmoType.Charge:
                currentIcon = null;
                fuelDisplay.gameObject.SetActive(false);
                break;
            case AmmoType.Shotgun:
                layout.spacing = new Vector2(0, 0);
                currentIcon = ammoSprites[3];
                layout.cellSize = new Vector2(5, 5);
                fuelDisplay.gameObject.SetActive(false);

                break;

        }
    }


    public void UpdateAmmo(RangedWeapon weapon)
    {
        SetAmmoType(weapon.ammoType);

        foreach(AmmoIconObject obj in ammoObjects)
        {
            Destroy(obj.gameObject);

        }

        ammoObjects.Clear();

        if (ammoType != AmmoType.Thrower)
        {
            for (int i = 0; i < weapon.ammunitionCount; i++)
            {
                AmmoIconObject temp = Instantiate(prefab, transform);
                temp.icon.sprite = currentIcon;
                ammoObjects.Add(temp);
            }
        } else if(ammoType == AmmoType.Thrower)
        {
            fuelDisplay.SetFuel(weapon);
        }
    }
}
