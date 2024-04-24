using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectController : NetworkBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,1);
    }
}
