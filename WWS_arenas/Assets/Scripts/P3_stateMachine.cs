using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_stateMachine : MonoBehaviour
{
    private Transform cowB_T = null;
    private SpriteRenderer cowB_SR = null;
    private Animator cowB_An = null;

    private Vector3 floorPos;
    private Vector3 velocity;
    private float speed = 3.5f;
    private Vector3 gravity;
    private Vector3 jumpF;
    private bool onAir = false;
    private bool canJump = false;

    private CBState currentState;

    enum CBState
    {
        IDLE_ST,
        RUN_ST,
        JUMP_ST,
        ROLL_ST
    };

    private void Start()
    {
        currentState = CBState.IDLE_ST;
        cowB_T = GetComponent<Transform>();
        cowB_SR = GetComponent<SpriteRenderer>();
        cowB_An = GetComponent<Animator>();

        floorPos = cowB_T.position;
        velocity = new Vector3(0, 0, 0);
        gravity = new Vector3(0, -40f, 0);
        jumpF = new Vector3(0, 15, 0);
    }

    private void Update()
    {
        Debug.Log(currentState);

        switch(currentState)
        {
            case CBState.IDLE_ST:

                cowB_An.SetFloat("move", 0);
                cowB_An.SetBool("jump", false);
                cowB_An.SetBool("roll", false);

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                if (Input.GetKey(KeyCode.LeftShift)) currentState = CBState.ROLL_ST;

                break;

            case CBState.RUN_ST:

                if(Input.GetKey(KeyCode.A))
                {
                    cowB_SR.flipX = true;
                    velocity.x = -speed;
                    cowB_An.SetFloat("move", -velocity.x);
                    
                    cowB_T.position += velocity * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    cowB_SR.flipX = false;

                    velocity.x = speed;
                    cowB_An.SetFloat("move", velocity.x);

                    cowB_T.position += velocity * Time.deltaTime;
                }

                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    cowB_An.SetFloat("move", 0);
                    currentState = CBState.IDLE_ST;
                }

                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                if (Input.GetKey(KeyCode.LeftShift)) currentState = CBState.ROLL_ST;

                break;

            case CBState.JUMP_ST:

                if (Input.GetKey(KeyCode.Space) && onAir == false)
                {
                    cowB_An.SetBool("roll", false);
                    cowB_An.SetFloat("move", 0);
                    cowB_An.SetBool("jump", true);

                    canJump = true;
                    onAir = true;
                    velocity.y = jumpF.y;
                }

                if (cowB_T.position.y < floorPos.y)
                {
                    cowB_An.SetBool("jump", false);

                    canJump = false;
                    onAir = false;
                    cowB_T.position = new Vector3(cowB_T.position.x, floorPos.y);

                    velocity = new Vector3(0, 0, 0);

                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                    currentState = CBState.IDLE_ST;
                }

                if (canJump)
                {
                    velocity += gravity * Time.deltaTime;
                    cowB_T.position += velocity * Time.deltaTime;
                }

                break;

            case CBState.ROLL_ST:

                if (cowB_T.position.y <= floorPos.y)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        cowB_An.SetFloat("move", 0);
                        cowB_An.SetBool("roll", true);

                        if (Input.GetKey(KeyCode.A))
                        {
                            cowB_SR.flipX = true;
                            velocity.x = -speed;

                            cowB_T.position += velocity * Time.deltaTime;
                        }

                        if (Input.GetKey(KeyCode.D))
                        {
                            cowB_SR.flipX = false;
                            velocity.x = speed;

                            cowB_T.position += velocity * Time.deltaTime;
                        }

                    }
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    cowB_An.SetBool("roll", false);
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) currentState = CBState.RUN_ST;
                    if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;
                    currentState = CBState.IDLE_ST;
                }

                if (Input.GetKey(KeyCode.Space)) currentState = CBState.JUMP_ST;

                break;
        }
    }
}
