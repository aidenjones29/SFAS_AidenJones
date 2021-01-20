using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    [SerializeField] private TMP_Text UITimeValue;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.timeLeft = 180;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GlobalVariables.gameFinished)
        {
            GlobalVariables.timeLeft -= Time.deltaTime;
            UITimeValue.text = GlobalVariables.timeLeft.ToString("F1");
        }

        if (GlobalVariables.timeLeft <= 0.0)
        {
            GlobalVariables.gameFinished = true;
        }
    }
}
