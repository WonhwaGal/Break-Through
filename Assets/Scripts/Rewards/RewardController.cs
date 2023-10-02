using System;
using UnityEngine;

public sealed class RewardController
{
    private readonly UIScriptableObject _prefabs;
    private readonly IPool<RewardIcon> _rewardPool;
    private readonly Transform _root;
    private readonly Camera _camera;

    public RewardController(UIScriptableObject uiPrefabs, Transform root)
    {
        _root = root;
        _prefabs = uiPrefabs;
        _camera = Camera.main;
        _rewardPool = new Pool<RewardIcon>(_prefabs.RewardIcon, _root);
        GameEventSystem.Subscribe<EnemyKilledEvent>(SpawnReward);
    }

    public void SpawnReward(EnemyKilledEvent @event)
    {
        switch (@event.RewardType)
        {
            case RewardType.Arrow:
                PlaceReward(@event, _prefabs.ArrowSprite);
                break;
            case RewardType.Key:
                PlaceReward(@event, _prefabs.KeySprite);
                break;
            default:
                PlaceReward(@event, _prefabs.HpSprite);
                break;
        }
    }

    private void PlaceReward(EnemyKilledEvent @event, Sprite sprite)
    {
        var rewardIcon = _rewardPool.Spawn();
        rewardIcon.transform.localScale = Vector3.one;
        rewardIcon.transform.position = _camera.WorldToScreenPoint(@event.SpawnPosition);
        rewardIcon.RewardImage.sprite = sprite;
        rewardIcon.RewardType = @event.RewardType;
        rewardIcon.RewardAmount = @event.RewardAmount;
        rewardIcon.RewardText.text = rewardIcon.RewardAmount.ToString();
        rewardIcon.OnReadyToDespawn += Despawn;
    }

    private void Despawn(RewardIcon reward)
    {
        reward.OnReadyToDespawn -= Despawn;
        _rewardPool.Despawn(reward);
    }

    public void Dispose()
    {
        GameEventSystem.Subscribe<EnemyKilledEvent>(SpawnReward);
        GC.SuppressFinalize(this);
    }
}