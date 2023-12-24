using System;
using UnityEngine;
using Cinemachine;

public sealed class PlayerMovement : IDisposable
{
    private readonly CharacterController _characterController;
    private readonly Transform _playerT;
    private readonly float _speed;
    private Vector3 _moveDirection;
    private const float GravityValue = -1.8f;

    private readonly Transform _followT;
    private float _mouseSensitivity;
    private Transform _cameraT;

    private readonly Pointer _aimPointer;

    public PlayerMovement(CharacterController characterController, float speed, Transform followT)
    {
        _characterController = characterController;
        _speed = speed;
        _playerT = _characterController.transform;
        _followT = followT;
        _aimPointer = ServiceLocator.Container.RequestFor<Pointer>();
        SetSensitivityMultiplier();
        GameEventSystem.Subscribe<SensitivityEvent>(ChangeSensitivity);
        GameEventSystem.Subscribe<FollowPlayerEvent>(AssignPlayer);
    }

    private void AssignPlayer(FollowPlayerEvent @event)
    {
        _cameraT = Camera.main.transform;
        _aimPointer.SetUpPointer(_cameraT);
        @event.Manager.SetUpCameras(_followT, _mouseSensitivity);
    }

    public void Move(IInputService input)
    {
        if (_cameraT == null)
            return;

        _moveDirection = _playerT.TransformDirection(input.KeyAxis);
        _moveDirection.y += GravityValue;
        _characterController.Move(_speed * Time.deltaTime * _moveDirection.normalized);

        if (input.KeyAxis != Vector3.zero || _aimPointer.IsAiming)
            Rotate();

        _aimPointer.Update();
    }

    private void Rotate()
    {
        var targetRotation = _cameraT.rotation;
        targetRotation.x = 0;
        targetRotation.z = 0;
        _playerT.rotation = Quaternion
            .Slerp(_playerT.rotation, targetRotation, _speed * Time.deltaTime);
    }

    private void ChangeSensitivity(SensitivityEvent @event) => _mouseSensitivity = @event.NewValue;

    private void SetSensitivityMultiplier()
    {
        if (PlayerPrefs.HasKey(Constants.PrefsSensitivity))
        {
            var step = Constants.MouseSensitivityStep;
            _mouseSensitivity =
                PlayerPrefs.GetInt(Constants.PrefsSensitivity) * step + step * 2;
        }
    }

    public void Dispose()
    {
        GameEventSystem.UnSubscribe<SensitivityEvent>(ChangeSensitivity);
        GameEventSystem.UnSubscribe<FollowPlayerEvent>(AssignPlayer);
        _aimPointer.Dispose();
        GC.SuppressFinalize(this);
    }
}