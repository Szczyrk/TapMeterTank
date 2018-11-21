using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class MatchPlayer : MatchProgressChangingObject, IPointerDownHandler
{

    [SerializeField]
    private AudioClip hitSound;

    // METHODS

    #region OnPointerCallbacks
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPaused)
        {
            // Play sound            
            Helper.PlayAudioIfSoundOn(hitSound);

            // Increase progress
            Attack(this);
          
        }
    }
    #endregion

    new private void Awake()
    {
        base.Awake();

        ChangeStatistic();
    }

    void ChangeStatistic()
    {
        statistics.RemoveAll(statistics => statistics.SO_Statistic);
        List<Statistic> statisticsPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Tank>().Statistics;
        for (int i = 0; i < statisticsPlayer.Count; i++)
        {
            Statistics.Add(statisticsPlayer[i]);
        }
    }
}
