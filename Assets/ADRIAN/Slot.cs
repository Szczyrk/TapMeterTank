using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillHolder AttachedSkill { get; set; }
    public bool slotUnlocked = false;
    public int slotID = 0;
    public int slotUnlockCost = 100;
    public GameObject locker;

    public static List<SO_process> skillList;
    public static List<Slot> lockedSlots;

    void Start()
    {
        if (lockedSlots == null) lockedSlots = new List<Slot>();
        skillList = new List<SO_process>();
        slotUnlocked = PlayerStatistic.Instance.GetSlotUnlocked(slotID);

        if (slotUnlocked && transform.childCount > 0)
        {
            GameObject locker = transform.GetChild(0).gameObject;
            if (locker != null)
                locker.SetActive(false);
        }

        if (!slotUnlocked) lockedSlots.Add(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Make it bright

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void ApplyChoosedSkill()
    {
        if(AttachedSkill != null && AttachedSkill.ProcessSkill != null)
            skillList.Add(AttachedSkill.ProcessSkill);
    }

    public void BuySlot()
    {
        Slot slot = lockedSlots.Find(s => s.slotID == 1);
        if (!slot) slot = lockedSlots[0];

        if (CoinsPanel.Instance.CoinsAvailable() < slot.slotUnlockCost)
            return;

        CoinsPanel.Instance.CoinsSpent(slot.slotUnlockCost);
        slot.UnlockSlot();
        lockedSlots.Remove(slot);
    }

    public void ShowBuyPanel()
    {
        SkillUnlockPanel.Instance.ShowSlotUnlockMessagePanel(this);
    }

    public void UnlockSlot()
    {
        PlayerStatistic.Instance.SetSlotUnlocked(
            slotID);
        slotUnlocked = true;

        if (locker != null)
            locker.SetActive(false);
    }
}

    
