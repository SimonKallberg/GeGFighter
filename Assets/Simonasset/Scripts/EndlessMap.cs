using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessMap : MonoBehaviour
{
    public float leftC = 0.0f;
    public float rightC = 0.0f;
    public float topC = 0.0f;
    public float botC = 0.0f;
    public float buffer = 0.0f;
    public float zdist = 10.0f;

    private void Awake()
    {
        leftC = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, zdist)).x;
        rightC = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, zdist)).x;
        topC = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, zdist)).y;
        botC = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, zdist)).y;
    }

    private void Update()
    {
        if (transform.position.x < leftC - buffer)
        {
            transform.position = new Vector3(rightC + buffer, transform.position.y, transform.position.z);
        }
        if (transform.position.x > rightC + buffer)
        {
            transform.position = new Vector3(leftC - buffer, transform.position.y, transform.position.z);
        }
        if (transform.position.y < topC + buffer)
        {
            transform.position = new Vector3(transform.position.x, botC + buffer, transform.position.z);
        }
        if (transform.position.y > botC + buffer)
        {
            transform.position = new Vector3(transform.position.x, topC - buffer, transform.position.z);
        }
    }
}
