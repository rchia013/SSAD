using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using Photon.Realtime;

/// <summary>
/// This class handles the movement of the characters
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    // Parameters that define ID of the players

    public int playerIndex;
    public string playerName;
    public int colorIndex;

    // Parameters that starts the countdown when the player enters the game

    bool start = true;
    Vector3 respawnPoint;

    // Parameters for the players movement

    public CharacterController controller;
    Vector3 direction;
    Vector3 velocity;
    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float gravity;
    float turnSmoothVelocity;
    public float jumpHeight = 3.0f;
    private bool jumping = false;

    // Parameters that checks for gravity

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    // Parameters that handles the animation of the characters

    public Animator anim;
    float freeze = 1.4f;
    public ParticleSystem speedEffect;
    public ParticleSystem sizeEffect;
    public ParticleSystem jumpEffect;

    // Parameters for when the player is respawninig

    public bool respawning = false;
    public bool moveable = false;
    public float respawnThreshold;

    public GameObject uiObject;
    public GameObject question;
    public TextMeshProUGUI countdown;

    // Parameters for the players points

    public int points;
    TextMeshProUGUI[] pointsUIList;

    // PHOTON:

    public PhotonView PV;

    // The players username

    public GameObject username;


    private void Start()
    {
        // Photon:

        PV = GetComponent<PhotonView>();
        pointsUIList = GameSetUp.GS.pointsUIList;
        points = 0;
        PV.RPC("curPlayerSetup", RpcTarget.All, gameObject.tag, colorIndex, playerIndex, playerName);

        //UI of the overall gameplay:

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
            // Controls the logic if moveable = true
            if (moveable)
            {
                Moving();
                Gravity();
                Jumping();
                anim.applyRootMotion = false;
            }
            else if (!moveable)
            {
                anim.applyRootMotion = true;
            }
        }
    }

    /// <summary>
    /// Function that moves the character.
    /// </summary>
    void Moving()
    {
        //Parameters for Moving the character
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // If WASD or Arrow keys are pressed
        if (direction.magnitude >= 0.1f)
        {
            // Move the character
            anim.enabled = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            
            // Checks if the player is in contact with the ground.
            if (isGrounded)
            {
                PV.RPC("setWalking", RpcTarget.All, true);
                anim.SetBool("isWalking", true);
            }
        }
        else
        {
             PV.RPC("setWalking", RpcTarget.All, false);
             anim.SetBool("isWalking", false);
             anim.PlayInFixedTime("Move", -1, freeze);
        }

    }

    /// <summary>
    /// Gravity of the character.
    /// </summary>
    void Gravity()
    {
        // Parameter to check if the character is in contact with the ground.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        //Ensures that the player falls if not in contact with the ground.
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    /// <summary>
    /// Jumping of the character
    /// </summary>
    void Jumping()
    {
        //If Spacebar is pressed and the character is in contact with the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Jumps by increasing the player's y-parameter
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }
    /// <summary>
    /// Handles the respawning of the character.
    /// </summary>
    void FixedUpdate()
    {
       if (PV.IsMine)
        {
            // If the player falls beyond a certain threshold, respawns the character
            if (transform.position.y < respawnThreshold && !respawning)
            {
                anim.applyRootMotion = false;
                respawning = true;

                transform.position = respawnPoint;
                moveable = false;
                anim.SetBool("isWalking", false);
                PV.RPC("setWalking", RpcTarget.All, false);

                question.SetActive(false);
                uiObject.SetActive(true);

                StartCoroutine("Countdown");
            }
         }
    }
    /// <summary>
    /// Countdowns for the players respawning time.
    /// </summary>
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
    /// <summary>
    /// Handles the controls once the player has finished respawning.
    /// </summary>
    void FinishRespawn()
    {
        moveable = true;
        respawning = false;
        uiObject.SetActive(false);
    }

    /// <summary>
    /// Boosts the speed of the player when the player uses the speed powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
    public void boostSpeed(bool enable)
    {
        PV.RPC("doBoostSpeed", RpcTarget.All, enable);
    }

    /// <summary>
    /// Boosts the size of the player when the player uses the size powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
    public void boostSize(bool enable)
    {
        PV.RPC("doBoostSize", RpcTarget.All, enable);
    }

    /// <summary>
    /// Boosts the jump of the player when the player uses the jump powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
    public void boostJump(bool enable)
    {
        PV.RPC("doBoostJump", RpcTarget.All, enable);
    }

    /// <summary>
    /// Sets the walking animation of the character.
    /// </summary>
    /// <param name="isWalking">if set to <c>true</c> [is walking].</param>
    [PunRPC]
    public void setWalking(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);
    }

    /// <summary>
    /// Enables the speed boost effect when the player uses the speed powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
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
    /// <summary>
    /// Enables the size boost effect when the player uses the size powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
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
    /// <summary>
    /// Enables the jump boost effect when the player uses the jump powerup.
    /// </summary>
    /// <param name="enable">if set to <c>true</c> [enable].</param>
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

    /// <summary>
    /// Changes the points of the player.
    /// </summary>
    /// <param name="playerIndex">Index of the player.</param>
    /// <param name="x">The x.</param>
    [PunRPC]
    public void ChangePoints(int playerIndex, int x)
    {

        points += x;
        pointsUIList[playerIndex].SetText(points.ToString());
    }

    /// <summary>
    /// Gets the points.
    /// </summary>
    /// <returns> Points </returns>
    public int getPoints()
    {
        return points;
    }
}
