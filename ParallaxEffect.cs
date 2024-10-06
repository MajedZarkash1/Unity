using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

//starting position for the parallax game object
    Vector2 startingposition;


// start Z value of the parallax game object
    float startingZ;

    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingposition;


    float zDistanceformTarget => transform.position.z - followTarget.transform.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceformTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zDistanceformTarget) / clippingPlane;

// Start is called before the first frame update
    void Start()
    {
        startingposition = transform.position;
        startingZ = transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingposition + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x,newPosition.y, startingZ);
    }
}
