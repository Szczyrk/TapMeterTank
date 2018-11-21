using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAcctiveTutorial : MonoBehaviour
{

    Toggle toggle;

    // Use this for initialization
    void Start()
    {
        toggle = GetComponent<Toggle>();
        if (PlayerStatistic.Instance.GetShowTutorial())
            toggle.isOn = true;
        else
            toggle.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            PlayerStatistic.Instance.SetDisableShowTutorial(1);
            TutorialStart.Instance.showTutorial2 = true;
        }
        else
            PlayerStatistic.Instance.SetDisableShowTutorial(0);
    }
}
