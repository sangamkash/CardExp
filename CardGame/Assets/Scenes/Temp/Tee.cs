using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tee : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(gameObject.name);
    }
}
