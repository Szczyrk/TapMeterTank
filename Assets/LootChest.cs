using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LootChest : MonoBehaviour
{
    public float angVelForceMax = 7f;
    public GameObject openedParticle;
    public GameObject groundHitParticle;

    Rigidbody _rb;
    bool interactable = false;
    bool chestOpened = false;
    float timeThreshold = 0.2f;
    float lastButtonPress = 0f;
    int timesToClick = 3;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!interactable) return;

        lastButtonPress += Time.deltaTime;
        if (lastButtonPress > timeThreshold)
            timesToClick = 3;

        if (timesToClick < 1 && !chestOpened)
            ChestOpened();

    }

    void ChestOpened()
    {
        chestOpened = true;
        openedParticle.SetActive(true);
        this.GetComponent<MeshRenderer>().enabled = false;
        GetComponentInParent<FallingChest>().ChestOpened();
    }

    private void OnCollisionEnter(Collision collision)
    {
        interactable = true;
        if (groundHitParticle != null)
            groundHitParticle.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!interactable) return;
        timesToClick--;
        lastButtonPress = 0;
        Vector3 angularVelocity = new Vector3(Random.Range(-angVelForceMax, angVelForceMax), 0 , Random.Range(-angVelForceMax, angVelForceMax));
        _rb.angularVelocity = angularVelocity;
    }
}
