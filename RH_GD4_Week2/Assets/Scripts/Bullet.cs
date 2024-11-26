using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float splosionforce = 10000f;

   public GameObject splosionprefab;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, 10f);
        foreach(Collider hit in hits)
        {
            Rigidbody rig = hit.attachedRigidbody;
            if (rig)
            {
                //Add explosion force to all rigidbodies
                rig.AddExplosionForce(splosionforce, gameObject.transform.position, 10f);
                GameObject splosion = Instantiate(splosionprefab, gameObject.transform.position, gameObject.transform.rotation);
                if(rig.gameObject.tag == "Police")
                {
                    //If police car, add damage
                    //Should be a portion of 110damage, scaled down by it's distance from the explosion, maximum of 10m away
                    float dist1 = (hit.ClosestPoint(gameObject.transform.position) - gameObject.transform.position).magnitude;
                    float damage1 = ((110f / 10f) *(10 - dist1));
                    rig.gameObject.GetComponent<PoliceCar>().TakeDamage(damage1);
                }
                Destroy(gameObject);
            }
        } 
    }
}
