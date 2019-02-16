using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public float backgroundSize;
    public float parallaxSpeed;
    public bool scrolling, parallax;
    

    private Transform cameraTransform;
    [SerializeField]
    private Transform[] layers;
    private float viewZone = 60;
    [SerializeField]
    private int leftIndex;
    [SerializeField]
    private int rightIndex;
    private float lastCameraX;
    private float lastCameraY;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;

        layers = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void LateUpdate()
    {
        if (parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            float deltaY = cameraTransform.position.y - lastCameraY;

            transform.position += new Vector3(deltaX * parallaxSpeed, deltaY * parallaxSpeed);
        }

        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;


        
        if (scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }
        

    }

    private void ScrollLeft()
    {
        //print("Scrolling Left");
        int lastRight = rightIndex;
        layers[rightIndex].position = new Vector3((layers[leftIndex].position.x - backgroundSize), layers[leftIndex].position.y);
        leftIndex = rightIndex;
        rightIndex--;
        if(rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        //print("Scrolling Right");

        int lastLeft = leftIndex;
        layers[leftIndex].position = new Vector3((layers[rightIndex].position.x + backgroundSize), layers[rightIndex].position.y);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }
}

