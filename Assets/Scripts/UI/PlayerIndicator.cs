using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    public Transform player;
    public Vector3 Offset = new Vector3(0, 0, -20);

    private void Start()
    {
        transform.position = player.position + Offset;
    }

    private void LateUpdate()
    {
        transform.position = player.position + Offset;
    }
}
