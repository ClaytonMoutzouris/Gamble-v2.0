using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : EntityRenderer
{

    public SpriteRenderer rangedWeapon;
    public ButtonTooltip buttonTooltip;
    public Player player;
    public Vector2 weaponOffset;

    protected override void Awake()
    {
        base.Awake();
        rangedWeapon.transform.localPosition = weaponOffset;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

    }

    public void ShowButtonTooltip(bool show)
    {
        //Debug.Log("Show Tooltip " + show);
        buttonTooltip.ShowTooltip(show);
    }

    public void SetWeaponOffset(Vector2 offset)
    {
        weaponOffset = offset;

    }

    public void ShowWeapon(bool show)
    {
        rangedWeapon.gameObject.SetActive(show);
    }

    public void SetWeaponRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg - 90;
        //print("Angle: " + angle);


        //Rotate Player when aiming behind
        //print("THIS HAPPENED! Player should be facing left.");
        //Rotate the animation for the gun on the Z-axis
        
        if (angle <= -90)
        {
            rangedWeapon.flipY = true;
            //weapon.transform.localScale = new Vector3(1, -1, 1);
            rangedWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            rangedWeapon.transform.SetPositionAndRotation(rangedWeapon.transform.position, Quaternion.Euler(0, 0, -angle));
            rangedWeapon.transform.localPosition = new Vector2(-weaponOffset.x,weaponOffset.y);
        }
        else
        {
            rangedWeapon.flipY = false;
            //weapon.transform.localScale = new Vector3(1, 1, 1);
            rangedWeapon.transform.SetPositionAndRotation(rangedWeapon.transform.position, Quaternion.Euler(0, 0, -angle));
            rangedWeapon.transform.localPosition = weaponOffset;
        }
        
        //weapon.transform.SetPositionAndRotation(weapon.transform.position, Quaternion.Euler(0, 0, -angle));
    }

    public override void Draw()
    {
        base.Draw();
        //meleeWeapon.flipX = (Entity.mDirection == EntityDirection.Left);

    }
}
