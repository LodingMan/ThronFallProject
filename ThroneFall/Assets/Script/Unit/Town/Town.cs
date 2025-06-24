using System;
using UnityEngine;
using static GameEnums;

public class Town : MonoBehaviour,
    ITownDataProvider,
    ITargetableUnit,
    IInteractionAble
{
    public string townID;
    protected EGameState _gameState;
    protected TownState _townState;
    protected Attack _attack;
    protected Health _health;
    protected TownData _townData;
    protected GameCoinHandler _gameCoinHandler;
    [SerializeField] protected TownBuyProgress _townBuyProgress;
    [SerializeField] protected GameObject _buildTownObject;
    [SerializeField] protected Collider _InteractionCollider;
    [SerializeField] protected GameObject _silhouette;
    [SerializeField] protected GameObject _preTown;

    private bool _isTargetAble = false;
    public bool GetTargetAble => _isTargetAble;
    public EUnitType GetUnitType => EUnitType.Town;
    public ETownState GetCurrentTownState => _townState.GetCurrentState;
    public Vector3 GetPosition => transform.position;

    public PoolObject SpawnEffect;
    public PoolObject BreakEffect;

    public Town ParentTown;

    protected Action<Town> _breakAction;

    InstancePanelTownInfo _instancePanelTownInfo;
    [SerializeField] protected Transform _trReturnCoin;

    private int _currentRecvCoin = 0;

    private void Start()
    {
        _gameCoinHandler = FindObjectOfType<GameCoinHandler>();
    }

    public virtual void Initialize(TownData townData)
    {
        _townData = townData;
        _townState = GetComponent<TownState>();//GetComponent<TownState>();
        if (_townState == null)
        {
            Debug.LogWarning("unitState is NULL");
            return;
        }
        _townState?.Initialize();

        if (TryGetComponent<Health>(out var unitHealth))
        {
            _health = unitHealth;
            _health.Initialize(_townData.Hp);
        }

        if (TryGetComponent<Attack>(out var attack))
        {
            _attack = attack;
            _attack.Initialize(new AttackData()
            {
                Damage = _townData.Damage,
                CoolDown = townData.AttackCoolDown,
                Range = townData.AttackRange,
            }, _townState);
        }

        _townBuyProgress = GetComponentInChildren<TownBuyProgress>();
        if (_townBuyProgress != null)
        {
            _townBuyProgress.Initialize(townData.Price);
        }

        GameStateEventBus.RegistEvent(GameStateCallbackEvent);
        GameResultEventBus.RegistEvent(GameResultCallbackEvent);

        _buildTownObject.SetActive(false);
        _townBuyProgress.SetActive(false);
        _instancePanelTownInfo = FindObjectOfType<InstancePanelTownInfo>();
        CheckInteratable();

    }

    public void RegistEvent(Action<Town> breakAction)
    {
        _breakAction = breakAction;
    }

    private void CheckInteratable()
    {
        bool isInteract = true;

        if (FlagEnumHas(_townState.GetCurrentState, ETownState.Break))
        {
            isInteract = false;
        }
        if (FlagEnumHas(_townState.GetCurrentState, ETownState.Enable))
        {
            isInteract = false;
        }
        if (_gameState != EGameState.Waiting)
        {
            isInteract = false;
        }
        _isInteractable = isInteract;
        //_InteractionCollider.enabled = isInteract;

    }

    public virtual void TownCreate()
    {
        _InteractionCollider.enabled = true;
        _isTargetAble = true;
        if (SpawnEffect != null)
        {
            var obj = ObjectPooler.instance.GetObjectPool(SpawnEffect, transform.position);
            obj.transform.position = transform.position;//this.transform.position; 
        }
        _townState.AddTownState(ETownState.Enable);
        _townState.RemoveTownState(ETownState.Disable);
        _townState.RemoveTownState(ETownState.Break);
        _buildTownObject.SetActive(true);
        _health.SetActive(true);
        _health.SetFullHealth();
        _preTown.SetActive(false);
        _silhouette.SetActive(false);
        CheckInteratable();
    }

    public virtual void TownBreak()
    {
        var obj = ObjectPooler.instance.GetObjectPool(BreakEffect, transform.position);
        obj.transform.position = transform.position;//this.transform.position;
        _isTargetAble = false;
        _townState.AddTownState(ETownState.Break);
        _buildTownObject.SetActive(false);
        _health.SetActive(false);
        CheckInteratable();
        _breakAction(this);
        _InteractionCollider.enabled = false;
    }

    public void Hit(AttackInfo attackInfo)
    {
        if (_townState.GetCurrentState != ETownState.Break)
        {
            _health.MinusHP(attackInfo.Damage);
            if (_health.CurrentHP <= 0)
            {
                _health.SetHP(0);
                _health.SetActive(false);
                _townState.Clear();
                TownBreak();
            }
            Debug.Log($"OnHit : {attackInfo.AttackType}, {attackInfo.Damage}");
        }
    }



    public virtual void GameStateCallbackEvent(EGameState state)
    {
        _gameState = state;
    }

    public virtual void GameResultCallbackEvent(EGameResult result)
    {
        if (result == EGameResult.RoundClear)
        {
            if (FlagEnumHas(_townState.GetCurrentState, ETownState.Break))
            {
                TownCreate();
            }
            _health.SetFullHealth();
        }
    }


    public TownData GetTownData()
    {
        return _townData;
    }


    public bool _isInteractable = false;
    public bool IsInteractable => _isInteractable;

    public void OnInteraction(InputInfo interactionInfo)
    {
        bool isLackCoin = false;
        if (_isInteractable == false)
        {
            return;
        }

        if (interactionInfo.inputType == GameEnums.EInputType.Down)
        { 
            AudioController.instance.PlaySound("Town_Build", SoundConfig.SoundType.Effect2);

            _currentRecvCoin = 0;
        }
        if (interactionInfo.inputType == GameEnums.EInputType.Press)
        {
            var progressTick = (GameConfig.TOWN_BUY_DURATION / _townData.Price) * (_currentRecvCoin + 1);
            if (progressTick < interactionInfo.deltaTime)
            {
                if (_gameCoinHandler.PopCoin())
                {
                    _currentRecvCoin++;
                }
                else
                {
                    isLackCoin = true;
                }
            }
        }
        if (interactionInfo.inputType == GameEnums.EInputType.Up)
        {
            _townBuyProgress.Clear();
            _gameCoinHandler.UseCoinCancel(_trReturnCoin);
            AudioController.instance.Stop(SoundConfig.SoundType.Effect2);
            if (IsInteractable)
            {
                AudioController.instance.PlaySound("CoinUseCancel", SoundConfig.SoundType.Effect);
            }

        }
        _townBuyProgress.UpdateProgress(interactionInfo, isLackCoin);

        if (_townData.Price <= _currentRecvCoin)
        {
            TownCreate();
            AudioController.instance.PlaySound("Town_Buy", SoundConfig.SoundType.Effect);
            _townBuyProgress.Clear();
            _townBuyProgress.SetActive(false);
            _instancePanelTownInfo.SetActivePanel(false);
            _gameCoinHandler.UseCoinComplete();
        }
        Debug.Log($"TownInteraction: {interactionInfo.deltaTime}");
    }

    public void OnTriggerIn()
    {
        if (_isInteractable)
        {
            _instancePanelTownInfo.SetInfo(_townData);
            _instancePanelTownInfo.SetActivePanel(true);
            _townBuyProgress.SetActive(true);
            _silhouette?.SetActive(true);
        }

    }

    public void OnTriggerOut()
    {
        _instancePanelTownInfo.SetActivePanel(false);
        _townBuyProgress.SetActive(false);
        _silhouette?.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateEventBus.UnRegistEvent(GameStateCallbackEvent);
        GameResultEventBus.UnRegistEvent(GameResultCallbackEvent);
    }
}
