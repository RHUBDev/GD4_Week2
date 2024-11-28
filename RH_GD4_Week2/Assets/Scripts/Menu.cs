using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Vector3 campos1 = new Vector3(0, 4, -10);
    private Vector3 campos2 = new Vector3(25, 4, -10);
    private Vector3 campos3 = new Vector3(50, 4, -10);
    private int level = 0;
    public GameObject rightbutton;
    public GameObject leftbutton;
    public TMP_Text playertext;
    private float t = 0;
    private float camspeed = 1.7f;
    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        Time.timeScale = 1.0f;
        playertext.text = "1 Player";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RightButton()
    {
        if (level == 0)
        {
            StartCoroutine(DoCam(campos1, campos2));
            playertext.text = "2 Player";
        }
        else if (level == 1)
        {
            StartCoroutine(DoCam(campos2, campos3));
            rightbutton.SetActive(false);
            playertext.text = "2 Player\nCoop";
        }
        leftbutton.SetActive(true);
        level += 1;
    }

    public void LeftButton()
    {
        if(level == 1)
        {
            StartCoroutine(DoCam(campos2, campos1));
            leftbutton.SetActive(false);
            playertext.text = "1 Player";
        }
        else if (level == 2)
        {
            StartCoroutine(DoCam(campos3, campos2));
            playertext.text = "2 Player";
        }
        rightbutton.SetActive(true);
        level -= 1;
    }

    public void StartButton()
    {
        if (level == 0)
        {
            SceneManager.LoadScene("OnePlayer");
        }
        else if (level == 1)
        {
            SceneManager.LoadScene("TwoPlayer");
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("CoOp");
        }
    }

    IEnumerator DoCam(Vector3 pos1, Vector3 pos2)
    {
        t = 0;
        while (t < 1)
        {
            t += (Time.deltaTime * camspeed);
            Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t);
            //Camera.main.transform.position = newpos;
            yield return null;
        }
    }
}