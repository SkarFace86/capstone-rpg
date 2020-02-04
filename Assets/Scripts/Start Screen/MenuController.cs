using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : StateMachine
{
    public CameraRig cameraRig;

    public Canvas canvas;

    // Start is called before the first frame update
    public void BeginSelectCharacter()
    {
        ChangeState<SelectCharacterState>();
    }
}
