using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuidePanel : MonoBehaviour,IInputSubscriber
{
    [SerializeField]private TMP_Text lbText;

    private void Start()
    {
    }

    public void SetPanel(int phase)
    {
        switch (phase)
        {
            case 0:
                this.gameObject.SetActive(false);
                break;
            case 1:
                this.gameObject.SetActive(true);
                lbText.text = "Town�� ������ �����Ͽ� \n��ȣ�ۿ��� ���� Town�� �Ǽ��ϼ���.";
                break;
            case 2:
                this.gameObject.SetActive(true);
                lbText.text = "Town�� ������ �����Ͽ� \n��ȣ�ۿ��� ���� Town�� �Ǽ��ϼ���.";
                break;
            case 3:
                this.gameObject.SetActive(true);
                var key = SaveDataManager.SaveSettingData.InputSetting._inputBindings.Find(input => input.actionName == "CallUp").key;
                lbText.text = $"{key}Ű�� ���� ������ �̵���ų �� �ֽ��ϴ�";
                InputManager.RegisterInput(this);
                break;
            case 4:
                this.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnInput(InputInfo info)
    {
        if (info.actionName == "CallUp" && info.inputType == GameEnums.EInputType.Down)
        {
            GetComponentInParent<GuidePhaseHandler>().NextPhase();
            InputManager.UnRegisterInput(this);
        }
    }
}
