using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tank : MonoBehaviour
{
    private float movespeed = 10f;
    private float turnspeed = 30f;
    private float turretturnspeed = 30f;
    public GameObject turretobj;
    public GameObject bulletprefab;
    public Transform gunpoint;
    private float bulletforce = 40f;
    public TMP_Text mainmessage;
    // Start is called before the first frame update
    void Start()
    {
        //Start game paused
        Time.timeScale = 0.0f;
        StartCoroutine(DoStart());
    }

    public IEnumerator DoEnd()
    {
        //Show end game message, and restart after 3 seconds
        mainmessage.text = "BUSTED!";
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("OnePlayer");
    }

    IEnumerator DoStart()
    {
        //After 2 seconds, start game
        yield return new WaitForSecondsRealtime(2);
        mainmessage.text = "";
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        //Rotate Tank
        gameObject.transform.Rotate(0, turnspeed * Time.deltaTime * horiz, 0);
        //Move Tank
        gameObject.transform.Translate(0, 0, movespeed * Time.deltaTime * vert);

        //Get turret input
        float turrhoriz = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            turrhoriz -= 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            turrhoriz += 1;
        }
        //TurnTurret
        turretobj.transform.Rotate(0, turretturnspeed * Time.deltaTime * turrhoriz, 0);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Shoot bullet
            GameObject bullet1 = Instantiate(bulletprefab, gunpoint.transform.position, gunpoint.transform.rotation);
            bullet1.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * bulletforce, ForceMode.Impulse);
        }
    }
}
