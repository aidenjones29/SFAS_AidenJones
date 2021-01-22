using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class GlobalVariables //Global Values for use in other scripts.
{
    public static bool menuActive;
    public static bool gamePaused;
    public static bool gameFinished = false;

    public static int trashCollected = 0;
    public static float timeLeft = 180;
}

public class PauseMenu : MonoBehaviour
{
    //--- Animation Objects ---
    [SerializeField] private Camera gameCamera;            //Main Game Camera
    [SerializeField] private Camera laptopCamera;          //The cam to change to when paused.
    [SerializeField] private GameObject backgroundCamera;  //camera with render texture to laptop screen.
    [SerializeField] private GameObject PhoneThumb;        //Thumb for phone animation.
    [SerializeField] private GameObject ArmObject;         
    [SerializeField] private GameObject PhoneObject;       
    [SerializeField] private GameObject laptopHinge;       //Laptop empty object for closing laptop.

    //--- UI Objects ---
    [SerializeField] private GameObject gameUI;
    [SerializeField] private Material[] phoneScreens;
    
    //--- Animation clips ---
    [SerializeField] private AnimationClip[] PhoneMovements; //Phone animations for transitioning selections.
    [SerializeField] private AnimationClip camMoveAnimOut;   //Anim clip for moving camera.
    [SerializeField] private AnimationClip camMoveAnimIn;
    private Animation camAnimation;
    private Animation ThumbAnimation;
    private Animation ArmAnimation;
    private Animation LaptopCloseAnimation;

    //--- Variable Setup ---
    private enum phoneMenus { Play, Controls, Quit }
    private phoneMenus currentPhoneSelection = phoneMenus.Play;

    private Material[] phoneMats = new Material[7]; //List of phone materials for getting screen material.

    private float gameResumeTime = 1.0f; //Timer to allow animation to play.

    private bool gameStarted = false;
    private bool phoneActive = true;
    private bool controlsActive = false;
    private bool gameResumed = false;

    // Start is called before the first frame update
    void Start()
    {
        camAnimation = laptopCamera.GetComponent<Animation>();
        laptopCamera.enabled = false;
        GlobalVariables.menuActive = false;
        if(SceneManager.GetActiveScene().name == "MainScene") GlobalVariables.gamePaused = true;
        camAnimation = backgroundCamera.GetComponent<Animation>();
        ThumbAnimation = PhoneThumb.GetComponent<Animation>();
        ArmAnimation = ArmObject.GetComponent<Animation>();
        phoneMats = PhoneObject.GetComponent<MeshRenderer>().materials;
        LaptopCloseAnimation = laptopHinge.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name != "MainScene")
        {
            gamePause();
        }

        if(gameResumed == true && gameResumeTime <= 0.0f) //Resume game after animation has played.
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

        if (phoneActive == true) //Pause all gameplay when the pause screen is active (Phone up)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ThumbAnimation.clip = PhoneMovements[(int)currentPhoneSelection];
                ThumbAnimation.Play();
                currentPhoneSelection++;
                if ((int)currentPhoneSelection == PhoneMovements.Length) currentPhoneSelection = 0;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                phoneMenuSelection(currentPhoneSelection);
            }
        }
        else if (controlsActive == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                phoneActive = true;
                controlsActive = false;
                phoneMats[0] = phoneScreens[0];
                PhoneObject.GetComponent<MeshRenderer>().materials = phoneMats;
            }
        }
    }


    public void gamePause()
    {
        GlobalVariables.menuActive = !GlobalVariables.menuActive;

        if (GlobalVariables.menuActive)
        {
            gameCamera.enabled = false;
            laptopCamera.enabled = true;
            phoneActive = true;
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

    void phoneMenuSelection(phoneMenus phoneMenu)
    {
        switch (phoneMenu)
        {
            case phoneMenus.Play:
                GlobalVariables.gamePaused = false;
                if(SceneManager.GetActiveScene().name == "MainScene") StartCoroutine(AnimPause(ArmAnimation));
                else gamePause();
                phoneActive = false;
                break;
            case phoneMenus.Controls:
                phoneActive = false;
                controlsActive = true;
                phoneMats[0] = phoneScreens[1];
                PhoneObject.GetComponent<MeshRenderer>().materials = phoneMats;
                break;
            case phoneMenus.Quit:
                Application.Quit();
                break;
        }
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(4);
        LaptopCloseAnimation.Play();
        StartCoroutine("CloseAnim");
    }

    IEnumerator CloseAnim()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("HouseClean");
    }

    IEnumerator AnimPause(Animation anim)
    {
        anim.Play();
        yield return new WaitForSeconds(1);
    }
}
