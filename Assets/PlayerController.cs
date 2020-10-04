using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

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
    private bool moveable;
    //Check for gravity
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    //Animation
    static Animator anim;




    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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
            if (!isGrounded && jumping)
            {
                anim.SetBool("isJumping", true);
                print("Jumping animation");
                jumping = false;
            }
            if (isGrounded)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isWalking", true);
                anim.SetBool("isIdle", true);

            }
        }
        else
        {
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
}
