using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3.0f;
    public Vector3 Offset = Vector3.zero;
    public Vector3 scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);

    }

    public void SetOffset(Vector3 newOffset)
    {
        Offset = newOffset;
        transform.position += Offset;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += scrollSpeed*Time.deltaTime;
    }
}
