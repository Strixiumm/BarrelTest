using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenSlideButtons : MonoBehaviour
{
    public EditorReferences References;

    [Serializable]
    public class EditorReferences
    {
        public VerticalLayoutGroup ButtonsGroup;
    }

    public int Spacing;
    public int Size;
    private bool _open = false;

    private void Awake()
    {
        foreach (Transform child in References.ButtonsGroup.transform)
        {
            child.gameObject.SetActive(false);
            child.localScale = Vector3.zero;
        }
        Size = References.ButtonsGroup.padding.bottom;
        Spacing = (int)References.ButtonsGroup.spacing;
    }
    
    public void Toggle()
    {
        _open = !_open;
        foreach (Transform child in References.ButtonsGroup.transform)
        {
            child.gameObject.SetActive(true);
            child.DOComplete();
            child.DOScale(_open ? 1f : 0f, .1f).SetDelay(_open ? 0f : .1f);
        }
        DOTween.To(() => References.ButtonsGroup.spacing, x => References.ButtonsGroup.spacing = x, this._open ? Spacing : -Size, .2f);
        DOTween.To(() => References.ButtonsGroup.padding.bottom, x => References.ButtonsGroup.padding.bottom = x, this._open ? (Size + Spacing) : 0, .2f).OnComplete(
            () =>
            {
                if (!_open)
                {
                    foreach (Transform child in References.ButtonsGroup.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                    
            });
    }

    public void Close()
    {
        if(_open)
            Toggle();
    }

    private void OnDisable()
    {
        Close();
    }
}