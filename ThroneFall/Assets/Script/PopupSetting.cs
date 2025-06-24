using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static GameConfig;



public class PopupSetting : BasePopup 
{
    public Slider BgmSlider;
    public Slider EffectSlider;
    public TMP_Dropdown DrResolution;
    public Toggle tglFullScreen;
    public Toggle tglWindowScreen;
    public Resolution CurrentResolution;
    public KeyBindPanel KeyBindPanel;
    public GameObject ResetConfirmPanel;

    public void Initialize()
    {
        BgmSlider.value = SoundConfig.BgmVolume;
        EffectSlider.value = SoundConfig.EffectVolume;
        tglFullScreen.SetIsOnWithoutNotify(PlayerPrefs.GetInt("FullScreen",0) != 0);
        tglWindowScreen.SetIsOnWithoutNotify(PlayerPrefs.GetInt("FullScreen",0) == 0);
        
        List<string> options = GameResolutionDatas
            .Select(r => $"{r.width} x {r.height}")
            .ToList();
        DrResolution.ClearOptions();
        DrResolution.AddOptions(options);
        DrResolution.SetValueWithoutNotify(GameConfig.ResolutionIndex);

        CurrentResolution = GameConfig.GameResolutionDatas[ResolutionIndex];

    }
    public override void OpenPopup()
    {
        base.OpenPopup();
    }

    public override void ClosePopup()
    {
        base.ClosePopup();
    }

    public void OnChangeResolution(int index)
    {
        DrResolution.value = index;
        GameConfig.SetResolution(index);
        CurrentResolution = GameResolutionDatas[index];
    }

    public void OnValueChangeBgmVolume(float value)
    {
        BgmSlider.value = value;
        SoundConfig.SetVolume(SoundConfig.SoundType.Bgm ,value); 
    }
    public void OnValueChangeEffectVolume(float value)
    {
        EffectSlider.value = value;
        SoundConfig.SetVolume(SoundConfig.SoundType.Effect ,value); 
    }

    public void OnCheckFullScreen()
    {
        Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, true);
        PlayerPrefs.SetInt("FullScreen", 1);
    }

    public void OnCheckWindowScreen()
    {
        Screen.SetResolution(CurrentResolution.width, CurrentResolution.height, false);
        PlayerPrefs.SetInt("FullScreen", 0);
        
 
    }
    public void OpenKeyBindPanel()
    {
        KeyBindPanel.gameObject.SetActive(true);
        KeyBindPanel.OpenPanel();
    }

    public void OnClickSaveReset()
    {
        SaveDataManager.ClearSave();
        ResetConfirmPanel.SetActive(true);
    }

    
}
