using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float splosionforce = 10000f;
    
    private void Update()
    {
        Debug.Log("bullet pos = " + gameObject.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, 10f);
        foreach(Collider hit in hits)
        {
            Rigidbody rig = hit.attachedRigidbody;
            if (rig)
            {
                rig.AddExplosionForce(splosionforce, gameObject.transform.position, 10f);
                Destroy(gameObject);
            }
        } 
    }
}
