using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public GameObject tank;
    public NavMeshAgent navagent;
    private float movespeed = 5f;
    private NavMeshPath path;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navagent.SetDestination(tank.transform.position);
        bool boo = navagent.CalculatePath(navagent.destination, navagent.path);
        if (boo)
        {
            navagent.Move((navagent.steeringTarget - navagent.transform.position).normalized * navagent.speed * Time.deltaTime);
        }

        if (transform.eulerAngles.x > 45 || transform.eulerAngles.x < -45 || transform.eulerAngles.z < -45 || transform.eulerAngles.z < -45)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.rotation = Quaternion.identity;
    }
}
