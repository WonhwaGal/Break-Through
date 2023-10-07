using System.Threading.Tasks;
using UnityEngine;

public sealed class FinalLevelDoor : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private static readonly int s_openDoor = Animator.StringToHash("PlayerOpen");
    private bool _finalLevelUnlocked;
    private StatisticsCounter _stats;

    private void Start()
    {
        _stats = ServiceLocator.Container.RequestFor<StatisticsCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckForUnlock())
            return;

        if (other.GetComponent<PlayerView>())
        {
            _animator.SetBool(s_openDoor, true);
            LoadFinalLevel();
        }
    }

    private bool CheckForUnlock()
    {
        _finalLevelUnlocked = _stats.KeyNumber >= Constants.UnlockFinalLevelKeyNumber;
        return _finalLevelUnlocked;
    }

    private async void LoadFinalLevel()
    {
        await Task.Delay(1500);
        GameEventSystem.Send<LoadLevelEvent>(new LoadLevelEvent(toFinal: true));
    }
}