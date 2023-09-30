using System;
using UnityEngine;

public class ArrowView : MonoBehaviour, IArrow, IPausable
{
    [SerializeField] private Rigidbody _arrowRb;
    [SerializeField, Range(5, 20)] private float _force = 15;
    private Vector3 _pausedVelocity;
    private ParticleSystem _particleSystem;

    public ArrowType ArrowType { get; set; }
    public Rigidbody RigidBody => _arrowRb;
    public float Force => _force;

    public event Action<ArrowView> OnHittingAgent;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        GameEventSystem.Subscribe<GameStopEvent>(Pause);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamagable agent))
            agent.CauseDamage(ArrowType);

        OnHittingAgent?.Invoke(this);
    }

    public void Pause(GameStopEvent @event)
    {
        Debug.Log("I hear");
        if (_arrowRb.velocity != Vector3.zero && @event.IsPaused)
        {
            _pausedVelocity = _arrowRb.velocity;
            _arrowRb.velocity = Vector3.zero;
            _particleSystem.Pause();
            Debug.Log("I pause");
        }
        else
        {
            _arrowRb.velocity = _pausedVelocity;
            _particleSystem.Play();
            Debug.Log("I play");
        }
    }

    private void OnDisable()
    {
        _arrowRb.velocity = Vector3.zero;
        GameEventSystem.UnSubscribe<GameStopEvent>(Pause);
    }
}