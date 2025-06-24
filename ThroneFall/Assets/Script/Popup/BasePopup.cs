using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public string popupName = string.Empty;
    
    public virtual void OpenPopup()
    {
        if (PopupController.Instance.PopupList.Find(p => p == this) != null)
        {
            Destroy(this);
            return;
        }
        AudioController.instance.PlaySound("Button_Click", SoundConfig.SoundType.Effect);
        PopupController.Instance.PopupList.Add(this);
        Debug.Log("PopupOpen in BasePopup");
    }
    public virtual void ClosePopup()
    {
        PopupController.Instance.PopupList.Remove(this);
        AudioController.instance.PlaySound("Button_Click", SoundConfig.SoundType.Effect);
        Destroy(gameObject);
    }


}
