using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCameraToPlayer : MonoBehaviour
{

    void Start()
    {
        StartCoroutine("Wait");
        Transform player = FindObjectOfType<PlayerController>().transform;
        this.GetComponent<CinemachineVirtualCamera>().Follow = player;
        this.GetComponent<CinemachineVirtualCamera>().LookAt = player;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
    }
}
