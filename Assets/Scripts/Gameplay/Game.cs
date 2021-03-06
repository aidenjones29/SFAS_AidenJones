﻿using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private StoryData _data;
    [SerializeField] private GameObject backgroundCamera;
    [SerializeField] private GameObject laptopHinge;
    [SerializeField] private GameObject unityUI;

    private TextDisplay _output;
    private BeatData _currentBeat;
    private WaitForSeconds _wait;

    private Animation camAnimation;
    private Animation LaptopCloseAnimation;

    private void Awake()
    {
        _output = GetComponentInChildren<TextDisplay>();
        _currentBeat = null;
        _wait = new WaitForSeconds(0.5f);
        camAnimation = backgroundCamera.GetComponent<Animation>();
        LaptopCloseAnimation = laptopHinge.GetComponent<Animation>();
    }

    private void Update()
    {
        if(GlobalVariables.gamePaused == false)
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
                unityUI.SetActive(true);
                StartCoroutine(AnimPause(camAnimation));
            }
            else if(_currentBeat.ID == 3)
            {
                StartCoroutine("Quit");
            }

            if(_currentBeat.ID == 2 && SceneManager.GetActiveScene().name == "GameEnding")
            {
                SceneManager.LoadScene(0);
            }
            if (_currentBeat.ID == 3 && SceneManager.GetActiveScene().name == "GameEnding")
            {
                Application.Quit();
            }
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
