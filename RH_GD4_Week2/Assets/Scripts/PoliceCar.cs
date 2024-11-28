using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceCar : MonoBehaviour
{
    public List<Tank> tanks;
    public NavMeshAgent navagent;
    private float movespeed = 5f;
    private NavMeshPath path;
    private Vector3 startpos;
    private Quaternion startrot;
    private float health = 100f;
    public GameObject carfire;
    public GameObject carsplosion;
    private bool dead = false;
    private float maxhealth = 100f;
   // public Tank tankscript;
    private Renderer rend;
    private Tank neartank;
    // Start is called before the first frame update
    void Start()
    {
        neartank = tanks[0];
        //Save start position and rotation
        startpos = transform.position;
        startrot = transform.rotation;
        //Find fire particles on child transform
        carfire = transform.GetChild(0).gameObject;
        rend = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < tanks.Count; i++)
        {
            //Find and follow the nearest live tank
            if(((tanks[i].transform.position - transform.position).magnitude < (neartank.transform.position - transform.position).magnitude) && !tanks[i].ended)
            {
                neartank = tanks[i];
            }
            else if (tanks[i].ended)
            {
                tanks.Remove(tanks[i]);
                if (tanks.Count > 0)
                {
                    neartank = tanks[0];
                }
            }
        }

        if (navagent.enabled)
        {
            //Set NavMeshAgent destination and move, if enabled
            navagent.SetDestination(neartank.transform.position);
            bool boo = navagent.CalculatePath(navagent.destination, navagent.path);
            if (boo && health > 0)
            {
                navagent.Move((navagent.steeringTarget - navagent.transform.position).normalized * navagent.speed * Time.deltaTime);
            }
        }
        //If car is flipped, kill it and respawn
        if (!(transform.eulerAngles.x < 60 || transform.eulerAngles.x > 300) || !(transform.eulerAngles.z < 60 || transform.eulerAngles.z > 300) && !dead)
        {
            dead = true;
            health = 0f;
            StartCoroutine(Die());
        }

        //If car health is 0 or less, kill it
        if (health <= 0 && !dead)
        {
            dead = true;
            health = 0f;
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(float damage)
    {
        //Take damage from bullet
        health -= damage;
    }

    IEnumerator Die()
    {
        //The rest of the car killing function
        rend.material.color = new Color(0.2f, 0.2f, 0.2f);
        dead = true;
        health = 0f;
        navagent.enabled = false;
        carfire.SetActive(true);
        Instantiate(carsplosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        Respawn();
    }

    void Respawn()
    {
        //Respawn at start position after 3 seconds
        carfire.SetActive(false);
        transform.position = startpos;
        transform.rotation = startrot;
        navagent.enabled = true;
        health = maxhealth;
        rend.material.color = new Color(1f, 1f, 1f);
        dead = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If we hit the Tank and are not already dead, the player loses!
        if (collision.gameObject.tag == "Tank" && !dead)
        {
            Tank tank1 = collision.transform.root.gameObject.GetComponent<Tank>();
            tank1.DoEnd();
        }
    }
}
