using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Repeater _hor = new Repeater("Horizontal");
    private Repeater _ver = new Repeater("Vertical");

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;
    string[] _buttons = new string[] {"Fire1", "Fire2", "Fire3"};


    // Update is called once per frame
    void Update()
    {
        int x = _hor.Update();
        int y = _ver.Update();
        // Check for keyboard input
        if (x != 0 || y != 0)
            moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));

        // Check for mouse buttons pressed
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetButtonUp(_buttons[i]))
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
        }
    }

    void OnEnable()
    {
        InputController.moveEvent += OnMoveEvent;
        InputController.fireEvent += OnFireEvent;
    }

    void OnDisable()
    {
        InputController.moveEvent -= OnMoveEvent;
        InputController.fireEvent -= OnFireEvent;
    }

    private void OnFireEvent(object sender, InfoEventArgs<int> e)
    {
        
    }

    private void OnMoveEvent(object sender, InfoEventArgs<Point> e)
    {
        
    }
}

class Repeater
{
    private const float threshold = 0.5f;
    private const float rate = 0.25f;
    private float _next;
    private bool _hold;
    private string _axis;

    public Repeater(string axisName)
    {
        _axis = axisName;
    }

    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        if (value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return retValue;
    }
}