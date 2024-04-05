using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy1sec : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.gameObject, 1);
    }
}

