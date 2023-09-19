using UnityEngine;

public class AimPointer
{
    private Vector3 _camPosition;
    private float _size;
    private GameObject _pointer;

    public AimPointer(float size, GameObject pointer)
    {
        _size = size;
        _pointer = pointer;
    }

    public void ShowPointer()
    {
        float scale = Vector3.Distance(_pointer.transform.position, _camPosition);
        _pointer.transform.localScale = Vector3.one * scale * _size;
        _pointer.SetActive(true);
    }

    public void HidePointer() => _pointer.SetActive(false);
}
