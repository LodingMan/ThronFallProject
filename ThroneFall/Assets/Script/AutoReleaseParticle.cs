using UnityEngine;
using System;
using System.Collections;

public class AutoReleaseParticle : MonoBehaviour
{
    private ParticleSystem ps;
    PoolObject poolObject;
    public Action onComplete;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        poolObject = GetComponent<PoolObject>();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEnd());
    }

    private IEnumerator WaitForEnd()
    {
        if (ps == null) yield break;

        ps.Play();

        while (ps.IsAlive(true))
            yield return null;

        ObjectPooler.ReturnPool(poolObject);
        //onComplete?.Invoke();
    }
}