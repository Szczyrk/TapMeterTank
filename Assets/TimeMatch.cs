using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeMatch : MonoBehaviour {

    public static TimeMatch Instance;
    TextMeshProUGUI text;
    public TextMeshProUGUI moneyText;
   public  GameObject Star1;
   public GameObject Star2;
   public GameObject Star3;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
    }
    private float timer = 0;
    // Use this for initialization
    void Start () {
        text = GetComponent<TextMeshProUGUI>();

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
    }

   public void Stop()
    {
        if (timer * 1000 < 5000)
        {
            Star1.SetActive(false);
            Star2.SetActive(false);
            Star3.SetActive(false);
        }
        if (timer * 1000 < 10000)
        {
            Star1.SetActive(false);
            Star2.SetActive(false);
        }
        if (timer * 1000 < 20000)
        {
            Star1.SetActive(false);
        }
        string minSec = string.Format("{0}:{1:00}:{2:000}", (int)timer / 60, (int)timer % 60, (timer * 1000) % 1000);
        text.text = minSec;
        int money;
        money = (1000 /(int)timer);
        if (money < 0)
            money = 5;
        moneyText.text = money.ToString();
        PlayerStatistic.Instance.SetCoinsAmount(PlayerStatistic.Instance.GetCoinsAmount() + money);
    }
}
