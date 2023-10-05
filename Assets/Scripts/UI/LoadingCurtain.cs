using System.Collections;
using UnityEngine;

public class LoadingCurtain : MonoBehaviour, IService
{
    public CanvasGroup Curtain;

    private WaitForSeconds _timeSpan = new WaitForSeconds(0.03f);

    private void Awake() => DontDestroyOnLoad(this);

    public void Show()
    {
        gameObject.SetActive(true);
        Curtain.alpha = 1;
    }

    public void Hide() => StartCoroutine(FadeIn());

    private IEnumerator FadeIn()
    {
        while (Curtain.alpha > 0)
        {
            Curtain.alpha -= 0.03f;
            yield return _timeSpan;
        }

        gameObject.SetActive(false);
    }
}