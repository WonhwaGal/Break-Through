using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private SpawnScriptableObject _spawnPrefabs;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        RegisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Container.Register<KeyboardInputService>(new KeyboardInputService());
        ServiceLocator.Container.Register<ArrowController>(new ArrowController(_spawnPrefabs));
        ServiceLocator.Container.Register<Pointer>(
            new Pointer(_spawnPrefabs.PointerPrefab, ServiceLocator.Container.RequestFor<KeyboardInputService>()));
    }
}