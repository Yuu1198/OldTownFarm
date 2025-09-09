using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public static ScreenTransition Instance;

    [SerializeField] private Image transitionBackground;
    [SerializeField] private float transitionDuration = 0.5f;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator FadeOutIn(System.Action onFadeComplete)
    {
        transitionBackground.gameObject.SetActive(true);
        yield return Fade(1);
        onFadeComplete?.Invoke();
        yield return Fade(0);
        transitionBackground.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        Color color = transitionBackground.color;
        float startingAlpha = color.a;
        float time = 0f;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float t = time / transitionDuration;
            color.a = Mathf.Lerp(color.a, startingAlpha, t);
            transitionBackground.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        transitionBackground.color = color;
    }
}
