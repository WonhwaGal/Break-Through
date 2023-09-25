using UnityEngine;

public class AimPointer: MonoBehaviour
{
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _size = 0.008f;

    private Vector3 _camPosition;

    public void ShowPointer()
    {
        float scale = Vector3.Distance(_pointer.transform.position, _camPosition);
        _pointer.transform.localScale = Vector3.one * scale * _size;
        _pointer.SetActive(true);
    }

    public void HidePointer() => _pointer.SetActive(false);
}
