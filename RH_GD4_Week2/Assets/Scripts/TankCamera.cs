using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public GameObject tank;
    private bool isfirstperson = false;
    private Vector3 fpscameraoffset = new Vector3(0f,4f,1.3f);
    private Quaternion fpscameraangle = Quaternion.Euler(10f, 0f, 0f);
    public int tanknum;
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetButtonDown("Camera" + tanknum))
        {
            //Toggle camera mode
            isfirstperson = !isfirstperson;
        }

        if (isfirstperson)
        {
            //First person camera
            gameObject.transform.position = tank.transform.position + tank.transform.forward * fpscameraoffset.z + Vector3.up * fpscameraoffset.y;
            gameObject.transform.rotation = Quaternion.Euler(tank.transform.eulerAngles.x + 10f, tank.transform.eulerAngles.y, tank.transform.eulerAngles.z);
        }
        else
        {
            //Make the camera follow the tank, 30m behind it and 30m up
            gameObject.transform.position = tank.transform.position - tank.transform.forward * 30f + Vector3.up * 30f;
            transform.LookAt(tank.transform);
        }
    }
}
