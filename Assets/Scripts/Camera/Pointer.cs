using UnityEngine;

public class Pointer: IService
{
    private GameObject _aimPointer;
    private Transform _cameraT;
    private LayerMask _enemyMask;
    private const float MaxDistance = 30;
    private const float Size = 0.01f;

    public Pointer(GameObject pointer, IInputService input)
    {
        _aimPointer = GameObject.Instantiate(pointer);
        _cameraT = Camera.main.transform;
        _enemyMask = LayerMask.GetMask("Enemy");

        _aimPointer.SetActive(false);
        GameEventSystem.Subscribe<PlayerAimEvent>(ShowPointer);
    }

    public Transform PointerT => _aimPointer.transform;

    public void Update()
    {
        if (!_aimPointer.activeInHierarchy)
            return;

        PlacePointer();
    }

    public void SetUpPointer(Transform parent) => _aimPointer.transform.SetParent(parent, false);

    private void PlacePointer()
    {
        RaycastHit rayCast;
        if (Physics.Raycast(_cameraT.position, _cameraT.forward, out rayCast, MaxDistance, _enemyMask))
        {
            float scale = Vector3.Distance(_aimPointer.transform.position, _cameraT.position);
            _aimPointer.transform.localScale = Vector3.one * scale * Size;
            _aimPointer.transform.position = rayCast.point;
        }
        else
        {
            _aimPointer.transform.position = Vector3.zero;
        }
    }

    private void ShowPointer(PlayerAimEvent @event) => _aimPointer.gameObject.SetActive(@event.AimPressed);

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(ShowPointer);
    }
}