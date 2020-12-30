using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Game : MonoBehaviour
{
    private enum phoneMenus {Play, Controls, Quit}
    private phoneMenus currentPhoneSelection = phoneMenus.Play;

    [SerializeField] private StoryData _data;
    [SerializeField] private GameObject backgroundCamera;
    [SerializeField] private GameObject laptopHinge;
    [SerializeField] private GameObject PhoneThumb;
    [SerializeField] private GameObject ArmObject;
    [SerializeField] private GameObject PhoneObject;

    [SerializeField]private AnimationClip[] PhoneMovements;

    [SerializeField] private Material[] phoneScreens;
    private  Material[] phoneMats = new Material[7];

    private TextDisplay _output;
    private BeatData _currentBeat;
    private WaitForSeconds _wait;

    private Animation camAnimation;
    private Animation LaptopCloseAnimation;
    private Animation ThumbAnimation;
    private Animation ArmAnimation;

    private float gameStartedTime = 1.0f;
    private bool gameStarted = false;
    private bool phoneActive = true;
    private bool controlsActive = false;

    private void Awake()
    {
        _output = GetComponentInChildren<TextDisplay>();
        _currentBeat = null;
        _wait = new WaitForSeconds(0.5f);
        camAnimation = backgroundCamera.GetComponent<Animation>();
        ThumbAnimation = PhoneThumb.GetComponent<Animation>();
        LaptopCloseAnimation = laptopHinge.GetComponent<Animation>();
        ArmAnimation = ArmObject.GetComponent<Animation>();
        phoneMats = PhoneObject.GetComponent<MeshRenderer>().materials;
    }

    private void Update()
    {
        if(phoneActive == true)
        {
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                ThumbAnimation.clip = PhoneMovements[(int)currentPhoneSelection];
                ThumbAnimation.Play();
                currentPhoneSelection++;
                if ((int)currentPhoneSelection == PhoneMovements.Length) currentPhoneSelection = 0;
            }
            if(Input.GetKeyDown(KeyCode.Return))
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
        else
        {
            if(_output.IsIdle)
            {
                if (_currentBeat == null)
                {
                    DisplayBeat(1);
                }
                else
                {
                    UpdateInput();
                }
            }

            if(_currentBeat.ID == 4)
            {
                StartCoroutine(AnimPause(camAnimation));
            }
            else if(_currentBeat.ID == 3)
            {
                StartCoroutine("Quit");
            }
        }
    }


    void phoneMenuSelection(phoneMenus phoneMenu)
    {
        switch (phoneMenu)
        {
            case phoneMenus.Play:
                StartCoroutine(AnimPause(ArmAnimation));
                phoneActive = false;
                break;
            case phoneMenus.Controls:
                phoneActive = false;
                controlsActive = true;
                phoneMats[0] = phoneScreens[1];
                PhoneObject.GetComponent<MeshRenderer>().materials = phoneMats;
                break;
            case phoneMenus.Quit:
                StartCoroutine(AnimPause(ArmAnimation));
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

        if( anim == camAnimation)
        {
            SceneManager.LoadSceneAsync("MainGame");
        }
    }

    private void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_currentBeat != null)
            {
                if (_currentBeat.ID == 1)
                {
                    Application.Quit();
                }
                else
                {
                    DisplayBeat(1);
                }
            }
        }
        else
        {
            KeyCode alpha = KeyCode.Alpha1;
            KeyCode keypad = KeyCode.Keypad1;

            for (int count = 0; count < _currentBeat.Decision.Count; ++count)
            {
                if (alpha <= KeyCode.Alpha9 && keypad <= KeyCode.Keypad9)
                {
                    if (Input.GetKeyDown(alpha) || Input.GetKeyDown(keypad))
                    {
                        ChoiceData choice = _currentBeat.Decision[count];
                        DisplayBeat(choice.NextID);
                        break;
                    }
                }

                ++alpha;
                ++keypad;
            }
        }
    }

    private void DisplayBeat(int id)
    {
        BeatData data = _data.GetBeatById(id);
        StartCoroutine(DoDisplay(data));
        _currentBeat = data;
    }

    private IEnumerator DoDisplay(BeatData data)
    {
        _output.Clear();

        while (_output.IsBusy)
        {
            yield return null;
        }

        _output.Display(data.DisplayText);

        while(_output.IsBusy)
        {
            yield return null;
        }
        
        for (int count = 0; count < data.Decision.Count; ++count)
        {
            ChoiceData choice = data.Decision[count];
            _output.Display(string.Format("{0}: {1}", (count + 1), choice.DisplayText));

            while (_output.IsBusy)
            {
                yield return null;
            }
        }

        if(data.Decision.Count > 0)
        {
            _output.ShowWaitingForInput();
        }
    }
}
