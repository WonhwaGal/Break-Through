﻿using UnityEngine;

public class Pointer: IService
{
    private IInputService _input;
    private GameObject _aimPointer;
    private Transform _cameraT;
    private LayerMask _enemyMask;
    private const float MaxDistance = 30;
    private const float Size = 0.01f;

    public Pointer(GameObject pointer, IInputService input)
    {
        _aimPointer = GameObject.Instantiate(pointer);
        _input = input;
        _cameraT = Camera.main.transform;
        _enemyMask = LayerMask.GetMask("Enemy");

        _aimPointer.SetActive(false);
        _input.OnPressingAim += ShowPointer;
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

    private void ShowPointer(bool toShow) => _aimPointer.gameObject.SetActive(toShow);

    public void Dispose() => _input.OnPressingAim -= ShowPointer;
}