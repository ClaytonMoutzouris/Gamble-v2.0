using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Vector3 Offset = Vector3.zero;
    public Vector3 scrollDirection = Vector3.up;
    public float scrollSpeed = 1;
    public float duration = 1.5f;
    float spawnTimestamp;
    public TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().sortingLayerName = "UI";
        spawnTimestamp = Time.time;
        //GetComponent<MeshRenderer>().sortingLayerID = 0;
    }

    public void SetOffset(Vector3 newOffset)
    {
        Offset = newOffset;
        transform.position += Offset;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += scrollDirection * Time.deltaTime * scrollSpeed;

        if(Time.time > spawnTimestamp + duration)
        {
            Destroy(gameObject);
        }
    }
}
