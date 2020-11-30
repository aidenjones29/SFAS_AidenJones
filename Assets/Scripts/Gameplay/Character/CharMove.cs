using UnityEngine;
using System.Collections;

public class CharMove : MonoBehaviour
{
    CharacterController PlayerController;
    public GameObject player;
    public float movementSpeed = 10.0f;

    private Vector3 moveDirection = Vector3.zero;
    private float x;
    private float y;
    private Vector3 rotateValue;

    void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    void Update()
    {

        if (PlayerController.isGrounded)
        {
            moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            moveDirection = moveDirection * movementSpeed;

            if (Input.GetKey("space"))
            {
                moveDirection.y += 10.0f;
            }
        }

        //Gravity
        moveDirection.y -= 15f * Time.deltaTime;
        PlayerController.Move(moveDirection * Time.deltaTime);

        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotateValue = new Vector3(x, y * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotateValue;

        if (Input.GetMouseButtonDown(0))
        {

        }
        if(Input.GetMouseButtonDown(1))
        {

        }
    }
}