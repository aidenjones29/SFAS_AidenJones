using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerGravity;

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.menuActive == false)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * (playerGravity * Time.deltaTime);

            controller.Move(move * playerSpeed * Time.deltaTime);
        }
    }
}
