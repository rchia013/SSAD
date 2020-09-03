using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{

    // START COUNTDOWN:

    bool start = true;
    Vector3 respawnPoint;

    // GENERAL MOVEMENT:

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public CharacterController controller;
    float turnSmoothVelocity;

    //JUMP:

    bool jumping = false; 
    public float maxJump;
    float curJump;
    public float jumpInc;

    // ANIMATION:

    public Animator animator;
    float freeze = 1.4f;
    public ParticleSystem trail;

    // RESPAWN:

    public bool moveable = false;
    public float respawnThreshold;

    public GameObject uiObject;
    public GameObject question;
    public TextMeshProUGUI countdown;

    // POINTS:

    public int points;

    

    private void Start()
    {
        uiObject.SetActive(true);
        trail.Pause();

        animator.enabled = false;

        points = 0;

        controller = GetComponent<CharacterController>();

        respawnPoint = transform.position;

        StartCoroutine("Countdown");

    }

    // Update is called once per frame
    void Update()
    {

        if (moveable)
        { 

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.enabled = true;
                trail.Play();
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                controller.Move(direction * speed * Time.deltaTime);
            }

            else
            {
                trail.Stop();
                animator.PlayInFixedTime("Move", -1, freeze);
                //animator.enabled = false;

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

                if (IsGrounded())
                {
                    jumping = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y < respawnThreshold)
        {
            transform.position = respawnPoint;
            moveable = false;
            trail.Stop();
            animator.enabled = false;

            question.SetActive(false);
            uiObject.SetActive(true);

            StartCoroutine("Countdown");
        }

        if (jumping)
        {
            if (curJump < maxJump)
            {
                controller.Move(new Vector3(0f, jumpInc, 0f));
                curJump += jumpInc;
            }
            else
            {   
                jumping = false;
                curJump = 0;
            }
        }    
    }


    IEnumerator Countdown()
    {
        int counter = 3;
        countdown.SetText(counter.ToString());

        if (start)
        {
            start = false;
        }

        else
        {
            uiObject.GetComponent<Image>().color = new Color(0.965f, 0.275f, 0.196f, 0.396f);
        }

        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
       
            counter--;

            if (counter > 0)
            {
                countdown.SetText(counter.ToString());
            }
            else
            {
                countdown.SetText("");
            }

        }
        FinishRespawn();
    }

    void FinishRespawn()
    {
        moveable = true;
        uiObject.SetActive(false);

        controller.Move(Vector3.down * 1.5f);

    }

    bool IsGrounded()
    {
        if (transform.position.y < 1.55)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}