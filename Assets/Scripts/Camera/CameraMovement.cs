using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minVertRotation;
    [SerializeField] private float _maxVertRotation;
    [SerializeField] private Transform _center;
    [SerializeField] private GameObject _pointerObj;

    private float _cameraXRotation;
    private float _cameraYRotation;
    private Transform _playerT;
    private AimPointer _aimPointer;
    public Transform CenterT => _center.transform;

    private void Awake() => _aimPointer = new AimPointer(0.008f, _pointerObj);

    public void AssignToPlayer(Transform playerT) => _playerT = playerT;

    public void UpdateRotation(Vector3 mouseInput)
    {
        Rotate(mouseInput);
        UpdatePointer();
    }

    private void Rotate(Vector3 mouseInput)
    {
        _cameraXRotation += mouseInput.y * (-1);
        _cameraXRotation = Mathf.Clamp(_cameraXRotation, _minVertRotation, _maxVertRotation);

        _cameraYRotation += mouseInput.x;
        _cameraYRotation %= 360;

        Vector3 rotatingAngle = new Vector3(_cameraXRotation, _cameraYRotation, 0);
        Quaternion rotation = Quaternion.Slerp(_center.transform.localRotation, Quaternion.Euler(rotatingAngle), _rotationSpeed * Time.deltaTime);
        _center.localRotation = rotation;
    }

    public void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, _playerT.transform.position, _moveSpeed * Time.deltaTime);
        transform.position = moveVector;
    }

    private void UpdatePointer()
    {

    }
}
