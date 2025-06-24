using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PopupFactory {
    public async UniTask CreatePopup<T>(string popupKey,Transform parent,Action<T> createComp) where T : BasePopup
    { 
        var task = await AddressablesManager.LoadAssetAsync<GameObject>(popupKey);
        if (task.Succeeded)
        {
            var popup = GameObject.Instantiate(task.Value,parent).GetComponent<T>();
            createComp.Invoke(popup);
        }

    }
    public void ClosePopup(BasePopup popup)
    {
        AddressablesManager.ReleaseAsset(popup.popupName);
        GameObject.Destroy(popup.gameObject);
    }
}

public class PopupController : MonoBehaviour
{
    public static PopupFactory Factory = new PopupFactory();
    public static PopupController Instance { get; private set; }
    public List<BasePopup> PopupList = new List<BasePopup>();
    public bool isOpenPopup => PopupList.Count != 0;
    
    public ReferenceScriptable ReferenceScriptable;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public void OpenPopup<T>(string popupName, Action<T> popupReady) where T: BasePopup
    {
        Debug.Log("PopupOpen in Controller");
        if (PopupList.Exists(popup => popup.popupName == popupName))
        {
            Debug.LogWarning($"Popup {popupName} is already open.");
            return;
        }
        Factory.CreatePopup<T>(popupName, transform,(popup) =>
        {
            popupReady.Invoke(popup);
        });
    }
    public void ClosePopup(BasePopup popup)
    {
        Factory.ClosePopup(popup);
    }

    public void OpenSettingPopup()
    {
        OpenPopup<PopupSetting>("PopupSetting", (popup) =>
        {
            popup.Initialize();
            popup.OpenPopup();

        });
; 
    }

    public void CloseLastOpenPopup()
    {
        PopupList[^1].ClosePopup();
    }


}
