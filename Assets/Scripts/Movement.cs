using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System;
using Photon.Realtime;

public class Movement : MonoBehaviourPunCallbacks {

    //ID

    public int playerIndex;
    public string playerID;
    public string playerName;
    public int colorIndex;

    // START COUNTDOWN:

    bool start = true;
    Vector3 respawnPoint;
    public TextMeshProUGUI timer;

    // GENERAL MOVEMENT:

    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public CharacterController controller;
    float turnSmoothVelocity;

    //JUMP:

    bool jumping = false; 
    public float maxJump = 0.6f;
    float curJump;
    public float jumpInc = 0.075f;

    // ANIMATION:

    public Animator animator;
    float freeze = 1.4f;
    public ParticleSystem trail;

    public ParticleSystem speedEffect;
    public ParticleSystem sizeEffect;

    // RESPAWN:

    public bool moveable = false;
    public float respawnThreshold;

    public GameObject uiObject;
    public GameObject question;
    public TextMeshProUGUI countdown;

    // POINTS:

    public int points;
    // TextMeshProUGUI pointsUI;
    TextMeshProUGUI[] pointsUIList;
    // int[] pointsList;

    public PhotonView PV; //added this

    private void Start()
    {
        // Photon:

        PV = GetComponent<PhotonView>();
        pointsUIList = GameSetUp.GS.pointsUIList;
        points = 0;
        // PV.RPC("curPlayerSetup", RpcTarget.All, gameObject.tag, colorIndex, playerIndex, playerName);

        //UI:

        uiObject = GameSetUp.GS.uiObject;
        countdown = GameSetUp.GS.countdown;
        animator = GetComponent<Animator>();

        uiObject.SetActive(true);
        trail.Pause();
        speedEffect.Pause();
        sizeEffect.Pause();

        animator.enabled = false;


        // Player Settings:

        controller = GetComponent<CharacterController>();
        respawnPoint = transform.position;
        respawnThreshold = respawnPoint.y - 3;

        // Game Start:

        StartCoroutine("Countdown");

    }

    [PunRPC]
    void curPlayerSetup(string tag, int color, int index, string name)
    {
        this.tag = tag;
        // this.colorIndex = color;
        // this.playerIndex = index;
        // this.playerName = name;

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) //added this
        {
            Debug.Log("PV is mine!");
            if (moveable)
            {

                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                if (direction.magnitude >= 0.1f)
                {
                    animator.enabled = true;
                    PV.RPC("enableAnimation", RpcTarget.All, true);
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    controller.Move(direction * speed * Time.deltaTime);
                }

                else
                {
                    PV.RPC("enableAnimation", RpcTarget.All, false);

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
    }


    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            if (transform.position.y < respawnThreshold)
            {
                print("drop");

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
            moveable = false;
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

    [PunRPC]
    public void ChangePoints(int playerIndex, int x)
    {
        Debug.Log(playerIndex);
   
        points += x;
        pointsUIList[playerIndex].SetText("Points: " + points.ToString());
    }

    public int getPoints()
    {
        return points;
    }
    
    [PunRPC]
    private void enableAnimation(bool enable)
    {
        if (enable)
        {
            trail.Play();
        }
        else
        {
            trail.Stop();
            animator.PlayInFixedTime("Move", -1, freeze);
        }
    }

    public void boostSpeed(bool enable)
    {
        PV.RPC("doBoostSpeed", RpcTarget.All, enable);
    }

    public void boostSize(bool enable)
    {
        PV.RPC("doBoostSize", RpcTarget.All, enable);
    }

    [PunRPC]
    public void doBoostSpeed(bool enable)
    {
        if (enable)
        {
            speedEffect.Play();
        }
        else
        {
            speedEffect.Stop();
        }
    }

    [PunRPC]
    public void doBoostSize(bool enable)
    {
        if (enable)
        {
            sizeEffect.Play();
        }
        else
        {
            sizeEffect.Stop();
        }
    }
}