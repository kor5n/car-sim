using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    public InputActionAsset carControl; 

    private InputAction handBrake;

    private InputAction move;

    public bool eBraking = false;

    public Vector2 movement;

    public static CarInput instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }

        var groundMap = carControl.FindActionMap("GROUND", true);
        move = groundMap.FindAction("MOVE", true);
        handBrake = groundMap.FindAction("HANDBRAKE", true);

        //register
        move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        move.canceled  += ctx => movement = Vector2.zero;   
        handBrake.started  += _ => eBraking = true;
        handBrake.canceled += _ => eBraking = false;
    }

    void OnEnable()
    {
      carControl.Enable();
      Debug.Log("Input Enabled");
    }

    void OnDisable()
    {
        carControl.Disable();
        Debug.Log("Input Disabled");
    }
}
