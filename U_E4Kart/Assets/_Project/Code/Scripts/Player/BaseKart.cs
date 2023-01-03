using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BaseKart : MonoBehaviour
{
    public BaseSubKart subKart;
    public CameraFollower myCam;
    [SerializeField] private AudioSource brakeAudioSource;

    public bool isGrounded;
    public bool isAccelerating;
    public bool isBraking;
    public bool isReversing;

    public float speed;
    public float maxSpeed;

    private Rigidbody thisRigidbody;
    private Coroutine surroundingsDetectionRoutine;
    private Coroutine endGameSurroundingsDetectionRoutine;

    private Quaternion normal;
    private Vector2 axisInput;
    private Vector3 momentumDirection;

    private InputMapping input;

    private Vector3 checkpointPosition;
    private Quaternion checkpointSubKartRotation;
    private Quaternion checkpointRotation;

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        normal = transform.rotation;

        input = new InputMapping();
        input.Enable();

        input.Player.Accelerate.performed += StartAccelerating;
        input.Player.Reverse.canceled += StoppedReversing;
        input.Player.Brake.performed += StartBrake;
        input.Player.Brake.canceled += StopBrake;
        RaceController.onStartRace += OnStartRace;

        momentumDirection = Vector3.zero;

        EnableMovement(false);
    }

    private void OnDestroy()
    {
        input.Player.Accelerate.performed -= StartAccelerating;
        input.Player.Reverse.canceled -= StoppedReversing;
        input.Player.Brake.performed -= StartBrake;
        input.Player.Brake.canceled -= StopBrake;
        RaceController.onStartRace -= OnStartRace;
    }

    public void RegisterCheckpoint(Vector3 position, Quaternion rotation)
    {
        checkpointPosition = position;
        checkpointRotation = rotation;
        checkpointSubKartRotation = subKart.transform.rotation;
    }

    private void Respawn()
    {
        EnableMovement(false);
        thisRigidbody.velocity = Vector3.zero;
        transform.position = checkpointPosition;
        transform.rotation = checkpointRotation;
        subKart.transform.rotation = checkpointSubKartRotation;
        EnableMovement(true);
    }

    private void OnStartRace()
    {
        RegisterCheckpoint(transform.position, transform.rotation);
        EnableMovement(true);
    }

    public void EnableMovement(bool enable)
    {
        if (thisRigidbody != null)
        {
            thisRigidbody.isKinematic = !enable;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = !enable;
        }

        if (enable)
        {
            DetectSurroundings();
        }
        else
        {
            StopDetectingSurroundings();
        }
    }

    public void DisableController()
    {
        input.Disable();
        EndGameDetectSurroundings();
    }

    private void StartAccelerating(InputAction.CallbackContext context)
    {
        //momentumDirection /= 2;
        //thisRigidbody.velocity = momentumDirection;
    }
    
    private void StoppedReversing(InputAction.CallbackContext context)
    {
        myCam.ForceNormalCamera();
    }
    
    private void StartBrake(InputAction.CallbackContext context)
    {
        if (thisRigidbody.velocity.magnitude == 0) return;
        
        brakeAudioSource.Play();
    }
    
    private void StopBrake(InputAction.CallbackContext context)
    {
        brakeAudioSource.Pause();
    }

    private void Update()
    {
        isAccelerating = input.Player.Accelerate.ReadValue<float>() != 0;
        isBraking = input.Player.Brake.ReadValue<float>() != 0;
        axisInput = input.Player.Movement.ReadValue<Vector2>();
        isReversing = input.Player.Reverse.ReadValue<float>() != 0;
        
        if(myCam != null)
            myCam.UpdateReverseInput(isReversing);
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
        if (isGrounded && thisRigidbody.velocity.magnitude >= 1)
            subKart.UpdateRotation(axisInput);
    }

    private void HandleMovement()
    {
        thisRigidbody.AddForce((isGrounded ? -transform.up : Vector3.down) * 10f);
        if (isGrounded)
        {
            if (isAccelerating && !isBraking && !isReversing)
            {
                if (thisRigidbody.velocity.magnitude < maxSpeed)
                {
                    momentumDirection = Vector3.Lerp(momentumDirection, subKart.transform.forward, Time.deltaTime);
                    thisRigidbody.velocity = momentumDirection * Time.deltaTime * speed;
                }
            }

            if (isReversing && !isBraking && !isAccelerating)
            {
                if (thisRigidbody.velocity.magnitude < maxSpeed / 2)
                {
                    momentumDirection = Vector3.Lerp(momentumDirection, -subKart.transform.forward, Time.deltaTime / 1.5f);
                    thisRigidbody.velocity = momentumDirection * Time.deltaTime * speed;
                }
            }

            if (isBraking)
            {
                thisRigidbody.velocity = Vector3.Lerp(thisRigidbody.velocity, Vector3.zero, Time.deltaTime);
                momentumDirection = Vector3.zero;
            }
        }

        if (Mathf.Abs(thisRigidbody.velocity.magnitude) <= 0)
            momentumDirection = Vector3.zero;
        subKart.GetVisuals.SetIsMoving(thisRigidbody.velocity.magnitude > .2f);
        Debug.Log(thisRigidbody.velocity.magnitude);
    }

    private void StopDetectingSurroundings()
    {
        if (surroundingsDetectionRoutine != null)
            StopCoroutine(surroundingsDetectionRoutine);
        if (endGameSurroundingsDetectionRoutine != null)
            StopCoroutine(endGameSurroundingsDetectionRoutine);
    }

    private void DetectSurroundings()
    {
        if (surroundingsDetectionRoutine != null)
            StopCoroutine(surroundingsDetectionRoutine);
        surroundingsDetectionRoutine = StartCoroutine(Detection());

        IEnumerator Detection()
        {
            while (true)
            {
                var surroundings = Physics.OverlapSphere(transform.position, 1f);
                foreach (var collision in surroundings)
                {
                    var deathCollision = collision.GetComponent<RespawnerPlane>();
                    if (deathCollision != null)
                    {
                        Respawn();
                    }

                    var finishLine = collision.GetComponent<FinishLine>();
                    if (finishLine != null && collision.isTrigger)
                    {
                        finishLine.FinishRace(PhotonNetwork.player);
                        StopDetectingSurroundings();
                    }

                    var checkpoint = collision.GetComponent<Checkpoint>();
                    if (checkpoint != null && collision.isTrigger)
                    {
                        RegisterCheckpoint(checkpoint.transform.position, transform.rotation);
                    }
                }
                yield return null;
            }
        }
    }
    
    private void EndGameDetectSurroundings()
    {
        if (surroundingsDetectionRoutine != null)
            StopCoroutine(surroundingsDetectionRoutine);
        if (endGameSurroundingsDetectionRoutine != null)
            StopCoroutine(endGameSurroundingsDetectionRoutine);
        endGameSurroundingsDetectionRoutine = StartCoroutine(Detection());

        IEnumerator Detection()
        {
            while (true)
            {
                var surroundings = Physics.OverlapSphere(transform.position, 1f);
                foreach (var collision in surroundings)
                {
                    var deathCollision = collision.GetComponent<RespawnerPlane>();
                    if (deathCollision != null)
                    {
                        EnableMovement(false);
                        StopDetectingSurroundings();
                    }
                }
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(Application.isPlaying ? subKart.GetColliderTransform.position : transform.position,
            transform.position + (Vector3.down * 1f));
    }
}