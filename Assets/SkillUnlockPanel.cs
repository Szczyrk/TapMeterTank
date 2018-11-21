using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillUnlockPanel : MonoBehaviour
{
    public static SkillUnlockPanel Instance;
    public GameObject buySkillMessagePanel;
    public GameObject buySlotMessagePanel;
    public GameObject notEnoughtCoinsMessagePanel;
    private SkillHolder skillSelected;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void ShowSkillUnlockMessage(SkillHolder skill)
    {
        if (CoinsPanel.Instance.CoinsAvailable() < skill.skillCost)
        {
            notEnoughtCoinsMessagePanel.SetActive(true);
            notEnoughtCoinsMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Not enough coins! \n" +
                                                                        "You need " + skill.skillCost + " to unlock this skill!";
            return;
        }

        skillSelected = skill;
        buySkillMessagePanel.SetActive(true);

        buySkillMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Do you want to buy " + skill.skillType + " skill for " + skill.skillCost + " coins?";
    }

    public void BuySkill()
    {
        skillSelected.BuySkill();
        skillSelected = null;
    }

    public void ShowSlotUnlockMessagePanel(Slot slot)
    {
        if (CoinsPanel.Instance.CoinsAvailable() < slot.slotUnlockCost)
        {
            notEnoughtCoinsMessagePanel.SetActive(true);
            notEnoughtCoinsMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Not enough coins! \n" +
                                                                        "You need " + slot.slotUnlockCost + " to unlock this slot!";
            return;
        }

        buySlotMessagePanel.SetActive(true);

        buySlotMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Do you want to buy new slot for " + slot.slotUnlockCost + " coins?";
    }
}
