using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamageController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject combatText;

    public void DisplayCombatText(string amount, Unit target, Color color)
    {
        GameObject obj = Instantiate(combatText, canvas.transform);
        Text ct = obj.GetComponent<Text>();
        ct.text = amount;
        ct.color = color;
        ct.rectTransform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        StartCoroutine(Sequence(obj));
    }

    IEnumerator Sequence(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);

        Destroy(obj);
    }
}
