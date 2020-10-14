using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    private Vector2 velocity;
    public float smoothTimeY;
    public GameObject cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void FixedUpdate()
    {
        float posY = Mathf.SmoothDamp(transform.position.y, cam.transform.position.y, ref velocity.y, smoothTimeY);
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}
