using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // speed += Time.deltaTime * 10;
        // transform.rotation = Quaternion.Euler(0,speed,0);
        transform.Rotate(Vector3.up * (Time.deltaTime * speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("onTriggerEnter");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
    }
}
