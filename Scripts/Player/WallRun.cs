using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] Transform orientation;


    [Header("Detection")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;


    [Header("Wall Running")]
    private bool wallLeft;
    private bool wallRight;
    [SerializeField] private float wallrunJumpForce;
    [SerializeField] private float wallrunGravity;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float WallRunfov;
    [SerializeField] private float WallRunfovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt
    {
        get;
        private set;
    }


    private Rigidbody rb;





    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (canWallRun())
        {
            if (wallLeft)
            {
                startWallRun();
                Debug.Log("Left!");
            }
            else if (wallRight)
            {
                startWallRun();
                Debug.Log("Right!");
            }
            else
            {
                stopWallRun();
            }
        }
        else
        {
            stopWallRun();
        }

    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);

    }


    private bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }


    private void startWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallrunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, WallRunfov, WallRunfovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {

                Vector3 wallrunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallrunJumpDirection * wallrunJumpForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {

                Vector3 wallrunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallrunJumpDirection * wallrunJumpForce * 100, ForceMode.Force);
            }
        }

    }

    private void stopWallRun()
    {
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, WallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

}
