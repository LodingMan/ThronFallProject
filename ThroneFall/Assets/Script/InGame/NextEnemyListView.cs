using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountData
{
    public string EnemyIconName;
    public int Count;
    public SpawnerTransformData SpawnerTransformData;
}
public class NextEnemyListView : BaseItemListView<EnemyCountData,NextEnemyListItem>
{
    public int index;
    private FollowSpawnerUI _followSpawnerUI;

    private void Awake()
    {
    }



    public void SetTransform(SpawnerTransformData spawnerTransformData)
    {
        _followSpawnerUI = GetComponent<FollowSpawnerUI>();

        if (_followSpawnerUI != null)
        {
            _followSpawnerUI.targetSpawner = spawnerTransformData.SpawnerTransform;
        }
    }
    public void HideItem()
    {
        foreach (var nextEnemyListItem in ItemList)
        {
            nextEnemyListItem.gameObject.SetActive(false);
        }
    }
    public void ShowItem()
    {
        foreach (var nextEnemyListItem in ItemList)
        {
            nextEnemyListItem.gameObject.SetActive(true);
        }
    }

}
