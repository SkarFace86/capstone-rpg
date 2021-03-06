﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MenuButtons : State
{

    public void StartSelectCharacter()
    {
        
    }

    public void LoadStartScene(string scenename)
    {
        Debug.Log(SceneManager.GetSceneAt(0).name);
        StartCoroutine(StartScreenTransition(scenename));
    }

    IEnumerator StartScreenTransition(string name)
    {
        SceneManager.LoadScene(name);

        yield return new WaitForSeconds(0.5f);
    }
}
