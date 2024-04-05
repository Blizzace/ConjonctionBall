using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyifnotowned : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (HasStateAuthority == false)
        {
            Destroy(gameObject);
            return;
        }
    }


}