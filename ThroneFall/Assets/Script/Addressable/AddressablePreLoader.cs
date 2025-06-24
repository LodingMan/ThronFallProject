using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablePreLoader : MonoBehaviour
{
    [SerializeField] ReferenceScriptable referenceScriptable;

    private void Awake()
    {
         AddressablesManager.Initialize();

        foreach (var referencePair in referenceScriptable.ReferenceList)
        {
            AddressablesManager.LoadAsset<GameObject>(referencePair.Reference);
        }
        var task = AddressablesManager.LoadAssetsAsync<Sprite>("Sprite");
    }
}
