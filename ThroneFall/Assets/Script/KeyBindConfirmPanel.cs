using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyBindConfirmPanel : MonoBehaviour
{
    private bool isCommandInput = false;
    [SerializeField] TMP_Text lbCount;
    private InputBinding selectBindKey;
    private Action OnCompleteBind;

    public void OpenPanel(InputBinding binding, Action completeBind)
    {
        selectBindKey = binding;
        isCommandInput = false;
        this.gameObject.SetActive(true);
        StartCoroutine(AwaitCommand());
        OnCompleteBind = completeBind;
    }


    IEnumerator AwaitCommand()
    {
        float duration = 3f;
        while (!isCommandInput)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        if (key != KeyCode.Escape)
                        {
                            var currentBindKey =
                                SaveDataManager.SaveSettingData.InputSetting._inputBindings.Find(i =>
                                    i.key == selectBindKey.key);
                            currentBindKey.key = key;
                            isCommandInput = true;
                        }
                    }

                }
            }
            duration -= Time.deltaTime;
            if (duration < 0)
            {
                isCommandInput = true;
            }
            lbCount.text = $"{Mathf.Max(0, (int)duration)}";
            yield return null;
        }
        SaveDataManager.SaveSettings();
        OnCompleteBind?.Invoke();
        this.gameObject.SetActive(false);
    }
}
