using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    private float movespeed = 10f;
    private float turnspeed = 30f;
    private float turretturnspeed = 30f;
    public GameObject turretobj;
    public GameObject bulletprefab;
    public Transform gunpoint;
    private float bulletforce = 40f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        gameObject.transform.Rotate(0, turnspeed * Time.deltaTime * horiz, 0);

        gameObject.transform.Translate(0, 0, movespeed * Time.deltaTime * vert);

        float turrhoriz = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            turrhoriz -= 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            turrhoriz += 1;
        }
        turretobj.transform.Rotate(0, turretturnspeed * Time.deltaTime * turrhoriz, 0);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet1 = Instantiate(bulletprefab, gunpoint.transform.position, gunpoint.transform.rotation);
            bullet1.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * bulletforce, ForceMode.Impulse);
        }
    }
}