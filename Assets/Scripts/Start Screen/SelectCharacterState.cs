using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SelectCharacterState : State
{
    private GameObject parent;
    private List<GameObject> characters = new List<GameObject>();

    private int index = 0;

    private void Start()
    {
        parent = GameObject.Find("Characters");
        Debug.Log(parent.transform.childCount);
        for (int i = 0; i < parent.transform.childCount; i++)
            characters.Add(parent.transform.GetChild(i).gameObject);
    }

    void Update()
    {
        //if (Input.GetAxis("Horizontal") != 0)
        //{
        //    if (index == 0 && Input.GetAxis("Horizontal") < 0)
        //        index = parent.transform.childCount;
        //    else if (index == parent.transform.childCount && Input.GetAxis("Horizontal") > 0)
        //        index = 0;
        //    else
        //        index += (int)Input.GetAxis("Horizontal");

        //    Debug.Log(index);
        //}

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (index == 0)
                index = parent.transform.childCount - 1;
            else
                index--;

            LookAtCharacter(index);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (index == parent.transform.childCount - 1)
                index = 0;
            else
                index++;

            LookAtCharacter(index);
        }
    }

    void LookAtCharacter(int index)
    {

    }
}
