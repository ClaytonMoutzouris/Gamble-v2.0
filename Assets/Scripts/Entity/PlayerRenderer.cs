using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : EntityRenderer
{

    public SpriteRenderer weapon;
    public Player player;
    public Vector2 weaponOffset;

    protected override void Awake()
    {
        base.Awake();
        weapon.transform.localPosition = weaponOffset;
    }

    public void SetWeaponOffset(Vector2 offset)
    {
        weaponOffset = offset;

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
            weapon.flipY = true;
            //weapon.transform.localScale = new Vector3(1, -1, 1);
            weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weapon.transform.SetPositionAndRotation(weapon.transform.position, Quaternion.Euler(0, 0, -angle));
            weapon.transform.localPosition = new Vector2(-weaponOffset.x,weaponOffset.y);
        }
        else
        {
            weapon.flipY = false;
            //weapon.transform.localScale = new Vector3(1, 1, 1);
            weapon.transform.SetPositionAndRotation(weapon.transform.position, Quaternion.Euler(0, 0, -angle));
            weapon.transform.localPosition = weaponOffset;
        }
        
        //weapon.transform.SetPositionAndRotation(weapon.transform.position, Quaternion.Euler(0, 0, -angle));
    }
}
