using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerGravity;

    [SerializeField]
    private GameObject greenTeleport;
    [SerializeField]
    private GameObject redTeleport;
    [SerializeField]
    private GameObject PauseManager;

    bool animPlaying = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Demo")
        {
            animPlaying = true;
        }
    }

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

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if(SceneManager.GetActiveScene().name == "MainGame")
        {
            if(collision.gameObject.name == "PortalGreen")
            {
                transform.position = redTeleport.transform.position;
                transform.rotation = redTeleport.transform.rotation;
            }
            else if (collision.gameObject.name == "PortalRed")
            {
                transform.position = greenTeleport.transform.position;
                transform.rotation = greenTeleport.transform.rotation;
            }
            else if(collision.gameObject.name == "ExitDoor")
            {
                StartCoroutine(NextLevelLoad());
            }
        }
    }

    IEnumerator NextLevelLoad()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("HouseClean");
    }
}
