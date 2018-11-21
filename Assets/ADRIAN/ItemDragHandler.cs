using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{

    private PointerEventData pointerEventData;
    private List<RaycastResult> results;
    private List<Slot> slotsInResults;
    private GraphicRaycaster raycaster;
    private Slot appendedSlot;

    // Position on start
    private Vector2 initPosition;
    bool CheckBuy;
    bool dragEnabled = false;


    private void Awake()
    {
        slotsInResults = new List<Slot>();
        appendedSlot = null;
        raycaster = GetComponentInParent<GraphicRaycaster>();
        results = new List<RaycastResult>();
        pointerEventData = new PointerEventData(EventSystem.current);
        initPosition = transform.position;
    }

    

    public void OnDrag(PointerEventData eventData)
    {
        if (dragEnabled)
            transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragEnabled) return;

        //Clear lists
        slotsInResults.Clear();
        results.Clear();
        Raycast();
        if (appendedSlot)
        {
            appendedSlot.AttachedSkill = null;
            appendedSlot = null;
        }

        //Get all slots available in raycast
        foreach (RaycastResult result in results)
            if (result.gameObject.GetComponent<Slot>() != null)
                slotsInResults.Add(result.gameObject.GetComponent<Slot>());

        //If there is any slot under
        if (slotsInResults.Count > 0)
        {
            transform.position = initPosition;
                
                foreach(var slot in slotsInResults)
                {
                    //If slot has attached skill and slot is unlocked
                    if (slot.AttachedSkill != null || !slot.slotUnlocked)
                    {
                        return;
                    }
 
                    //Append skill if empty slot and ItemDragHandler object has Skill script
                    else if (GetComponent<SkillHolder>() != null)
                    {
                        //Detach skill from previous slot if appended
                        if (appendedSlot != null)
                            appendedSlot.AttachedSkill = null;

                        //Attach skill to slot
                        slot.AttachedSkill = GetComponent<SkillHolder>();
                        appendedSlot = slot;

                        //Position skill inside slot
                        transform.position = slot.transform.position;

                        return;
                    }

                }
        }

        //There is no slot under - detach slot if appended
        else
        {
            if(appendedSlot != null)
            {
                appendedSlot.AttachedSkill = null;
                appendedSlot = null;
            }

            transform.position = initPosition;
        }
    }

    private void Raycast()
    {
        pointerEventData.position = Input.mousePosition;
        raycaster.Raycast(pointerEventData, results);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetAsLastSibling();
        if (PlayerStatistic.Instance.GetSkillAmount(GetComponent<SkillHolder>().skillType) > 0)
            dragEnabled = true;
    }
}
