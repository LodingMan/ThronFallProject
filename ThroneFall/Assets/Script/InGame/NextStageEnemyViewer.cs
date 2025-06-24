using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static GameEnums;


public class SpawnerTransformData
{
    public Transform SpawnerTransform;
    public int SpawnerIndex;
}

public class EnemyViewerDataContext
{
    public StageData stageData;
    public List<UnitData> unitDatas;
}
public class NextStageEnemyViewer : MonoBehaviour,
    IInitialize<EnemyViewerDataContext>,
    IMultiEventHandlerProvider
{
    [SerializeField]List<NextEnemyListView> _enemyListView = new List<NextEnemyListView>();
    [SerializeField] NextEnemyListView _nextEnemyListViewObject;
    [SerializeField] Canvas _lootCanvas;
    private StageData StageData;
    private List<UnitData> UnitDatas;
    private List<SpawnerTransformData> SpawnerTransformDatas;
    private int _lastRecvCurrentRound = 0;
    
    private Dictionary<Type, Delegate> _eventCallBackDic = new();
    public Dictionary<Type, Delegate> GetEventProviderCallBackDic()
    {
        return _eventCallBackDic;
    }


    private void Start()
    {

    }

    public void Initialize(EnemyViewerDataContext context)
    {
        int spawnerCount = GameObject.FindObjectsOfType<Spawner>().ToList().Count;
        for (int i = 0; i < spawnerCount; i++)
        {
            var view = Instantiate(_nextEnemyListViewObject, this.transform);
            if (view.TryGetComponent<FollowSpawnerUI>(out var ui))
            {
                ui.canvas = _lootCanvas;
            }
            view.index = i;
            _enemyListView.Add(view);
        }

        UnitDatas = context.unitDatas;
        StageData = context.stageData;
        _eventCallBackDic[typeof(EGameState)] = new Action<EGameState>(OnStateChangeCallback);
        _eventCallBackDic[typeof(int)] = new Action<int>(OnRoundChangeCallback);
        var spawners = GameObject.FindObjectsOfType<Spawner>();
        SpawnerTransformDatas = spawners.Select(s => new SpawnerTransformData
        {
            SpawnerTransform = s.transform,
            SpawnerIndex = s.Index
        }).ToList();
    }

    public void OnRoundChangeCallback(int round)
    {
        _lastRecvCurrentRound = round;
    }

    public void OnStateChangeCallback(EGameState state)
    {
        switch (state)
        {
            case EGameState.None:
                break;
            case EGameState.Waiting:
                SetListViews(_lastRecvCurrentRound);
                ShowNextEnemysInfo();
                break;
            case EGameState.Combat:
                HideNextEnemysInfo();
                break;
            case EGameState.GameOver:
                break;
            case EGameState.GameClear:
                break;
            case EGameState.Result:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }  
    }

    public void SetListViews(int currentRound)
    {
        for (int i = 0; i < _enemyListView.Count; i++)
        {
            var view = _enemyListView.Find(s => s.index == i);
            var enemyInfos = StageData.GetRoundEnemyInfo(currentRound, view.index);
            var list = enemyInfos.Select(info =>
            {
                string iconName = UnitDatas.Find(u => u.UnitID == info.Item1).IconName;
                return new EnemyCountData
                {
                    EnemyIconName = iconName, 
                    Count = info.Item2,
                };
            }).ToList();
            view.SetItems(list);
            view.SetTransform(SpawnerTransformDatas.Find(s => s.SpawnerIndex == i));
        }
    }
    
    
    public void ShowNextEnemysInfo()
    {
        foreach (var view in _enemyListView)
        {
            view.ShowItem();
        }
    }
    public void HideNextEnemysInfo()
    {
        foreach (var view in _enemyListView)
        {
            view.HideItem();
        }
    }




    
    private Action _unRegistCallback;
    public void SetEventUnRegistCallback(Action callback)
    {
        _unRegistCallback += callback;
    }
    private void OnDestroy()
    {
        _unRegistCallback?.Invoke();
    }
}
