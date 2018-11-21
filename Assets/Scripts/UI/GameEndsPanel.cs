using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndsPanel : MonoBehaviour
{
    public List<MoveUIPanel> panelsToHide;
    public MoveUIPanel playerLostPanel;

    [SerializeField]
    private SO_process playerWon;

    [SerializeField]
    private SO_process playerLost;

    [SerializeField]
    private GameObject panelToShow;

    private void Awake()
    {
        // Make something when match ends
        EventManager.SubscribeToEventMatchEnd(ShowAnimation);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeFromEventMatchEnd(ShowPanel);
        EventManager.UnsubscribeFromEventAllProcessesFinished(ShowPanel);
    }

    private void ShowAnimation(bool won)
    {
        foreach (MoveUIPanel panel in panelsToHide)
        {
            if(panel != null)
            panel.Hide();
        }

        // Subscribe to this event so this script will be noticed when its processed's finished
        EventManager.SubscribeToEventAllProcessesFinished(ShowPanel);

        // Run process
        if (won)
        {
            EventManager.RaiseEventProcessStarted(playerWon);
        }
        else
        {
            Invoke("ShowPlayerLostPanel", 3f);
            EventManager.RaiseEventProcessStarted(playerLost);
        }
    }

    private void ShowPlayerLostPanel()
    {
        if(playerLostPanel != null)
        playerLostPanel.Show();
    }

    private void ShowPanel(bool isMainProcessWorking, bool isMainPlayerProcessWorking, bool isMainEnemyProcessWorking)
    {
        //panelToShow.SetActive(true);

        // Unsubscribe
        EventManager.UnsubscribeFromEventAllProcessesFinished(ShowPanel);
    }

    private void ShowPanel(bool isMainProcessWorking)
    {
        //panelToShow.SetActive(true);

        // Unsubscribe
        EventManager.UnsubscribeFromEventAllProcessesFinished(ShowPanel);
    }
}
