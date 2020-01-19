using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HitSuccessIndicator : MonoBehaviour
{
    private const string ShowKey = "Show";
    private const string HideKey = "Hide";

    [SerializeField] private Canvas canvas;
    [SerializeField] Panel panel;
    [SerializeField] Image arrow;
    [SerializeField] Text label;
    Tweener transition;

    void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.gameObject.SetActive(false);
    }

    public void SetStats(int chance, int amount)
    {
        arrow.fillAmount = (chance / 100f);
        label.text = string.Format("%{0} - {1} Damage", chance, Mathf.Abs(amount));
        label.color = amount > 0 ? Color.green : Color.red;
    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        SetPanelPos(ShowKey);
    }

    public void Hide()
    {
        SetPanelPos(HideKey);
        transition.completedEvent += delegate(object sender, System.EventArgs e)
        {
            canvas.gameObject.SetActive(false);
        };
    }

    void SetPanelPos(string pos)
    {
        if (transition != null && transition.IsPlaying)
            transition.Stop();

        transition = panel.SetPosition(pos, true);
        transition.duration = 0.5f;
        transition.equation = EasingEquations.EaseInOutQuad;
    }
}
