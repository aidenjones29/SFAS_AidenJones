using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class GlobalVariables
{
    public static bool menuActive;
    public static bool gamePaused;
}

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    [SerializeField] private Camera laptopCamera;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject backgroundCamera;
    [SerializeField] private GameObject PhoneThumb;
    [SerializeField] private GameObject ArmObject;
    [SerializeField] private GameObject PhoneObject;
    [SerializeField] private GameObject laptopHinge;

    [SerializeField] private AnimationClip[] PhoneMovements;
    [SerializeField] private AnimationClip camMoveAnimOut;
    [SerializeField] private AnimationClip camMoveAnimIn;
    
    [SerializeField] private Material[] phoneScreens;
    
    private enum phoneMenus { Play, Controls, Quit }
    private phoneMenus currentPhoneSelection = phoneMenus.Play;

    private Animation camAnimation;
    private Animation ThumbAnimation;
    private Animation ArmAnimation;
    private Animation LaptopCloseAnimation;

    private Material[] phoneMats = new Material[7];

    private float gameStartedTime = 1.0f;
    private float gameResumeTime = 1.0f;
    
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

        if (phoneActive == true)
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
                StartCoroutine(Quit());
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

        if (anim == camAnimation)
        {
            SceneManager.LoadSceneAsync("MainGame");
        }
    }
}
