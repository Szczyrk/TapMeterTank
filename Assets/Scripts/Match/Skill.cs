using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public SO_process skillProcess;
    public SkillType skillType;

    public Sprite inactiveSprite;
    public TextMeshProUGUI delayText;
    public float skillUsageDelay = 5f;

    Sprite activeSprite;
    float timeToUseAgain;
    Image image;

    [HideInInspector]
    public int skillAmount = 0;

    public enum SkillType { Shoot, TowerSpin, Granade, MolotovCoctail, BombDrop}

    private void Start()
    {
        image = GetComponent<Image>();
        activeSprite = image.sprite;
        skillAmount = PlayerStatistic.Instance.GetSkillAmount(skillType);
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
        {
            image.sprite = activeSprite;
            delayText.text = "";
        }
    }

    public void UseSkill()
    {
        if (skillAmount < 1 || timeToUseAgain > 0) return;

        image.sprite = inactiveSprite;
        timeToUseAgain = skillUsageDelay;
        skillAmount--;
        transform.GetComponentInChildren<TextMeshProUGUI>().text = "" + skillAmount;
        PlayerStatistic.Instance.SetSkillAmount(skillType, skillAmount);
        EventManager.RaiseEventProcessStarted(skillProcess, MatchProgressChangingObject.Type.Player);
    }
}