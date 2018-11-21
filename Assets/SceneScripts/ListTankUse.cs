using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTankUse : MonoBehaviour 
{

    [SerializeField]
    public List<GameObject> tanks = new List<GameObject>();

   
    public List<GameObject> tanksOnScene = new List<GameObject>();

    public List<GameObject> Camers = new List<GameObject>();

    [SerializeField]
    List<GameObject> Background = new List<GameObject>();

    [SerializeField]
    List<GameObject> SpawnPositons = new List<GameObject>();

    public float hightTank;


    GameObject SpawnPositon;
    private int r = 1;
    public static int numSpawned = 0;
    public Vector3 spacing_between_objects;
    public GameObject cam;

    void Start () {
        numSpawned = 0;
    }




    void SpawnRandomObject()
    {
        //spawns item in array position between 0 and 100
        int whichItem = Random.Range(0, Background.Count);

        GameObject myObj = Instantiate(tanks[numSpawned]) as GameObject;
        GameObject myBackground = Instantiate(Background[whichItem]) as GameObject;

        myObj.name = tanks[numSpawned].name;
        myObj.transform.SetParent(SpawnPositon.transform);
        myBackground.transform.position = SpawnPositon.transform.position;
        myBackground.transform.LookAt(SpawnPositon.transform, new Vector3(0, 0, 0));
        SpawnPositon.transform.position = myBackground.transform.Find("Road").position;
        myObj.transform.position = new Vector3(SpawnPositon.transform.position.x, hightTank, SpawnPositon.transform.position.z);
        SpawnPositon.AddComponent<TankStartOption>();
        tanksOnScene.Add(myObj);

        var go = Instantiate(cam);
        go.name = "Camera";
        Camers.Add(go);
    
        Cinemachine.CinemachineVirtualCamera cinemachine = Camers[numSpawned].GetComponent<Cinemachine.CinemachineVirtualCamera>();
        //cinemachine.m_Lens.FieldOfView = fieldOfViewCamera;
        cinemachine.m_Follow = myObj.transform;
       // cinemachine.m_LookAt = myObj.transform;
        Camers[numSpawned].SetActive(false);

        numSpawned++;
    }

    void Update()
    {
        r *= -1;
        if (tanks.Count > numSpawned)
        {
            for(int i = 0; i<tanks.Count;i++)
            {
                Tank tank = tanks[i].GetComponent<Tank>();
                if (PlayerStatistic.Instance.GetTankLevel(tank.tankType) >= 0)
                {
                    tanks[i].GetComponent<Tank>().level = PlayerStatistic.Instance.GetTankLevel(tank.tankType);
                }
            }
            //where your instantiated object spawns from
            SpawnPositon = SpawnPositons[numSpawned];
            SpawnPositon.transform.position += spacing_between_objects;
            SpawnRandomObject();
        }
    }
}
