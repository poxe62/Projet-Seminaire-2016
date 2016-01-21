using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 3.0f;

    //Animation triggers
    bool IsAttacking;
    bool IsMoving;
    bool IsJumping;
    private float Attackelapsed;

    //Rotation
    public float speed = 50.0f;
    private float rotate;
    private Quaternion qTo = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    private Quaternion qForward = Quaternion.identity;
    private Quaternion qBack = Quaternion.Euler(0.0f, 180.0f, 0.0f);


    // Use this for initialization
    void Start () {
        IsAttacking = false;
        IsMoving = false;
        IsJumping = false;
        Attackelapsed = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, 0); //Set X and Z velocity to 0
        if (Time.time > Attackelapsed + 1)
        {
            IsMoving = false;
            IsAttacking = false;
        }
        if (GetComponent<Rigidbody>().velocity.y < 0.05 && GetComponent<Rigidbody>().velocity.y > -0.05)
        {
            IsJumping = false;
        }

        //When Moving
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!IsAttacking)
            {
                transform.Translate(0, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
                if (!IsJumping)
                {
                    GetComponent<Animation>().Play("Run");
                }
                IsMoving = true;
            }
        }
        else
            IsMoving = false;

        //When Attacking
        if (Input.GetKeyDown(KeyCode.E) && !IsJumping)
        {
            Attackelapsed = Time.time;
            GetComponent<Animation>().Play("StaffHit");
            IsAttacking = true;
        }

        //When Jumping
        if (GetComponent<Rigidbody>().velocity.y < 0.05 && GetComponent<Rigidbody>().velocity.y > -0.05 && Input.GetKeyDown("space"))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 250, 0), ForceMode.Force);
            IsJumping = true;
            GetComponent<Animation>().Play("JumoRun");
        }

        //When not doing anything
            if (!IsAttacking && !IsMoving && !IsJumping)
            GetComponent<Animation>().Play("CombatModeA");


        //Rotating
        if (Input.GetAxis("Horizontal") != 0 && !IsAttacking)
        {
            rotate += Input.GetAxis("Horizontal");
            qTo = Quaternion.Euler(0.0f, rotate, 0.0f);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, Time.deltaTime * speed);
    }
}
