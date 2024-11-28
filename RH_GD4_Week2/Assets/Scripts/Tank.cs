using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

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
    public int score = 0;
    private float damagetime = 2f;
    private bool grounded = true;
    public GameObject[] wheels;
    public int tanknum;
    public Tank othertank;
    public bool ended = false;
    private bool doneend = false;
    Scene activescene;
    public NavMeshObstacle navob;
    private int highscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Start game paused
        score = 0;
        Time.timeScale = 0.0f;
        activescene = SceneManager.GetActiveScene();
        StartCoroutine(DoStart());
        if (PlayerPrefs.HasKey("CoOpHighScore"))
        {
            highscore = PlayerPrefs.GetInt("CoOpHighScore");
        }
    }

    public void DoEnd()
    {
        ended = true;
        //Show end game message, and restart after 3 seconds
        damagemessage.text = "";
        mainmessage.text = "BUSTED!";
        //Turn on NavMeshObstacle so cops avoid us
        navob.enabled = true;
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
        if (!ended)
        {
            //Get tank move inputs
            float horiz = Input.GetAxis("Horizontal" + tanknum);
            float vert = Input.GetAxis("Vertical" + tanknum);

            float turrhoriz = 0;
            if (activescene.name == "OnePlayer")
            {
                //Get turret input if single player

                if (Input.GetKey(KeyCode.Q))
                {
                    turrhoriz -= 1;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    turrhoriz += 1;
                }
            }

            //Get fire input 
            if (Input.GetButtonDown("Fire" + tanknum))
            {
                //Shoot bullet
                Bullet bullet1 = Instantiate(bulletprefab, gunpoint.transform.position, gunpoint.transform.rotation).GetComponent<Bullet>();
                bullet1.tank = this;
                bullet1.gameObject.GetComponent<Rigidbody>().AddForce(gunpoint.transform.forward * bulletforce, ForceMode.Impulse);
            }

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
                    grounded = true;
                }
            }

            //If not rotated more than 60 degrees from being flat, and if 'grounded', then add inputs
            if ((transform.eulerAngles.x < 60 || transform.eulerAngles.x > 300) && (transform.eulerAngles.z < 60 || transform.eulerAngles.z > 300) && grounded)
            {
                //Rotate Tank if moving
                if (vert > 0)
                {
                    gameObject.transform.Rotate(0, turnspeed * Time.deltaTime * horiz, 0);
                }
                else if (vert < 0)
                {
                    //Flip horizontal movement if reversing
                    gameObject.transform.Rotate(0, turnspeed * Time.deltaTime * -horiz, 0);
                }
                //Move Tank
                gameObject.transform.Translate(0, 0, movespeed * Time.deltaTime * vert);
            }

            if (activescene.name == "OnePlayer")
            {
                //TurnTurret (Single Player)
                turretobj.transform.Rotate(0, turretturnspeed * Time.deltaTime * turrhoriz, 0);
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
        else
        {
            if (!doneend)
            {
                if (activescene.name == "OnePlayer")
                {
                    //If player has been caught, show beaten/failed high score
                    doneend = true;
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
                    else if (score < highscore)
                    {
                        int diff = highscore - score;
                        endmessage.text = "Unlucky!\n<color=red>-$" + diff.ToString("n0") + "</color>";
                    }
                    else if (score == highscore)
                    {
                        endmessage.text = "Almost!\n<color=green>+$0</color>";
                    }

                    StartCoroutine(DoReload());
                }
                else if (activescene.name == "TwoPlayer")
                {
                    //If both players have been caught, show won/lost
                    if (ended && othertank.ended)
                    {
                        doneend = true;
                        if (score > othertank.score)
                        {
                            int diff = score - othertank.score;
                            endmessage.text = "Congrats!\n<color=green>+$" + diff.ToString("n0") + "</color>";
                        }
                        else if (score < othertank.score)
                        {
                            int diff = othertank.score - score;
                            endmessage.text = "Unlucky!\n<color=red>-$" + diff.ToString("n0") + "</color>";
                        }
                        else if (score == othertank.score)
                        {
                            endmessage.text = "Draw!\n<color=green>+$0</color>";
                        }
                        //Call restart function for only 1 player
                        if (tanknum == 1)
                        {
                            StartCoroutine(DoReload());
                        }
                    }
                }
                else if (activescene.name == "CoOp")
                {
                    //If both players have been caught, show beaten/failed high score
                    if (ended && othertank.ended)
                    {
                        doneend = true;
                        int totalscore = 0;
                        if(tanknum == 1)
                        {
                            totalscore = score;
                        }
                        else if(tanknum == 2)
                        {
                            totalscore = othertank.score;
                        }
                        
                        if (totalscore > highscore)
                        {
                            int diff = totalscore - highscore;
                            endmessage.text = "Congrats!\n<color=green>+$" + diff.ToString("n0") + "</color>";
                            PlayerPrefs.SetInt("CoOpHighScore", totalscore);
                        }
                        else if (totalscore < highscore)
                        {
                            int diff = highscore - totalscore;
                            endmessage.text = "Unlucky!\n<color=red>-$" + diff.ToString("n0") + "</color>";
                        }
                        else if (totalscore == highscore)
                        {
                            endmessage.text = "Almost!\n<color=green>+$0</color>";
                        }
                        //Call restart function for only 1 player
                        if (tanknum == 1)
                        {
                            StartCoroutine(DoReload());
                        }
                    }
                }
            }
        }
    }
    public void CauseDamage(int amount)
    {
        if (activescene.name != "CoOp")
        {
            score += amount;
            scoremessage.text = "Damage: $" + score.ToString("n0");
        }
        else if (tanknum == 1)
        {
            score += amount;
            scoremessage.text = "Damage: $" + score.ToString("n0");
        }
        else if (tanknum == 2)
        {
            othertank.score += amount;
            scoremessage.text = "Damage: $" + othertank.score.ToString("n0");
        }
        //Show damage in UI
        //I googled how to do the number formatting with .ToString("n0") because I couldn't remember how to do it
        damagetime = 0f;
        damagemessage.text = "+$" + amount.ToString("n0");
    }

    IEnumerator DoReload()
    {
        //Restart game
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(activescene.name);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        //Quit back to menu scene
        SceneManager.LoadScene("Menu");
    }
}
