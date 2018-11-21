using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialStart : MonoBehaviour
{
    public static TutorialStart Instance; 
    public GameObject TutorialPanel;
    public GameObject TutorialPanelSkill;
    public GameObject buySkillArrow;
    public GameObject chooseTankArrow;
    public GameObject upgradeArrow;
    public GameObject text;

    bool showTutorial = false;
     public bool showTutorial2 = true;

    private void Update()
    {
        showTutorial = PlayerStatistic.Instance.GetShowTutorial();
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
            TutorialStartMenu();
    }
    public void TutorialStartMenu()
    {

        if (PlayerStatistic.Instance.GetShowTutorial())
        {
            TutorialPanel.SetActive(true);
        }
    }

    public void AcctiveTutorialSkill()
    {
        if (showTutorial)
        {

            TutorialPanelSkill.SetActive(true);
            showTutorial = false;
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        buySkillArrow.SetActive(true);
    }

    public void AcctiveTutorialChooseTank()
    {
        if(showTutorial && showTutorial2)
        {
            showTutorial2 = false;
            TutorialPanel.SetActive(true);
            text.SetActive(true);
            chooseTankArrow.SetActive(true);
        }
    }
    public void AcctiveTutorialUpgradeTank()
    {
        if (showTutorial)
        {
            TutorialPanel.SetActive(true);
            text.SetActive(true);
            upgradeArrow.SetActive(true);

        }
    }
    public void ScenaTutorial()
    {
        if (showTutorial)
        {
            LevelLoader.Instance.LoadLevel(4);
        }
    }
}
