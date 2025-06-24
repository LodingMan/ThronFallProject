using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindPanel : MonoBehaviour
{
    [SerializeField]private List<InputFixButton> buttons = new();
    [SerializeField]private KeyBindConfirmPanel confirmPanel;
    List<InputBinding> info = new(); 


    public void OpenPanel()
    {
        info = SaveDataManager.SaveSettingData.InputSetting._inputBindings;

        foreach (var inputFixButton in buttons)
        {
            var binding = info.Find(i => i.actionName == inputFixButton.inputName);
            inputFixButton.lbText.text = $"{binding.key}";
        }
    }

    public void Refresh()
    {
        foreach (var inputFixButton in buttons)
        {
            var binding = info.Find(i => i.actionName == inputFixButton.inputName);
            inputFixButton.lbText.text = $"{binding.key}";
        }
    }

    public void OnClickBind(InputFixButton button)
    {
        var binding = info.Find(i => i.actionName == button.inputName);
        confirmPanel.OpenPanel(binding,Refresh);
    }
}
