using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownBuyProgress : MonoBehaviour,
    IInitialize<int>
{
    private int _townPrice = 0;
    private int _progressCurrentCoin = 0;
    private float _progressElapsed = 0f;
    private Slider _progressSlider;
    [SerializeField] private TMP_Text lbProgressText;

    private void Awake()
    {
        _progressSlider = GetComponent<Slider>();
    }

    public void Initialize(int townPrice)
    {
        _townPrice = townPrice;
        _progressCurrentCoin = 0; 
        _progressSlider.maxValue = _townPrice;
        lbProgressText.text = $"{_progressCurrentCoin}/{_townPrice}";
        SetActive(false);
    }
    public void UpdateProgress(InputInfo info, bool isLackCoin)
    {
        // 가격이 3원이고 3초에 게이지가 만땅으로 차야되면 1초에 value가 1증가해야되잖아
        // 그니까 ProgressTime/TownPreice이 1초에 증가야야되는 양인거지. 
        // price = 10 , buyprogressTiem = 3 이면 3/10 = 0.3 0.3초에 1씩 증가해야되는거임.
        if (info.inputType == GameEnums.EInputType.Down)
        {
            _progressCurrentCoin = 0;
            SetActive(true);

        }
        if (info.inputType == GameEnums.EInputType.Press && !isLackCoin)
        {
            var progressTick = GameConfig.TOWN_BUY_DURATION / _townPrice;
            _progressCurrentCoin = (int)(info.deltaTime / progressTick);
            lbProgressText.text = $"{_progressCurrentCoin}/{_townPrice}";
            if (!isLackCoin)
            {
                _progressSlider.value = info.deltaTime * (_townPrice / GameConfig.TOWN_BUY_DURATION);
            }
        }

        if (info.inputType == GameEnums.EInputType.Up)
        {
            SetActive(false);
        }
        
    }

    public void Clear()
    {
        _progressSlider.value = 0;
        lbProgressText.text = "0/0";
        _progressCurrentCoin = 0;
    }
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
