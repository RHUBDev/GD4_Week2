using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTankSpin : MonoBehaviour
{
    private float spinspeed = 40f;

    // Update is called once per frame
    void Update()
    {
        //Spin tanks in menu scene
        gameObject.transform.Rotate(0, spinspeed * Time.deltaTime, 0);
    }
}
