using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public GameObject tank;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = tank.transform.position - tank.transform.forward * 30f + Vector3.up * 30f;
        transform.LookAt(tank.transform);
    }
}
