using UnityEngine;

public sealed class BonusReward : BaseRewardItem
{
    [SerializeField] private RewardType _rewardType;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;
    private bool _foundByPlayer;

    private void Start()
    {
        RewardType = _rewardType;
        RewardAmount = Random.Range(_minAmount, _maxAmount);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerView>())
        {
            _foundByPlayer = true;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (_foundByPlayer)
            SendReceiveAwardEvent();
    }
}