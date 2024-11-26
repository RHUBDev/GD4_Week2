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
    private Vector3 startpos;
    private Quaternion startrot;
    private float health = 100f;
    public GameObject carfire;
    public GameObject carsplosion;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        startrot = transform.rotation;
        carfire = transform.GetChild(0).gameObject;
        //carsplosion = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        navagent.SetDestination(tank.transform.position);
        bool boo = navagent.CalculatePath(navagent.destination, navagent.path);
        if (boo && health > 0)
        {
            navagent.Move((navagent.steeringTarget - navagent.transform.position).normalized * navagent.speed * Time.deltaTime);
        }

        if (transform.eulerAngles.x > 60 || transform.eulerAngles.x < -60 || transform.eulerAngles.z > 60 || transform.eulerAngles.z < -60)
        {
            health = 0f;
            StartCoroutine(Die());
        }

        if(health <= 0)
        {
            health = 0f;
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage Amount = " + damage);
        health -= damage;
    }

    IEnumerator Die()
    {
        carfire.SetActive(true);
        Instantiate(carsplosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        Respawn();
    }

    void Respawn()
    {
        carfire.SetActive(false);
        carsplosion.SetActive(false);
        transform.position = startpos;
        transform.rotation = startrot;
    }
}
