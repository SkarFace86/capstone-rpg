using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamageController : MonoBehaviour
{
    [SerializeField] private Text abilityDamage;
    [SerializeField] private Text abilityHeal;
    [SerializeField] private GameObject canvas;
    [SerializeField] private CameraRig cameraRig;
    private Camera camera;

    void Start()
    {
        abilityDamage.text = "";
        abilityHeal.text = "";
        camera = cameraRig.GetComponentInChildren<Camera>();
    }

    public void DisplayAbilityDamage(string amount, Unit defender)
    {
        canvas.SetActive(true);
        abilityDamage.text = amount;
        abilityDamage.rectTransform.position = camera.WorldToScreenPoint(defender.transform.position);
        StartCoroutine(Sequence(amount, null, defender, null));
    }

    public void DisplayAbilityHeal(string amount, Unit target)
    {
        canvas.SetActive(true);
        abilityHeal.text = amount;
        abilityHeal.rectTransform.position = camera.WorldToScreenPoint(target.transform.position);
        //StartCoroutine(Sequence());
    }

    public void DisplayAbilityAborb(string amount, Unit defender, Unit attacker)
    {
        canvas.SetActive(true);
        abilityDamage.text = amount;
        abilityDamage.rectTransform.position = camera.WorldToScreenPoint(defender.transform.position);

        abilityHeal.text = amount;
        abilityHeal.rectTransform.position = camera.WorldToScreenPoint(attacker.transform.position);
        //StartCoroutine(Sequence());
    }

    public void DisplayAbilityStatusEffect(string message, Unit target)
    {
        canvas.SetActive(true);
        abilityDamage.text = message;
        abilityDamage.rectTransform.position = camera.WorldToScreenPoint(target.transform.position);
        //StartCoroutine(Sequence());
    }

    IEnumerator Sequence(string targetMessage, string selfMessage, Unit targetUnit, Unit selfUnit)
    {
        yield return new WaitForSeconds(1.5f);

        abilityDamage.text = "";
        abilityHeal.text = "";

        canvas.SetActive(false);
    }
}
