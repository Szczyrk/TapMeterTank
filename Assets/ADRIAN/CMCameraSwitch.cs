using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCameraSwitch : MonoBehaviour
{
    public static CMCameraSwitch Instance;
    public GameObject[] cameraList;
    public GameObject currentCamera;
    private int currentCameraID;

    GameObject playerCamera;
    GameObject enemyCamera;

    void Start()
    {
        currentCameraID = 0;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this.gameObject);
    }

    public void NextCamera()
    {
        if (currentCameraID < cameraList.Length - 1)
        {
            currentCamera = cameraList[currentCameraID + 1];
            currentCamera.SetActive(true);
            cameraList[currentCameraID].SetActive(false);
            currentCameraID++;
        }
    }

}
