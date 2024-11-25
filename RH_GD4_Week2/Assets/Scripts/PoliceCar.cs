using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public GameObject tank;
    public NavMeshAgent navagent;
    private float movespeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navagent.destination = tank.transform.position;
        Debug.Log("1 = " + tank.transform.position);
        Debug.Log("2 = " + navagent.destination);
        gameObject.transform.Translate(0, 0, movespeed * Time.deltaTime, Space.Self);
    }
}
