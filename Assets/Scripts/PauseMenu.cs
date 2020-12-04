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

    private Animation camAnimation;
    [SerializeField]
    private AnimationClip camMoveAnimOut;
    [SerializeField]
    private AnimationClip camMoveAnimIn;

    private bool gameResumed = false;
    private float gameResumeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        camAnimation = laptopCamera.GetComponent<Animation>();
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
                camAnimation.clip = camMoveAnimOut;
                camAnimation.Play();
            }
            else
            {
                camAnimation.clip = camMoveAnimIn;
                camAnimation.Play();
                gameResumed = true;
            }
        }

        if(gameResumed == true && gameResumeTime <= 0.0f)
        {
            gameCamera.enabled = true;
            laptopCamera.enabled = false;
            gameUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            gameResumed = false; gameResumeTime = 1.0f;
        }
        else if(gameResumed == true && gameResumeTime >= 0.0f)
        {
            gameResumeTime -= Time.deltaTime;
        }
    }
}
