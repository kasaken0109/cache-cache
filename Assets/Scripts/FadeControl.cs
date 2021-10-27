using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeControl : MonoBehaviour
{
    static private FadeControl _instance;
    static public FadeControl Instance
    {
        get
        {
            if (_instance == null)
            {
                object fade = FindObjectOfType(typeof(FadeControl));
                if (fade != null)
                {
                    Debug.Log("ある");
                    _instance = (FadeControl)fade;
                }
                else
                {
                    // 自身を入れる。
                    GameObject fadeObj = new GameObject("FadeObj");
                    _instance = fadeObj.AddComponent<FadeControl>();

                    // Canvasの生成
                    GameObject canvasObj = new GameObject("FadeCanvas");
                    canvasObj.transform.SetParent(fadeObj.transform);
                    Canvas canvas = canvasObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.sortingOrder = 10;
                    canvasObj.AddComponent<CanvasScaler>();
                    canvasObj.AddComponent<GraphicRaycaster>();

                    // Imaageの生成
                    GameObject imageObj = new GameObject("FadeImage");
                    imageObj.transform.SetParent(canvasObj.transform);
                    _instance._fadeImage = imageObj.AddComponent<Image>();
                    RectTransform rect = _instance._fadeImage.GetComponent<RectTransform>();
                    rect.anchorMin = Vector2.zero;
                    rect.anchorMax = Vector2.one;
                    rect.offsetMin = Vector2.zero;
                    rect.offsetMax = Vector2.zero;

                    _instance._fadeImage.color = Color.black;

                    _instance._canvasGroup = imageObj.AddComponent<CanvasGroup>();
                    fadeObj.hideFlags = HideFlags.HideInHierarchy;
                    DontDestroyOnLoad(fadeObj);
                }
            }

            return _instance;
        }
    }

    Image _fadeImage;
    CanvasGroup _canvasGroup;

    Action _onEndFade;

    public const float DefFadeTime = 1.0f;

    float _startVal = 0;
    float _endVal = 0;
    float _time = 0;
    float _fadeTime = 0;
    bool _isFade = false;

    public static bool IsFade => Instance._isFade;

    void Update()
    {
        if (!_isFade) return;

        _time += Time.deltaTime;
        float a = _time / _fadeTime;
        _canvasGroup.alpha = Mathf.Lerp(_startVal, _endVal, a);

        if (a > 1f)
        {
            Debug.Log("Fade終了");
            _onEndFade?.Invoke();
            _onEndFade = null;
            _isFade = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public static void FadeIn(float fadeTime = DefFadeTime, Action action = null)
    {
        Instance.StartFade(1.0f, 0, fadeTime, action);
    }
    
    public static void FadeOut(float fadeTime = DefFadeTime, Action action = null)
    {
        Instance.StartFade(0, 1.0f, fadeTime, action);
    }

    public static void FadeIn(Action action)
    {
        Instance.StartFade(1.0f, 0, DefFadeTime, action);
    }

    public static void FadeOut(Action action)
    {
        Instance.StartFade(0, 1.0f, DefFadeTime, action);
    }

    private void StartFade(float startVal, float endVal, float fadeTime, Action onEndFade)
    {
        if (_isFade) return;

        _isFade = true;

        this._startVal = startVal;
        this._endVal = endVal;
        this._fadeTime = fadeTime;
        this._onEndFade = onEndFade;

        _canvasGroup.alpha = startVal;
        _canvasGroup.blocksRaycasts = true;
        _time = 0;
    }

    private void OnDestroy()
    {
        if (_instance == this) { _instance = null; }
    }
}
