using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{

    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float lenght;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        lenght = transform.position.x; 
    }

    // Update is called once per frame
    void Update()
    {
        float distancemoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

            if (distancemoved > xPosition + lenght)
            xPosition = xPosition + lenght;
            else if (distancemoved < xPosition -lenght)
            xPosition = xPosition - lenght;
;    }
}
