using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageInitializationContext
{
    public StageData StageData;
    public List<UnitData> UnitDatas;
    public List<TownData> TownDatas;
    public IGameStateProvider GameState;
    public IGameRoundProvider GameRound;
    public IEnemyCountProvider EnemyCount;
    public IGameCoinProvider GameCoin;
    public List<UnitPair> UnitObjects;
    public EventRegisterContext EventRegisterContext;
}
public class StageLifeCycleHandler : MonoBehaviour,
    IAsyncInitialize<List<UnitData>>
{
    private StageInitializationContext Context;
    [SerializeField] private GameObject sceneBlind;
     [SerializeField] ReferenceScriptable StageReference;
    public async UniTask Initialize(List<UnitData> unitDatas)
    {
        await CreateStage(unitDatas);
    }

    public async UniTask CreateStage(List<UnitData> unitDatas)
    {
        //var result = await AddressablesManager.LoadAssetAsync<GameObject>($"Stage{GameConfig.CurrentSelectStage}");

        var result = await AddressablesManager.LoadAssetAsync2<GameObject>(StageReference.FindStage($"{GameConfig.CurrentSelectStage}Stage").Reference);
        if (!result.Succeeded)
        {
            return;
        }
        var prefab = result.Value;
        var stagePrefab = Instantiate(prefab,this.transform);
        stagePrefab.GetComponentInChildren<PlayerCreator>()?.Initialize(unitDatas);
        AudioController.instance.Stop(SoundConfig.SoundType.Bgm);
        sceneBlind.SetActive(false);
    }
    private void OnDestroy()
    {
        if(Context == null)
            return;
        AddressablesManager.ReleaseAsset(StageReference.FindStage($"{GameConfig.CurrentSelectStage}Stage").Reference);
 
    }

    public void RestartStage()
    {
        SceneLoader.Instance.LoadSingleScene("InGameScene");
    }

}
