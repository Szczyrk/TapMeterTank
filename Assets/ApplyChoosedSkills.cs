using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyChoosedSkills : MonoBehaviour
{
    public List<SO_process> skillsProcesses;
    public List<GameObject> skillsPrefabs;

    private void Start()
    {
        if (Slot.skillList == null)
            return;
        for (int i = 0; i < Slot.skillList.Count; i++)
        {
            Transform skillHolder = transform.GetChild(i);
            Instantiate(skillsPrefabs[skillsProcesses.FindIndex(t => t == Slot.skillList[i])], skillHolder);
        }
    }
}
