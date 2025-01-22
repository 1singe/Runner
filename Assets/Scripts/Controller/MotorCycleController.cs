using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;


public class MotorCycleController : MonoBehaviour
{
    // Physics
    public float Friction;
    public float AirResistance;


    public float GroundDetectionRadius = 0.01f;

    public int normalSampleCount = 5;

    [Range(0, 1)] public float sampleDistance = 0.001f;

    public float ThrusterLength = 1f;
    public float ThrusterForce = 1f;
    public float ThrusterDamping = 0.1f;

    public float TorqueForce = 1f;
    public float TorqueDamping = 0.1f;
    public float IdealHeight = 2f;

    public float MaxSpeed = 36f;
    public float Acceleration = 200;

    public bool grounded = false;

    private Rigidbody _rigidbody;

    private InputAction _accelerateAction;
    private InputAction _breakAction;
    private InputAction _moveAction;
    private InGameControls _controls;

    private Vector2 _moveVector;
    private bool _accelerating = false;
    private bool _braking = false;

    private Vector3 m_GoalVelocity;
    private Vector3 m_UnitGoal;

    public AnimationCurve AccelerationFactorFromDot;

    private float speedFactor = 1f;
    private float maxAccelerationFactor = 1f;

    public float MaxAccelerationForce = 10;

    public Vector3 ForceScale;
    public AnimationCurve MaxAccelerationForceFactorFromDot;
    private Vector3 GroundVelocity;

    public ParticleSystem DustParticles;
    public float DustDistanceMAx = 5f;

    private ParticleSystem.EmissionModule DustEmission;
    private ParticleSystem.MainModule DustMain;

    private float groundDist = 1f;
    public AnimationCurve DustCurve;
    private Vector3 groundProjection;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controls = new InGameControls();
        _accelerateAction = _controls.Player.Accelerate;
        _moveAction = _controls.Player.Move;
        _breakAction = _controls.Player.Break;

        _accelerateAction.performed += OnAccelerateActionPerformed;
        _breakAction.performed += OnBreakActionPerformed;
    }

    private void Start()
    {
        DustMain = DustParticles.main;
        DustEmission = DustParticles.emission;
    }

    private void OnBreakActionPerformed(InputAction.CallbackContext obj)
    {
        _braking = obj.performed;
    }

    private void OnAccelerateActionPerformed(InputAction.CallbackContext obj)
    {
        _accelerating = obj.performed;
    }

    private void OnEnable()
    {
        _accelerateAction.Enable();
        _moveAction.Enable();
        _breakAction.Enable();
    }

    private void OnDisable()
    {
        _accelerateAction.Disable();
        _moveAction.Disable();
        _breakAction.Disable();
    }


    private void Update()
    {
        _moveVector = _moveAction.ReadValue<Vector2>();
        UpdateVFX();
    }

    private void UpdateVFX()
    {
        DustParticles.transform.position = groundProjection;
        float horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z).sqrMagnitude;
        int Amount = (int)Mathf.Lerp(horizontalVelocity, 0f, DustCurve.Evaluate(Mathf.Clamp(groundDist, 0f, DustDistanceMAx) / DustDistanceMAx));
        DustEmission.rateOverTime = new ParticleSystem.MinMaxCurve(Amount * 10f);
        //DustEmission.SetBurst(0, new ParticleSystem.Burst(0, Amount * 0.5f, Amount, 0.05f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float halfChunkSize = (ProceduralGenerationManager.Instance.ChunkSize - 1) / 2f;

        float samplePosX = transform.position.x;
        float samplePosY = transform.position.z;
        float groundHeight = Generated_GenerationStatics.SampleDunes(samplePosX, samplePosY);

        groundDist = Mathf.Max(transform.position.y - groundHeight, 0);
        groundProjection = new Vector3(transform.position.x, groundHeight, transform.position.z);

        Vector3 WantedUp = Vector3.up;

        #region Hovering

        if (transform.position.y <= groundHeight + ThrusterLength)
        {
            var Velocity = _rigidbody.linearVelocity;

            // Calculate the normal of the noise
            // We find 3 points very close from eachother, and get the normal of the plan they form
            int sampleCount = 3;
            float[] adjacentGroundHeights = new float[sampleCount];

            // Center
            Vector2 aXY = new Vector2(samplePosX + sampleDistance, samplePosY); // 0 deg
            adjacentGroundHeights[0] = Generated_GenerationStatics.SampleDunes(aXY.x, aXY.y);
            Vector3 a = new Vector3(aXY.x, adjacentGroundHeights[0], aXY.y);

            Vector2 bXY = new Vector2(samplePosX + Mathf.Cos(Mathf.Deg2Rad * 120) * sampleDistance, samplePosY + Mathf.Sin(Mathf.Deg2Rad * 120) * sampleDistance); // 120 deg
            adjacentGroundHeights[1] = Generated_GenerationStatics.SampleDunes(bXY.x, bXY.y);
            Vector3 b = new Vector3(bXY.x, adjacentGroundHeights[1], bXY.y);

            Vector2 cXY = new Vector2(samplePosX + Mathf.Cos(Mathf.Deg2Rad * 240) * sampleDistance, samplePosY + Mathf.Sin(Mathf.Deg2Rad * 240) * sampleDistance); // 240 deg
            adjacentGroundHeights[2] = Generated_GenerationStatics.SampleDunes(cXY.x, cXY.y);
            Vector3 c = new Vector3(cXY.x, adjacentGroundHeights[2], cXY.y);

            Debug.DrawRay(a, Vector3.up * 0.01f, Color.green);
            Debug.DrawRay(b, Vector3.up * 0.01f, Color.green);
            Debug.DrawRay(c, Vector3.up * 0.01f, Color.green);

            // Find the normal with Cross Product

            Vector3 Normal = Vector3.Cross(c - b, c - a);
            Normal.Normalize();
            WantedUp = Normal;

            Debug.DrawRay(new Vector3(transform.position.x, groundHeight, transform.position.z), Normal.normalized * 1f, Color.blue);

            float x = transform.position.y - groundHeight - IdealHeight;

            float spring = (x * ThrusterForce) - (x * ThrusterDamping);

            _rigidbody.AddForce(Vector3.down * spring);
        }

        #endregion

        #region Input

        _moveVector = _moveAction.ReadValue<Vector2>();
