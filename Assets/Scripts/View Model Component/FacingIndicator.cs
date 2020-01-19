using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacingIndicator : MonoBehaviour
{
    [SerializeField] private Renderer[] directions;
    [SerializeField] private Material normal;
    [SerializeField] private Material selected;

    public void SetDirection(Directions dir)
    {
        int index = (int) dir;
        for (int i = 0; i < 4; i++)
            directions[i].material = (i == index) ? selected : normal;
    }
}
