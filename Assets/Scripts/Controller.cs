using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // camera
    public Camera playerCamera;
    public float maxAngle = 50f;
    public float mouseSensitivity = 2f;
    float horizontal;
    float vertical;
    bool UImode = false;

    // movement
    public Rigidbody rb;
    bool isGround = true;
    float checkGroundDistance = .6f;
    float walkSpeed = 3f;
    float runSpeed = 7f;
    float maxVelocityChange = 10f;

    private void Start()
    {
        // camera init rotation
        horizontal = transform.localEulerAngles.y;
        vertical = playerCamera.transform.localEulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // unlock cursor
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // TODO: UImode will keep rotating camera
            UImode = !UImode;
            Cursor.lockState = UImode ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = UImode ? true : false;
        }
            
        if (!UImode)
        {
            // camera rotate
            horizontal += mouseSensitivity * Input.GetAxis("Mouse X");
            vertical -= mouseSensitivity * Input.GetAxis("Mouse Y");
            vertical = Mathf.Clamp(vertical, -maxAngle, maxAngle);

            transform.localEulerAngles = new Vector3(0, horizontal, 0);
            playerCamera.transform.localEulerAngles = new Vector3(vertical, 0, 0);

            // jump
            CheckGround();
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(0f, 3.5f, 0f, ForceMode.Impulse);
                isGround = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!UImode && isGround)
        {
            // move
            float speed = Input.GetMouseButton(1) ? runSpeed : walkSpeed;

            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity) * speed;

            Vector3 velocityChange = (targetVelocity - rb.velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    void CheckGround()
    {
        Vector3 foot_pos = transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0);
        if (Physics.Raycast(foot_pos, Vector3.down, out RaycastHit hit, checkGroundDistance))
            isGround = true;
        else
            isGround = false;
    }

}
