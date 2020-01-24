using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNavigationScene : MonoBehaviour
{
    private Animation anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animation>();
        anim.Play("Camera_Opening");
    }
}
