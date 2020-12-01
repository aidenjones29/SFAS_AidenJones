using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public static class GlobalVariables
{
    public static bool menuActive;
}

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Camera gameCamera;
    [SerializeField]
    private Camera laptopCamera;
    [SerializeField]
    private GameObject gameUI;

    // Start is called before the first frame update
    void Start()
    {
        laptopCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            GlobalVariables.menuActive = !GlobalVariables.menuActive;

            if(GlobalVariables.menuActive)
            {
                gameCamera.enabled = false;
                laptopCamera.enabled = true;
                gameUI.SetActive(false);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                gameCamera.enabled = true;
                laptopCamera.enabled = false;
                gameUI.SetActive(true);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
