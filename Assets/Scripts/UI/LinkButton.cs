using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkButton : MonoBehaviour
{
    public string URL;
    public float BlinkTime = 0.5f;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image
            .DOColor(Color.clear, BlinkTime)
            .SetEase(Ease.InQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OpenLink()
    {
        Application.OpenURL(URL);
    }
}