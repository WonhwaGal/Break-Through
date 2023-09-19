using UnityEngine;

public class PlayerMovement
{
    private CharacterController _characterController;
    private Transform _playerT;
    private float _speed;
    private Vector3 _moveDirection;
    private const float GravityMultiplier = 2.5f;

    private CameraMovement _cameraMovement;

    public PlayerMovement(CharacterController characterController, Transform playerT, float speed)
    {
        _characterController = characterController;
        _speed = speed;
        _playerT = playerT;


        _cameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
        _cameraMovement.AssignToPlayer(_playerT);
    }

    public void Move(IInputService input)
    {
        _moveDirection = _playerT.TransformDirection(input.KeyAxis);
        _moveDirection.y += Physics.gravity.y * Time.deltaTime * GravityMultiplier;
        _characterController.Move(_speed * Time.deltaTime * _moveDirection.normalized);
        _cameraMovement.FollowPlayer();

        _cameraMovement.UpdateRotation(input.MouseAxis);
        Rotate();
    }

    private void Rotate()
    {
        Vector3 lookPoint = _cameraMovement.CenterT.position + _cameraMovement.CenterT.forward * _speed;
        Vector3 lookDirection = lookPoint - _playerT.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        lookRotation.x = 0;
        lookRotation.z = 0;

        _playerT.rotation = Quaternion.Lerp(_playerT.rotation, lookRotation, _speed * Time.deltaTime); ;
    }
}
