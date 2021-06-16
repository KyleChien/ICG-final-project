using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pill : MonoBehaviour
{
    void Start()
    {
        this.Invoke("DestroySelf", 5f);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
