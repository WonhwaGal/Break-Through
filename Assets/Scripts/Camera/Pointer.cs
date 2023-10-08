using UnityEngine;

public sealed class Pointer : IService
{
    private readonly GameObject _aimPointer;
    private readonly Transform _cameraT;
    private readonly LayerMask _enemyMask;
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
        if (!_aimPointer.activeSelf)
            return;

        PlacePointer();
    }

    public void SetUpPointer(Transform parent) => _aimPointer.transform.SetParent(parent, false);

    private void PlacePointer()
    {
        RaycastHit rayCast;
        if (Physics.Raycast(_cameraT.position, _cameraT.forward, out rayCast, Constants.MaxPointerDistance, _enemyMask))
        {
            float scale = Vector3.Distance(_aimPointer.transform.position, _cameraT.position);
            _aimPointer.transform.localScale = scale * Size * Vector3.one;
            _aimPointer.transform.position = rayCast.point;
        }
        else
        {
            _aimPointer.transform.position = Vector3.zero;
        }
    }

    private void ShowPointer(PlayerAimEvent @event) => _aimPointer.SetActive(@event.AimPressed);

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<PlayerAimEvent>(ShowPointer);
    }
}