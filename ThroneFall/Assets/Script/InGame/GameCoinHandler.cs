using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameEnums;

public class UseCoinInfo
{
    public ECoinUseType useType;
    public int coin;
}
public class GameCoinHandler : MonoBehaviour,
    IInitialize<StageData>
{
    [SerializeField]TMP_Text _lbCoin;
    private StageData _stageData;
    [SerializeField] GameCoin _coinPrefab;
    [SerializeField] List<GameCoin> _inBoxCoins = new List<GameCoin>();
    [SerializeField] List<GameCoin> _outBoxCoins = new List<GameCoin>();
    [SerializeField] private Transform _trCoinSpawn;
    [SerializeField] private Transform _trPopCoinSpawn;
    private Action<int> _onChangeCoin;
    private int _coin;
    private int Coin
    {
        get => _inBoxCoins.Count; 
    }
    public void Initialize(StageData stageData)
    {
        _stageData = stageData;
        if (GameConfig.CurrentSelectStage != 0)
        {
            for (int i = 0; i < GameConfig.GAME_START_COIN; i++)
            {
                CreateInBoxCoin();
            }
        }

        Refresh();


    }

    public void Reset()
    {
    }

    public void Refresh()
    {
        _lbCoin.text = $"Coin : {Coin}";
        _onChangeCoin?.Invoke(Coin);
    }

    public void UseCoinCancel(Transform returnTransform)
    {

        foreach (var outBoxCoin in _outBoxCoins)
        {
            outBoxCoin.SpawnCoin(returnTransform.position);
        }
        _outBoxCoins.Clear();
    }

    public void UseCoinComplete()
    {
        foreach (var outBoxCoin in _outBoxCoins)
        {
            Destroy(outBoxCoin.gameObject);
        }
        _outBoxCoins.Clear();
    }
    public void CreateInBoxCoin()
    {
        GameCoin coin = Instantiate(_coinPrefab, _trCoinSpawn.position, Quaternion.identity);
        coin.transform.SetParent(this.transform);
        PushCoin(coin);
    }
    public void CreateCoin(Vector3 pos)
    {
        GameCoin coin = Instantiate(_coinPrefab, pos + (Vector3.up * 2), Quaternion.identity);
        coin.transform.SetParent(this.transform);
        coin.SpawnCoin(pos + (Vector3.up * 2));
    }
    public void PushCoin(GameCoin coin)
    {
        _outBoxCoins.Remove(coin);
        _inBoxCoins.Add(coin);
        coin.transform.position = _trCoinSpawn.position;
        
        Refresh();
    }
    public bool PopCoin()
    {
        //상자에서 Coin사라짐. 별도의 공간으로 잠시 빼놓음. -1Coin
        if (_inBoxCoins.Count <= 0) return false;
        _outBoxCoins.Add(_inBoxCoins[_inBoxCoins.Count - 1]);
        _outBoxCoins[_outBoxCoins.Count - 1].transform.position = _trPopCoinSpawn.position;
        _inBoxCoins.RemoveAt(_inBoxCoins.Count - 1);
        
        Refresh();
        return true;
    }
    
}
