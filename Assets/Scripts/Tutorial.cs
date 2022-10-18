using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform finger;


    private void Start() 
    {
        finger.DOAnchorPosX(184, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
           StartCoroutine(StartFade());
        }
    }

    private IEnumerator StartFade()
    {
        float lerp = 0f;
        do
        {
            lerp += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, lerp);
            yield return null;
        } while (lerp < 1f);

        gameObject.SetActive(false);
    }
}
