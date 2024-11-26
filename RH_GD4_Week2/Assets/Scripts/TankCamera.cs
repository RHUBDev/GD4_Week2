using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public GameObject tank;

    // Update is called once per frame
    void Update()
    {
        //Make the camera follow the tank, 30m behind it and 30m up
        gameObject.transform.position = tank.transform.position - tank.transform.forward * 30f + Vector3.up * 30f;
        transform.LookAt(tank.transform);
    }
}
