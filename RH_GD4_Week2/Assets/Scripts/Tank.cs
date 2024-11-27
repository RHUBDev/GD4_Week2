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
    public TMP_Text scoremessage;
    public TMP_Text damagemessage;
    public TMP_Text endmessage;
    private int score = 0;
    private float damagetime = 2f;
    private bool grounded = true;
    public GameObject[] wheels;
    // Start is called before the first frame update
    void Start()
    {
        
        //Start game paused
        score = 0;
        Time.timeScale = 0.0f;
        StartCoroutine(DoStart());
    }

    public IEnumerator DoEnd()
    {
        //Show end game message, and restart after 3 seconds
        damagemessage.text = "";
        mainmessage.text = "BUSTED!";
        int highscore = 0;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highscore = PlayerPrefs.GetInt("HighScore");
        }
        if (score > highscore)
        {
            int diff = score - highscore;
            //I also googled how to do this Rich Text stuff (colour text) cos I forgot 
            endmessage.text = "Congrats!\n<color=green>+$" + diff.ToString("n0") + "</color>";
            PlayerPrefs.SetInt("HighScore", score);
        }
        else
        {
            int diff = highscore - score;
            endmessage.text = "Unlucky!:\n<color=red>-$" + diff.ToString("n0") + "</color>";
        }
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
        //RaycastHit[] hit;
        grounded = false;
        foreach (GameObject wheel in wheels)
        {
            //Raycast down from the outer edge of each tyre to see if it is near enough to the ground, then set grounded to true if any of them hit
            float sideoffset = 0f;
            if (wheel.name.EndsWith("l"))
            {
                sideoffset = -0.226f;
            }
            else if (wheel.name.EndsWith("r"))
            {
                sideoffset = 0.226f;
            }
            RaycastHit[] hit = Physics.RaycastAll(wheel.transform.position + wheel.transform.right * sideoffset, -Vector3.up, 0.95f);
            if (hit.Length > 0)
            {
                Debug.Log("True");
                grounded = true;
            }
        }

        if (grounded)
        {
            Debug.Log("True");
        }
        else
        {
            Debug.Log("False");
        }
        //If not rotated more than 60 degrees from being flat, and if 'grounded', then add inputs
        if ((transform.eulerAngles.x < 60 || transform.eulerAngles.x > 300) && (transform.eulerAngles.z < 60 || transform.eulerAngles.z > 300) && grounded)
        {
            //Rotate Tank
            gameObject.transform.Rotate(0, turnspeed * Time.deltaTime * horiz, 0);
            //Move Tank
            gameObject.transform.Translate(0, 0, movespeed * Time.deltaTime * vert);
        }
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
            Bullet bullet1 = Instantiate(bulletprefab, gunpoint.transform.position, gunpoint.transform.rotation).GetComponent<Bullet>();
            bullet1.tank = this;
            bullet1.gameObject.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * bulletforce, ForceMode.Impulse);
        }

        if (damagetime < 2)
        {
            damagetime += Time.deltaTime;
        }
        else
        {
            //Remove damage UI text after 2 seconds
            damagemessage.text = "";
        }
    }

    public void CauseDamage(int amount)
    {
        score += amount;
        //Show damage in UI
        //I googled how to do the number formatting with .ToString("n0") because I couldn't remember how to do it
        scoremessage.text = "Damage: $" + score.ToString("n0");
        damagetime = 0f;
        damagemessage.text = "+$" + amount.ToString("n0");
    }
}
