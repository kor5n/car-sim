using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Driving : MonoBehaviour
{
    [Header("Input")]
    private float moveInput = 0;
    private float steerInput = 0;

    [Header("Car Settings")]
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float decelleration = 10f;

    private Vector3 currentCarVelocity = Vector3.zero;
    private float carVelocityRatio = 0;

    [Header("Suspension Settings")]
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float wheelRadius;

    [Header("References")]
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private LayerMask drivable;
    [SerializeField] private Transform accelerationPoint;

    private int[] wheelsIsGrounded =  new int[4];
    private bool isGrounded = false;
    
    private void GroundCheck()
    {
        int tempGroundedWheels = 0;

        for (int i=0; i < wheelsIsGrounded.Length; i++)
        {
            tempGroundedWheels += wheelsIsGrounded[i];
        }

        if (tempGroundedWheels > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CalculateCarVelocity()
    {
        currentCarVelocity = transform.InverseTransformDirection(carRB.velocity);
        carVelocityRatio = currentCarVelocity.z / maxSpeed;
    }

    private void Suspension()
    {
        for(int i=0; i<rayPoints.Length;i++)
        {
            RaycastHit hit;
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, drivable))
            {
                wheelsIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLength - currentSpringLength) / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float dampForce = damperStiffness * springVelocity;

                float springForce = springStiffness * springCompression;

                float netForce = springForce - dampForce;

                carRB.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelsIsGrounded[i] = 0;

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (wheelRadius + maxLength) * -rayPoints[i].up, Color.green);
            }

        }

    }

    private void Acceleration()
    {
   
        carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        
    }

    private void Decceleration()
    {
        
        carRB.AddForceAtPosition(decelleration * moveInput * -transform.forward, accelerationPoint.position, ForceMode.Acceleration); //maybe should be changed
        
    }

    private void GetPlayerInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }
    
    private void Movement()
    {
        if (isGrounded)
        {
            Acceleration();
            Decceleration();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        carRB = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Movement();
    }
    private void Update()
    {
        GetPlayerInput();
    }
}
