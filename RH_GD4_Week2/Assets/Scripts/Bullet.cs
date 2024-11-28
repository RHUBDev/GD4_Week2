using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float splosionforce = 10000f;
    public GameObject splosionprefab;
    public Tank tank;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, 10f);
        foreach (Collider hit in hits)
        {
            Rigidbody rig = hit.attachedRigidbody;
            if (rig)
            {
                //Add explosion force to all rigidbodies
                rig.AddExplosionForce(splosionforce, gameObject.transform.position, 10f);
                GameObject splosion = Instantiate(splosionprefab, gameObject.transform.position, gameObject.transform.rotation);
                if (rig.gameObject.tag == "Police")
                {
                    //If police car, add damage
                    //Should be a portion of 110damage, scaled down by it's distance from the explosion, maximum of 10m away
                    float dist1 = (hit.ClosestPoint(gameObject.transform.position) - gameObject.transform.position).magnitude;
                    float damage1 = ((110f / 10f) * (10 - dist1));
                    rig.gameObject.GetComponent<PoliceCar>().TakeDamage(damage1);
                }
                Destroy(gameObject);
            }
        }
        if (collision.collider.gameObject.name.StartsWith("Building") || collision.collider.gameObject.name.StartsWith("Vehicle"))
        {
            Renderer rend = collision.collider.gameObject.GetComponent<Renderer>();
            if (rend.material.color != new Color(0.2f, 0.2f, 0.2f))
            {
                //If we hit a building or a vehicle, and it hasn't already been hit, make it look 'burnt', and send a damage value to the Tank script
                rend.material.color = new Color(0.2f, 0.2f, 0.2f);
                if (collision.collider.gameObject.name.StartsWith("Building Sky_big"))
                {
                    tank.CauseDamage(1000000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building Sky_small"))
                {
                    tank.CauseDamage(600000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building_Stadium"))
                {
                    tank.CauseDamage(1000000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building_Residential"))
                {
                    tank.CauseDamage(600000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building_Super"))
                {
                    tank.CauseDamage(600000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building_Gas"))
                {
                    tank.CauseDamage(600000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building_Factory"))
                {
                    tank.CauseDamage(600000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Building"))
                {
                    tank.CauseDamage(300000);
                }
                else if (collision.collider.gameObject.name.StartsWith("Vehicle"))
                {
                    tank.CauseDamage(30000);
                }
            }
        }
    }
}
