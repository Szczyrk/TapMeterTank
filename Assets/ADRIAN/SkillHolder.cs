using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    public SO_process ProcessSkill;
    public Skill.SkillType skillType;

    public int skillCost = 15;
    public int skillAmount;

    public Sprite unlockedSprite;


    private void Start()
    {
        skillAmount = PlayerStatistic.Instance.GetSkillAmount(skillType);
        transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "" + skillAmount;

        if (skillAmount < 1)
            transform.GetChild(1).gameObject.SetActive(false);
        else
            GetComponent<Image>().sprite = unlockedSprite;

    }

    public void ShowBuyPanel()
    {
        SkillUnlockPanel.Instance.ShowSkillUnlockMessage(this);
    }

   public void BuySkill()
    {
        if(skillAmount < 1)
            GetComponent<Image>().sprite = unlockedSprite;

        CoinsPanel.Instance.CoinsSpent(skillCost);
        skillAmount++;
        PlayerStatistic.Instance.SetSkillAmount(skillType, skillAmount);

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "" + skillAmount;
    }
}
