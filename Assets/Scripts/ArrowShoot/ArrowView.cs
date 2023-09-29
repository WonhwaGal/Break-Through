using System;
using System.Collections;
using UnityEngine;

public class ArrowView : MonoBehaviour, IArrow
{
    [SerializeField] private Rigidbody _arrowRb;
    [SerializeField, Range(5,20)] private float _force = 15;

    private const float ArrowLifeTime = 3.0f;

    public ArrowType ArrowType { get; set; }
    public Rigidbody RigidBody => _arrowRb;
    public float Force => _force;

    public event Action<ArrowView> OnHittingAgent;

    private void OnEnable() => StartCoroutine(TurnOff());

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable agent))
            agent.CauseDamage(ArrowType);

        OnHittingAgent?.Invoke(this);
    }

    IEnumerator TurnOff()
    {
        var arrowOnTime = 0.0f;
        while(arrowOnTime < ArrowLifeTime)
        {
            yield return null;
            arrowOnTime += Time.deltaTime;
        }
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _arrowRb.velocity = Vector3.zero;
        StopCoroutine(TurnOff());
    }
}