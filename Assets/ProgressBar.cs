using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    Color startingColor;
    [SerializeField]
    Color endingColor;

    [SerializeField]
    Transform glassCapsule;

    [SerializeField]
    Renderer fillRenderer;

    /// <summary>
    /// The global position from which the fill begins (0%)
    /// </summary>
    [SerializeField]
    Transform Floor;

    /// <summary>
    /// The global position at which the fill ends (100%)
    /// </summary>
    [SerializeField]
    Transform Roof;

    [SerializeField]
    float _percentage = 0f;

    /// <summary>
    /// Updates the fill to a percentage
    /// </summary>
    /// <param name="percentage">A value from 0 to 100</param>
    public void UpdatePercentage(float percentage)
    {
        if (percentage > 0)
        {
            glassCapsuleRenderer.enabled = true;
            fillRenderer.enabled = true;
        }
        if (percentage > 100f)
        {
            Debug.LogWarning($"Fill percentage {percentage} is over 100%");
            percentage = 100f;
        }
        if (percentage < 0f)
        {
            Debug.LogWarning($"Fill percentage {percentage} is negative");
            percentage = 0f;
        }

        if (percentage >= 100f && tweenStarted == false)
        {
            tweenStarted = true;
            glassCapsule.DORotate(Vector3.forward * 720, 1, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            var initialScale = glassCapsule.localScale;
            glassCapsule.DOScale(Vector3.zero, 0.5f).SetDelay(0.5f).SetEase(Ease.InQuint).OnComplete(() =>
            {
                glassCapsuleRenderer.enabled = false;
                fillRenderer.enabled = false;
                glassCapsule.localScale = initialScale;
            });
        }
        Color c = Color.Lerp(startingColor, endingColor, percentage * 0.01f);
        fillRenderer.material.SetColor("_Color", c);
        fillRenderer.material.SetColor("_CrossColor", c);
        _percentage = percentage;
    }

    private void Update()
    {
        var GlobalPositionDifference = Roof.position - Floor.position;
        fillRenderer.material.SetVector("_PlanePosition", Floor.position + GlobalPositionDifference * _percentage * 0.01f);
        fillRenderer.material.SetVector("_PlaneNormal", glassCapsule.up);
    }

    [SerializeField]
    Renderer glassCapsuleRenderer;
    private void Awake()
    {
        if (glassCapsuleRenderer == null)
        {
            glassCapsuleRenderer = glassCapsule.GetComponent<MeshRenderer>();
        }
    }

    bool tweenStarted = false;
    private void OnEnable()
    {
        tweenStarted = false;
        UpdatePercentage(0f);
    }
}
