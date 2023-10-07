using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class RewardIcon : BaseRewardItem
{
    public Image RewardImage;
    public TextMeshProUGUI RewardText;

    private Color _tempColor = Color.white;
    private const float ActiveTime = 1.5f;
    private const float FadeAwaySpeed = 1.0f;

    public event Action<RewardIcon> OnReadyToDespawn;

    private void OnEnable()
    {
        StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float alpha = 1.5f;
        float currentTime = 0.0f;
        while (currentTime < ActiveTime)
        {
            if (!_isPaused)
            {
                transform.position = Vector2.MoveTowards(transform.position, 
                    (Vector2)transform.position - Vector2.right, FadeAwaySpeed);
                alpha -= Time.deltaTime / ActiveTime;
                RewardText.alpha = alpha;
                _tempColor.a = alpha;
                RewardImage.color = _tempColor;
                currentTime += Time.deltaTime;
            }
            yield return null;
        }
        gameObject.SetActive(false);
        StopCoroutine(FadeAway());
    }

    private void OnDisable()
    {
        SendReceiveAwardEvent();
        OnReadyToDespawn?.Invoke(this);
    }
}