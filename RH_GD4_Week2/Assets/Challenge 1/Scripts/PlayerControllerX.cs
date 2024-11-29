using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControllerX : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float verticalInput;

    public GameObject[] obstacles;
    private int lastobstacle;
    private int firstobstacle;
    private int nextscorezone = 0;
    private int score = 0;
    public TMP_Text scoretext;
    public Rigidbody rig;
    public GameObject splosion;
    public Renderer[] rend;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        firstobstacle = 0;
        lastobstacle = 4;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            // get the user's vertical input
            verticalInput = Input.GetAxis("Vertical");

            // move the plane forward at a constant rate
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            // tilt the plane up/down based on up/down arrow keys
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime * verticalInput);

            if (transform.position.z > (obstacles[firstobstacle].transform.position.z + 120f))
            {
                float randomy = Random.Range(0f, 32.0f);
                obstacles[firstobstacle].transform.position = new Vector3(0, randomy, obstacles[lastobstacle].transform.position.z + 40f);
                firstobstacle += 1;
                lastobstacle += 1;
                if (firstobstacle > 4)
                {
                    firstobstacle = 0;
                }
                if (lastobstacle > 4)
                {
                    lastobstacle = 0;
                }
            }
        }
        rig.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ScoreZone" && other.gameObject.transform.parent.gameObject == obstacles[nextscorezone])
        {
            score += 1;
            scoretext.text = "Score: " + score.ToString("n0");
            nextscorezone += 1;
            if(nextscorezone > 4)
            {
                nextscorezone = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            dead = true;
            foreach (Renderer ren in rend)
            {
                ren.enabled = false;
            }
            rig.isKinematic = true;
            Instantiate(splosion, transform.position, transform.rotation);
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Challenge 1");
    }
}
