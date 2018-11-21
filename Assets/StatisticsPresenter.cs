using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsPresenter : MonoBehaviour
{
    public static StatisticsPresenter Instance;

    public List<Image> sliderValueImages;
    public float maxValue = 10f;

    public TextMeshProUGUI tankNameText;
    public TextMeshProUGUI tankLevelText;

    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI upgradeCostText;
    public List<Sprite> upgradeBttnSprites;
    public GameObject upgradeBttn;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetStatistics(Tank tank)
    {
        if (tank.level + 1 < tank.TankLevels.Count)
        {
            if (tank.level == 0)
            {
                upgradeText.text = "Buy new tank!";
                upgradeBttn.GetComponent<Image>().sprite = upgradeBttnSprites[2];
            }
            else
            {
                upgradeText.text = "Upgrade to next level!";
                upgradeBttn.GetComponent<Image>().sprite = upgradeBttnSprites[0];
            }

            upgradeCostText.text = "Cost: " + tank.TankLevels[tank.level].costToLvlUp;
            upgradeBttn.GetComponent<Button>().interactable = true;
        }
        else
        {
            upgradeText.text = "Max level reached!";
            upgradeCostText.text = "";
            upgradeBttn.GetComponent<Button>().interactable = false;
            upgradeBttn.GetComponent<Image>().sprite = upgradeBttnSprites[1];
        }


        tankNameText.text = tank.name;
        tankLevelText.text = "Level: " + tank.level;

        sliderValueImages[0].fillAmount = tank.TankLevels[tank.level].power / maxValue;
        sliderValueImages[2].fillAmount = tank.TankLevels[tank.level].speed / maxValue;
        sliderValueImages[4].fillAmount = tank.TankLevels[tank.level].weight / maxValue;

        if (tank.level + 1 < tank.TankLevels.Count)
        {
            sliderValueImages[1].fillAmount = tank.TankLevels[tank.level + 1].power / maxValue;
            sliderValueImages[3].fillAmount = tank.TankLevels[tank.level + 1].speed / maxValue;
            sliderValueImages[5].fillAmount = tank.TankLevels[tank.level + 1].weight / maxValue;
        }
        else
        {
            sliderValueImages[1].fillAmount = 0;
            sliderValueImages[3].fillAmount = 0;
            sliderValueImages[5].fillAmount = 0;
        }
    }
}
