using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsPanel : MonoBehaviour
{
    public static CoinsPanel Instance;

    TextMeshProUGUI coinText;
    private int coinsAvailable;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start ()
    {
        coinsAvailable = PlayerStatistic.Instance.GetCoinsAmount();

        coinText = GetComponentInChildren<TextMeshProUGUI>();
        coinText.text = ""+coinsAvailable;
	}

    public int CoinsAvailable()
    {
        return coinsAvailable;
    }

    public void CoinsSpent(int amount)
    {
        coinsAvailable -= amount;
        PlayerStatistic.Instance.SetCoinsAmount(coinsAvailable);

        coinText.text = "" + coinsAvailable;
    }
}
