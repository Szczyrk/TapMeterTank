using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankBuyPanel : MonoBehaviour
{
    public static TankBuyPanel Instance;
    public GameObject buyTankMessagePanel;
    public GameObject notEnoughtCoinsMessagePanel;
    public GameObject buyTankBttn;
    public GameObject acceptTank;
    private Tank selectedTank;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void tankSelected(Tank tank)
    {
        if (tank == null) return;

        selectedTank = tank;


        if (tank.level == 0)
        {
            acceptTank.SetActive(false);
            buyTankBttn.SetActive(true);
        }
        else if (tank.level + 1 >= tank.TankLevels.Count)
        {
            acceptTank.SetActive(true);
            buyTankBttn.SetActive(false);
        }
        else
        {
            acceptTank.SetActive(true);
            buyTankBttn.SetActive(false);
        }
    }

    public void ShowTankUnlockMessage()
    {
        if (selectedTank == null) return;

        if (selectedTank.level + 1 >= selectedTank.TankLevels.Count) return;

        int costToLvlUp = selectedTank.TankLevels[selectedTank.level].costToLvlUp;

        if (CoinsPanel.Instance.CoinsAvailable() < costToLvlUp)
        {
            notEnoughtCoinsMessagePanel.SetActive(true);
            notEnoughtCoinsMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Not enough coins! \nYou need " +
                                                                                            costToLvlUp + " to unlock this Tank!";
            return;
        }

        buyTankMessagePanel.SetActive(true);

        buyTankMessagePanel.GetComponentInChildren<TextMeshProUGUI>().text = "Do you want to buy " + selectedTank.tankType + 
                                                                                " Tank for " + costToLvlUp + " coins?";
    }

    public void LevelUpSelectedTank()
    {
        selectedTank.LevelUp();
    }
}
