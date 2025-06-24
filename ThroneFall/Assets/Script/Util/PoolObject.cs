using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PoolObject : MonoBehaviour
{
    public string objectName;
    public bool isOn = false;

    private void Start()
    {
    }

    public void CompleteUse()
    {
        StartCoroutine(WaitReturn());
    }

    IEnumerator WaitReturn()
    {
        yield return new WaitForSeconds(1f);
        this.transform.position = Vector3.zero;
        ObjectPooler.ReturnPool(this);
    }

}
