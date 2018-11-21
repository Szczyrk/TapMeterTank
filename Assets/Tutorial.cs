using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MatchController
{
    public PrestartAnimation prestart;
    public GameObject Click;
    public GameObject ClickText;
    public GameObject Arrow;
    public GameObject Arrow2;
    public GameObject Pres;
    public GameObject Pres2;
    public GameObject Hand;
    public MatchController controller;
    public SetOnDrop drop;
    public GameObject Skills;
    public MatchProgressChangingObject Enemy;
    public SO_process skillProcess;
    public float duration = 5f;


    public TextMeshProUGUI text;
    public List<string> texts;

    bool first = true;
    bool secend = true;

    public override void Attack(MatchProgressChangingObject matchProgressChangingObject)
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization


    void Start () {
        text.text = texts[0];
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (prestart.end && first)
        {
            first = false;
            controller.PauseMatch();
            Click.SetActive(true);
            Hand.SetActive(false);
            Arrow.SetActive(true);
            Pres.SetActive(true);
            text.text = texts[1];
        }
        if (drop.end && secend)
        {
            secend = false;
            controller.ResumeMatch();
            Enemy.Statistics[0].Value = 20;
            controller.Attack(Enemy);
            text.text = texts[3];
            Skills.SetActive(false);
            Arrow2.SetActive(false);
            controller.PauseMatch();
            StartCoroutine(Wait());
        }
    }

    public void UseskillMessage()
    {
        text.text = texts[2];
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        text.text = texts[4];
        EventManager.RaiseEventProcessStarted(skillProcess, MatchProgressChangingObject.Type.Enemy);
        StartCoroutine(WaitForEnemySkill());

    }
    IEnumerator WaitForEnemySkill()
    {
        yield return new WaitForSeconds(duration);
        text.text = texts[5];
        ClickText.SetActive(true);
        Pres2.SetActive(true);
        player.Statistics[0].Value = 30;
    }
}
