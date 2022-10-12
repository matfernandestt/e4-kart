using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BaseKart : MonoBehaviour
{
    public bool isGrounded;
    public bool isAccelerating;
    public bool isBraking;

    public float speed;
    
    private Rigidbody thisRigidbody;

    private BaseSubKart subKart;
    private Quaternion normal;
    private Vector2 axisInput;
    private Vector3 momentumDirection;

    private InputMapping input;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        subKart = GetComponentInChildren<BaseSubKart>();
        normal = transform.rotation;

        input = new InputMapping();
        input.Enable();

        input.Player.Accelerate.performed += StartAccelerating;

        momentumDirection = Vector3.zero;
    }

    private void OnDestroy()
    {
        input.Player.Accelerate.performed -= StartAccelerating;
    }

    private void StartAccelerating(InputAction.CallbackContext context)
    {
        momentumDirection = Vector3.zero;
        thisRigidbody.velocity = momentumDirection;
    }

    private void Update()
    {
        isAccelerating = input.Player.Accelerate.ReadValue<float>() != 0;
        isBraking = input.Player.Brake.ReadValue<float>() != 0;
        axisInput = input.Player.Movement.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        if (Physics.Raycast(subKart.GetColliderTransform.position, Vector3.down, out var hit, 1f))
        {
            var road = hit.collider.GetComponent<Road>();
            if (road != null)
            {
                isGrounded = true;

                normal = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, normal, Time.deltaTime * 5f);
        if(isGrounded && thisRigidbody.velocity.magnitude >= 1)
            subKart.UpdateRotation(axisInput);
    }

    private void HandleMovement()
    {
        thisRigidbody.AddForce((isGrounded ? -transform.up : Vector3.down) * 10f);
        if (isGrounded)
        {
            if (isAccelerating && !isBraking)
            {
                if (thisRigidbody.velocity.magnitude < 30f)
                {
                    momentumDirection = Vector3.Lerp(momentumDirection, subKart.transform.forward, Time.deltaTime);
                    thisRigidbody.velocity = momentumDirection * Time.deltaTime * speed;
                }
            }

            if (isBraking)
            {
                thisRigidbody.velocity /= 2f;
                momentumDirection = Vector3.zero;
            }
        }
        if(Mathf.Abs(thisRigidbody.velocity.magnitude) <= 0)
            momentumDirection = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(Application.isPlaying ? subKart.GetColliderTransform.position : transform.position, transform.position + (Vector3.down * 1f));
    }
}