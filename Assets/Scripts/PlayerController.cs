using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    //ID

    public int playerIndex;
    public string playerName;
    public int colorIndex;

    // START COUNTDOWN:

    bool start = true;
    Vector3 respawnPoint;

    // MOVEMENT:

    public CharacterController controller;
    Vector3 direction;
    Vector3 velocity;
    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float gravity;
    float turnSmoothVelocity;
    public float jumpHeight = 3.0f;
    private bool jumping = false;

    //Check for gravity
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    // ANIMATION:

    public Animator anim;
    float freeze = 1.4f;
    public ParticleSystem speedEffect;
    public ParticleSystem sizeEffect;
    public ParticleSystem jumpEffect;

    // RESPAWN:

    public bool respawning = false;
    public bool moveable = false;
    public float respawnThreshold;

    public GameObject uiObject;
    public GameObject question;
    public TextMeshProUGUI countdown;

    // POINTS:

    public int points;
    TextMeshProUGUI[] pointsUIList;

    // PHOTON:

    public PhotonView PV;

    // Username
    public GameObject username;


    private void Start()
    {
        // Photon:

        PV = GetComponent<PhotonView>();
        pointsUIList = GameSetUp.GS.pointsUIList;
        points = 0;
        PV.RPC("curPlayerSetup", RpcTarget.All, gameObject.tag, colorIndex, playerIndex, playerName);

        //UI:

        uiObject = GameSetUp.GS.uiObject;
        countdown = GameSetUp.GS.countdown;

        uiObject.SetActive(true);
        speedEffect.Pause();
        sizeEffect.Pause();
        jumpEffect.Pause();

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        username.GetComponent<TextMesh>().text = playerName;


        respawnPoint = transform.position;
        respawnThreshold = respawnPoint.y - 3;

        StartCoroutine("Countdown");
    }

    // Photon Setup:

    [PunRPC]
    void curPlayerSetup(string tag, int color, int index, string name)
    {
        this.tag = tag;
        this.colorIndex = color;
        this.playerIndex = index;
        this.playerName = name;

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (moveable)
            {
                Moving();
                Gravity();
                Jumping();
            }
            else
            {
                if (!anim.applyRootMotion)
                {
                    PV.RPC("setRootMotion", RpcTarget.All, true);
                    //anim.applyRootMotion = true;
                }
            }
        }
        else
        {
            if (!moveable)
            {
                if (!anim.applyRootMotion)
                {
                    PV.RPC("setRootMotion", RpcTarget.All, true);
                }
            }
        }
    }

    void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //anim.enabled = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            
            if (isGrounded)
            {
                PV.RPC("setWalking", RpcTarget.All, true);
            }
        }
        else
        {
            PV.RPC("setWalking", RpcTarget.All, false);
            //anim.PlayInFixedTime("Move", -1, freeze);
        }

    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    void Jumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    void FixedUpdate()
    {
       if (PV.IsMine)
        {
            if (transform.position.y < respawnThreshold && !respawning)
            {
                respawning = true;

                transform.position = respawnPoint;
                moveable = false;

                PV.RPC("setWalking", RpcTarget.All, false);

                question.SetActive(false);
                uiObject.SetActive(true);

                StartCoroutine("Countdown");
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
        respawning = false;
        uiObject.SetActive(false);
    }

    public void boostSpeed(bool enable)
    {
        PV.RPC("doBoostSpeed", RpcTarget.All, enable);
    }

    public void boostSize(bool enable)
    {
        PV.RPC("doBoostSize", RpcTarget.All, enable);
    }

    public void boostJump(bool enable)
    {
        PV.RPC("doBoostJump", RpcTarget.All, enable);
    }


    [PunRPC]
    public void setRootMotion(bool set)
    {
        anim.applyRootMotion = set;
    }

    [PunRPC]
    public void setWalking(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);
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

    [PunRPC]
    public void doBoostJump(bool enable)
    {
        if (enable)
        {
            jumpEffect.Play();
        }
        else
        {
            jumpEffect.Stop();
        }
    }

    [PunRPC]
    public void ChangePoints(int playerIndex, int x)
    {
        Debug.Log(playerIndex);

        points += x;
        pointsUIList[playerIndex].SetText(points.ToString());
    }

    public int getPoints()
    {
        return points;
    }
}
