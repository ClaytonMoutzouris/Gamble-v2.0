using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoDisplay : MonoBehaviour
{
    public List<AmmoIconObject> ammoObjects;
    WeaponClassType ammoType;
    public GridLayoutGroup layout;
    public List<Sprite> ammoSprites;
    public Sprite currentIcon;
    public AmmoIconObject prefab;
    public FuelDisplay fuelDisplay;

    public void SetAmmoType(WeaponClassType type)
    {
        ammoType = type;
        switch (ammoType)
        {
            case WeaponClassType.Pistol:
                layout.cellSize = new Vector2(5, 5);
                layout.spacing = new Vector2(0,0);
                currentIcon = ammoSprites[0];
                fuelDisplay.gameObject.SetActive(false);

                break;
            case WeaponClassType.Launcher:
                currentIcon = ammoSprites[1];
                layout.cellSize = new Vector2(5, 5);
                fuelDisplay.gameObject.SetActive(false);
                layout.spacing = new Vector2(0, 0);

                break;
            case WeaponClassType.Automatic:
                layout.cellSize = new Vector2(1, 5);
                layout.spacing = new Vector2(1, 0);

                currentIcon = ammoSprites[2];
                fuelDisplay.gameObject.SetActive(false);

                break;
            case WeaponClassType.Thrower:
                currentIcon = null;
                fuelDisplay.gameObject.SetActive(true);

                break;
            case WeaponClassType.Charge:
                currentIcon = null;
                fuelDisplay.gameObject.SetActive(false);
                break;
            case WeaponClassType.Shotgun:
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

        if (ammoType != WeaponClassType.Thrower)
        {
            for (int i = 0; i < weapon.ammunitionCount; i++)
            {
                AmmoIconObject temp = Instantiate(prefab, transform);
                temp.icon.sprite = currentIcon;
                ammoObjects.Add(temp);
            }
        } else if(ammoType == WeaponClassType.Thrower)
        {
            fuelDisplay.SetFuel(weapon);
        }
    }
}
