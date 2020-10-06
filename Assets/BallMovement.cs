using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class BallMovement : MonoBehaviour
{
    //ID

    public int playerIndex;
    public string playerID;
    public string playerName;
    public int colorIndex;

    // START COUNTDOWN:

    bool start = true;
    Vector3 respawnPoint;

    //player movement
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

    //Animation
    static Animator anim;
    float freeze = 1.4f;
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
    TextMeshProUGUI[] pointsUIList;

    // PHOTON:

    public PhotonView PV;



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        //anim.SetBool("Open_Anim", true);
        anim.SetBool("open", true);
    }
    // Update is called once per frame
    void Update()
    {
        Gravity();
        Jumping();
        Moving();



    }

    void Moving()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            if (isGrounded)
            {
                //anim.SetBool("Open_Anim", true);
                //anim.SetBool("Walk_Anim", true);
                anim.SetBool("isWalking", true);
            }
        }
        else
        {
            //anim.SetBool("Walk_Anim", false);
           // anim.SetBool("Open_Anim", false);
            anim.SetBool("isWalking", false);
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
            velocity.y = Mathf.Sqrt(jumpHeight * 3f * gravity);
            jumping = true;
        }
    }
    void FixedUpdate()
    {
        if (transform.position.y < respawnThreshold)
        {
            print("drop");

            transform.position = respawnPoint;
            moveable = false;
            anim.SetBool("isWalking", false);

            //question.SetActive(false);
            uiObject.SetActive(true);

            StartCoroutine("Countdown");
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
}