//Input Process{
        if (_moveVector.magnitude > 1.0f)
        {
            _moveVector.Normalize();
        }

        #endregion

        #region Movement

        m_UnitGoal = new Vector3(_moveVector.x, 0, _moveVector.y);

        Vector3 unitVelocity = m_GoalVelocity.normalized;

        float VelocityDot = 0; //Vector3.Dot(m_UnitGoal, unitVelocity);

        float accel = Acceleration * AccelerationFactorFromDot.Evaluate(VelocityDot);

        Vector3 goalVel = m_UnitGoal * (MaxSpeed * speedFactor);

        m_GoalVelocity = Vector3.MoveTowards(m_GoalVelocity, goalVel + GroundVelocity, accel * Time.fixedDeltaTime);

        Vector3 neededAcceleration = (m_GoalVelocity - _rigidbody.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = MaxAccelerationForce * MaxAccelerationForceFactorFromDot.Evaluate(VelocityDot) * maxAccelerationFactor;

        neededAcceleration = Vector3.ClampMagnitude(neededAcceleration, maxAccel);

        _rigidbody.AddForce(Vector3.Scale(neededAcceleration * _rigidbody.mass, ForceScale));

        //Hard clamp for ground collsion

        if (transform.position.y <= groundHeight)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, groundHeight), transform.position.z);
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, 0), _rigidbody.linearVelocity.z);
        }

        #endregion

        Quaternion toGoal = Quaternion.FromToRotation(transform.up, WantedUp);
        Vector3 rotAxis;
        float rotDegrees;
        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();
        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _rigidbody.AddTorque((rotAxis * (rotRadians * TorqueForce)) - (_rigidbody.angularVelocity * TorqueDamping));
    }


    //if (transform.position.y <= groundHeight + ThrusterLength)
    //{
    //    grounded = true;
    //    SpeedVector.y = Mathf.Max(SpeedVector.y, 0);
    //}
    /*
    // Gravity
    AccelerationVector += new Vector3(0, -9.8f, 0);

    if (Input.GetKey(KeyCode.Z) && grounded)
    {
        AccelerationVector += new Vector3(0, 0, ForwardAcceleration);
    }

    SpeedVector += AccelerationVector * Time.deltaTime;


    // Detect ground


    if (transform.position.y <= groundHeight + GroundDetectionRadius)
    {
        grounded = true;
        SpeedVector.y = Mathf.Max(SpeedVector.y, 0);
    }
    else
    {
        grounded = false;
    }


    transform.position += SpeedVector * Time.deltaTime;

    if (grounded)
    {
        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, groundHeight), transform.position.z);
    }

    // Damping the speed

    // If grounded, apply base friction and terrain friction
    if (grounded)
        SpeedVector *= (1f - Friction);
    else
        SpeedVector *= (1f - AirResistance);

    // At the very end, reset every acceleration vector

    AccelerationVector = Vector3.zero;
    */
}