using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minVertRotation;
    [SerializeField] private float _maxVertRotation;
    [SerializeField] private Transform _center;

    private float _mouseSensitivity;
    private float _cameraXRotation;
    private float _cameraYRotation;
    private Transform _playerT;
    private Pointer _aimPointer;
    private const string PrefsSensitivity = "masterSensitivity";

    public Transform CenterTransform => _center.transform;

    public void Init(Transform playerT)
    {
        _playerT = playerT;
        _aimPointer = ServiceLocator.Container.RequestFor<Pointer>();
        _aimPointer.SetUpPointer(transform);
        SetSensitivityMultiplier();
        GameEventSystem.Subscribe<SensitivityEvent>(ChangeSensitivity);
    }

    public void UpdateRotation(Vector3 mouseInput) => Rotate(mouseInput);

    private void Rotate(Vector3 mouseInput)
    {
        _cameraXRotation += mouseInput.y * _mouseSensitivity * (-1);
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, _minVertRotation, _maxVertRotation);

        _cameraYRotation += mouseInput.x * _mouseSensitivity;
        _cameraYRotation %= 360;

        Vector3 rotatingAngle = new Vector3(_cameraXRotation, _cameraYRotation, 0);
        Quaternion rotation = Quaternion.Slerp(_center.transform.localRotation, Quaternion.Euler(rotatingAngle), _rotationSpeed * Time.deltaTime);
        _center.localRotation = rotation;
    }

    public void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, _playerT.transform.position, _moveSpeed * Time.deltaTime);
        transform.position = moveVector;

        _aimPointer.Update();
    }

    private void ChangeSensitivity(SensitivityEvent @event) => _mouseSensitivity = @event.NewValue;

    private void SetSensitivityMultiplier()
    {
        if (PlayerPrefs.HasKey(PrefsSensitivity))
        {
            var step = PlayerPrefs.GetFloat("sensStep");
            _mouseSensitivity = PlayerPrefs.GetInt(PrefsSensitivity) * step + step * 2;
        } 
    }

    private void OnDestroy()
    {
        GameEventSystem.UnSubscribe<SensitivityEvent>(ChangeSensitivity);
    }
}