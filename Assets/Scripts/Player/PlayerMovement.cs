using UnityEngine;

public class PlayerMovement
{
    private readonly CharacterController _characterController;
    private readonly Transform _playerT;
    private readonly float _speed;
    private Vector3 _moveDirection;
    private const float GravityValue = -1.8f;

    private CameraMovement _cameraMovement;

    public PlayerMovement(CharacterController characterController, float speed)
    {
        _characterController = characterController;
        _speed = speed;
        _playerT = _characterController.transform;
        _cameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
        _cameraMovement.Init(_playerT);
    }

    public void Move(IInputService input)
    {
        _moveDirection = _playerT.TransformDirection(input.KeyAxis);
        _moveDirection.y += GravityValue;
        _characterController.Move(_speed * Time.deltaTime * _moveDirection.normalized);
        _cameraMovement.FollowPlayer();

        _cameraMovement.UpdateRotation(input.MouseAxis);
        Rotate();
    }

    private void Rotate()
    {
        Vector3 lookPoint = _cameraMovement.CenterTransform.position + _cameraMovement.CenterTransform.forward * _speed;
        Vector3 lookDirection = lookPoint - _playerT.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        lookRotation.x = 0;
        lookRotation.z = 0;

        _playerT.rotation = Quaternion.Lerp(_playerT.rotation, lookRotation, _speed * Time.deltaTime);
    }
}