using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingStart : MonoBehaviour
{
    public static SettingStart Instance;

    Cinemachine.CinemachineVirtualCamera cinemachine;
    public ListTankUse spawnSettings;
    int tankFollow;
    bool change;
    public GameObject cam;
    GameObject SelectGameObject;
    public float timeChangeCamera;

    public GameObject Accept;
    public GameObject Buy;
    public TankBuyPanel buyPanel;



    public GameObject SkillsMenu;
    public StatisticsPresenter statistics;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if (PlayerStatistic.Instance.GetLevelGame() == 0)
            LevelLoader.Instance.LoadLevelName("Tutorial");

        cinemachine = GetComponent<Cinemachine.CinemachineVirtualCamera>();
        change = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (change)
        {
            cam.SetActive(true);

            change = false;
            StartCoroutine(CameraChange());

            TankBuyPanel.Instance.tankSelected(spawnSettings.tanksOnScene[tankFollow].GetComponent<Tank>());
            StatisticsPresenter.Instance.SetStatistics(spawnSettings.tanksOnScene[tankFollow].GetComponent<Tank>());
        }

    }

    IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(timeChangeCamera);
    
        spawnSettings.Camers[tankFollow].SetActive(true);
    }

    public void Next()
    {
        if (tankFollow < spawnSettings.tanksOnScene.Count - 1)
        {
            spawnSettings.Camers[tankFollow].SetActive(false);
            change = true;
            tankFollow++;
        }
        else
        {
            spawnSettings.Camers[tankFollow].SetActive(false);
            change = true;
            tankFollow = 0;
        }
        Debug.Log(tankFollow);
 
    }


    public void Previous()
    {
        if (tankFollow > 0)
        {
            spawnSettings.Camers[tankFollow].SetActive(false);
            change = true;
            tankFollow -= 1;
        }
        else
        {
            spawnSettings.Camers[tankFollow].SetActive(false);
            change = true;
            tankFollow = spawnSettings.tanksOnScene.Count - 1;
        }
    }

    public void Choose()
    {
        SkillsMenu.SetActive(true);
    }

    public void StartGame()
    {
        SelectGameObject = Instantiate(spawnSettings.tanksOnScene[tankFollow]) as GameObject;
        SelectGameObject.tag = "Choose";
        //Debug.Log(SelectGameObject);
        DontDestroyOnLoad(SelectGameObject);

        int sceneID = MenagerLevelsPlayer.Instance.Levels[PlayerStatistic.Instance.GetLevelGame()].scene;
        LevelLoader.Instance.LoadLevel(sceneID);

    }
}
