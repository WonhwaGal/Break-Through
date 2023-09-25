using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyView : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private Slider _hpSlider;

    private StateMachine _stateMachine = new();
    private EnemyModel _enemyModel = new();
    private EnemyAnimator _enemyAnimator;
    private EnemyType _enemyType;

    public NavMeshAgent Agent { get; private set; }
    public EnemyType EnemyType { get => _enemyType; set => _enemyType = value; }
    public EnemyModel EnemyModel => _enemyModel;
    public IStateMachine StateMachine => _stateMachine;
    public EnemyAnimator EnemyAnimator => _enemyAnimator;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _enemyModel.OnMoving += _enemyAnimator.AnimateMovement;
        _enemyModel.OnTakingAShot += _enemyAnimator.AnimateShot;
        _enemyModel.OnDying += _enemyAnimator.AnimateDeath;
        _hpSlider.maxValue = _maxHP;
        EnemyModel.HP = _maxHP;
    }

    private void OnEnable()
    {
        EnemyModel.SetRewardValues();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))  //check dying
            EnemyModel.IsDead = true;

        _stateMachine.UpdateStateMachine();
    }

    private void OnDisable()
    {
        EnemyModel.IsDead = false;
    }

    private void OnDestroy()
    {
        _enemyModel.OnMoving -= _enemyAnimator.AnimateMovement;
        _enemyModel.OnTakingAShot -= _enemyAnimator.AnimateShot;
        _enemyModel.OnDying -= _enemyAnimator.AnimateDeath;
    }
}
