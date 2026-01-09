using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class FrontWheel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CarInput.instance.movement.x > 0)
        {
            transform.rotation = Quaternion.Euler(-90, 45, 0);
        }else if (CarInput.instance.movement.x < 0)
        {
            transform.rotation = Quaternion.Euler(-90, -45, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(-90,0,0);
        }
    }
}
