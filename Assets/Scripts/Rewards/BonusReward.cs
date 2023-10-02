using UnityEngine;

public class BonusReward : BaseRewardItem
{
    [SerializeField] private RewardType _rewardType;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;

    private void Start()
    {
        RewardType = _rewardType;
        RewardAmount = Random.Range(_minAmount, _maxAmount);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerView>())
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        SendReceiveAwardEvent();
    }
}
