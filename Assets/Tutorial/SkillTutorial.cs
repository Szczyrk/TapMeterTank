using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTutorial : MonoBehaviour {

    public SO_process skillProcess;
    public TextMeshProUGUI delayText;

    public float skillUsageDelay = 5f;
    float timeToUseAgain;

    public int skillAmount = 1;


    private void Start()
    {
        transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + skillAmount;
    }

    private void Update()
    {
        if (timeToUseAgain > 0)
        {
            timeToUseAgain -= Time.deltaTime;
            delayText.text = "" + (int)timeToUseAgain;
        }
        else
            delayText.text = "";
    }

    public void UseSkill()
    {
        if (skillAmount < 1 || timeToUseAgain > 0) return;
        Debug.Log("SkillUsed");

        timeToUseAgain = skillUsageDelay;
        skillAmount--;
        transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + skillAmount;
        EventManager.RaiseEventProcessStarted(skillProcess, MatchProgressChangingObject.Type.Player);
    }
}
