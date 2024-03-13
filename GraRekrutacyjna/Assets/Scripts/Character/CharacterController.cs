using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Rigidbody rB;
    Animator animator;

    public bool grounded;
    public LayerMask groundM;
    public float speed;
    public float sprintSpeed;
    public float jumpForce;
    public bool sprinting;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y -1.0f, transform.position.z), 0.5f, groundM);
        if(grounded)
        {
            animator.SetBool("InAir", true);
        }
        else animator.SetBool("InAir", false);


        if (sprinting)
        {
            horizontal = Input.GetAxis("Horizontal") * sprintSpeed;
            vertical = Input.GetAxis("Vertical") * sprintSpeed;
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal") * speed;
            vertical = Input.GetAxis("Vertical") * speed;

            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

        Vector3 movePostion = transform.right * horizontal + transform.forward * vertical;
        Vector3 newMovePosition = new Vector3(movePostion.x, rB.velocity.y, movePostion.z);

        rB.velocity = newMovePosition;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rB.velocity = new Vector3(rB.velocity.x, jumpForce, rB.velocity.z);
            animator.SetBool("Space", true);
        }
        else
        {
            animator.SetBool("Space", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && !grounded)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }
    }

}
