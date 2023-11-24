using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Movement
{ //Refactored script form https://catlikecoding.com/unity/tutorials/movement/
    public class PlayerMovementV03 : MonoBehaviour
    {
        [SerializeField]
        private ShadowController shadowController;
        [Header("Feedbacks")]
        [SerializeField] private MMFeedbacks jumpFeedback;
        [SerializeField] private MMFeedbacks landingFeedback;

        [SerializeField]
        private Transform playerInputSpace = default;
        [FormerlySerializedAs("ball")] [SerializeField]
        private Transform mainAlienBody = default;

        private PlayerInputCommandHandler _inputCommandHandler;
        private Vector3 _inputVector;

        public static bool WasJumpPressed;
        public static bool IsJumpingPressed;
        public static bool WasJumpReleased;
        public static bool WasInteractPressed;

        [SerializeField, Range(0f, 100f)]
        private float maxClimbSpeed = 4f;

        [SerializeField]
        private float maxRollingSpeed = 8f, maxFloatingSpeed = 12f;
        private float _currentSpeed;

        [SerializeField, Range(0f, 100f)] private float
            maxAcceleration = 10f,
            maxAirAcceleration = 1f,
            maxClimbAcceleration = 40f;

        [SerializeField, Range(0f, 10f)]
        private float jumpHeight = 2f;

        [SerializeField, Range(0, 5)]
        private int maxAirJumps = 0;

        [SerializeField, Range(0, 90)]
        private float maxGroundAngle = 25f, maxStairsAngle = 50f;

        [SerializeField, Range(90, 170)]
        private float maxClimbAngle = 140f;

        [SerializeField, Range(0f, 100f)]
        private float maxSnapSpeed = 100f;

        [SerializeField, Min(0f)]
        private float probeDistance = 1f;

        [SerializeField]
        private LayerMask probeMask = -1, stairsMask = -1, climbMask = -1;

        [SerializeField, Min(0.1f)]
        private float ballRadius = 0.5f;

        [SerializeField, Min(0f)]
        public float ballAlignSpeed = 180f;

        [SerializeField, Min(0f)]
        private float ballAirRotation = 0.5f;

        public Rigidbody body;
        private Rigidbody _connectedBody, _previousConnectedBody;

        private Vector3 _velocity, _desiredVelocity, _connectionVelocity;

        private Vector3 _connectionWorldPosition, _connectionLocalPosition;

        private Vector3 _upAxis, _rightAxis, _forwardAxis;

        private bool _desiredJump, _desiresClimbing;

        private Vector3 _contactNormal, _steepNormal, _climbNormal, _lastClimbNormal;

        private Vector3 _lastContactNormal, _lastSteepNormal, _lastConnectionVelocity;

        private int _groundContactCount, _steepContactCount, _climbContactCount;

        private bool OnGround => _groundContactCount > 0;

        private bool OnSteep => _steepContactCount > 0;

        private bool Climbing => _climbContactCount > 0 && _stepsSinceLastJump > 2;

        private bool _hasJumped = false;

        private int _jumpPhase;

        private float _minGroundDotProduct, _minStairsDotProduct, _minClimbDotProduct;

        private int _stepsSinceLastGrounded, _stepsSinceLastJump;

        private MeshRenderer _meshRenderer;

        public void PreventSnapToGround()
        {
            _stepsSinceLastJump = -1;
        }

        private void OnValidate()
        {
            _minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            _minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
            _minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.useGravity = false;
            _meshRenderer = mainAlienBody.GetComponent<MeshRenderer>();
            OnValidate();

            _inputCommandHandler = gameObject.MMGetComponentAroundOrAdd<PlayerInputCommandHandler>();
            if (_inputCommandHandler == null)
            {
                Debug.LogError("PlayerInputCommandHandler component not found on this game object.");
                return;
            }
            // Retrieve the ShadowController reference
            shadowController = gameObject.MMGetComponentAroundOrAdd<ShadowController>();
            if (shadowController == null)
            {
                Debug.LogError("ShadowController component not found on this game object.");
            }
            // Set currentSpeed to maxRollingSpeed initially
            _currentSpeed = maxRollingSpeed;
        }

        private void Update()
        {
            Vector2 inputVector = _inputCommandHandler.GetMoveInput();
            SetInputVector(inputVector);
            
            /*
            _inputVector.x = inputVector.x;
            _inputVector.z = inputVector.y;
            //Using ClampMagnitude instead of normalized to allow input that is in-between. Because, normalize is a typeof "all-or-nothing input".
            _inputVector = Vector3.ClampMagnitude(_inputVector, 1f);
            */
            
            WasInteractPressed = _inputCommandHandler.GetInteractInput();
            
            // Check if the shift key is currently pressed
            bool isShiftPressed = _inputCommandHandler.GetSprintInput();
            // Set currentSpeed to maxRollingSpeed if the shift key is not pressed, and to maxFloatingSpeed if it is
            _currentSpeed = isShiftPressed ? maxFloatingSpeed : maxRollingSpeed;

            if (playerInputSpace)
            {
                _rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, _upAxis);
                _forwardAxis =
                    ProjectDirectionOnPlane(playerInputSpace.forward, _upAxis);
            }
            else
            {
                _rightAxis = ProjectDirectionOnPlane(Vector3.right, _upAxis);
                _forwardAxis = ProjectDirectionOnPlane(Vector3.forward, _upAxis);
            }
            CalculateDesiredVelocity();

            _desiredJump |= _inputCommandHandler.GetJumpInput();
            _desiresClimbing = _inputCommandHandler.GetClimbInput();

            UpdateBall();
        }
        private void UpdateBall()
        {
            Vector3 rotationPlaneNormal = _lastContactNormal;
            float rotationFactor = 1f;

            if (!OnGround)
            {
                if (OnSteep)
                {
                    rotationPlaneNormal = _lastSteepNormal;
                }
                else
                {
                    rotationFactor = ballAirRotation;
                }
            }

            Vector3 movement =
                (body.velocity - _lastConnectionVelocity) * Time.deltaTime;
            movement -=
                rotationPlaneNormal * Vector3.Dot(movement, rotationPlaneNormal);

            float distance = movement.magnitude;

            Quaternion rotation = mainAlienBody.localRotation;
            if (_connectedBody && _connectedBody == _previousConnectedBody)
            {
                rotation = Quaternion.Euler(
                        _connectedBody.angularVelocity * (Mathf.Rad2Deg * Time.deltaTime)
                    ) *
                    rotation;
                if (distance < 0.001f)
                {
                    mainAlienBody.localRotation = rotation;
                    return;
                }
            }
            else if (distance < 0.001f)
            {
                return;
            }

            float angle = distance * rotationFactor * (180f / Mathf.PI) / ballRadius;
            Vector3 rotationAxis =
                Vector3.Cross(rotationPlaneNormal, movement).normalized;
            rotation = Quaternion.Euler(rotationAxis * angle) * rotation;
            if (ballAlignSpeed > 0f)
            {
                rotation = AlignBallRotation(rotationAxis, rotation, distance);
            }
            mainAlienBody.localRotation = rotation;
        }
        
        
        public void SetInputVector(Vector3 direction)
        {
            _inputVector.x = direction.x;
            _inputVector.y = 0f;
            _inputVector.z = direction.y; // Ensure z component is always 0
            //Using ClampMagnitude instead of normalized to allow input that is in-between. Because, normalize is a typeof "all-or-nothing input".
            _inputVector = Vector3.ClampMagnitude(_inputVector, 1f);
        }

        public void CalculateDesiredVelocity()
        {
            _desiredVelocity = new Vector3(_inputVector.x, 0f, _inputVector.y) * _currentSpeed;
        }

        private Quaternion AlignBallRotation(
            Vector3 rotationAxis, Quaternion rotation, float traveledDistance
        )
        {
            Vector3 ballAxis = mainAlienBody.up;
            float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            float maxAngle = ballAlignSpeed * traveledDistance;

            Quaternion newAlignment =
                Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
            if (angle <= maxAngle)
            {
                return newAlignment;
            }
            else
            {
                return Quaternion.SlerpUnclamped(
                    rotation, newAlignment, maxAngle / angle
                );
            }
        }

        private void FixedUpdate()
        {
            Vector3 gravity = CustomGravityV01.GetGravity(body.position, out _upAxis);
            UpdateState();
            
            

            AdjustVelocity();

            if (_desiredJump)
            {
                _desiredJump = false;
                Jump(gravity);
            }

            if (Climbing)
            {
                _velocity -=
                    _contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
            }
            else if (OnGround && _velocity.sqrMagnitude < 0.01f)
            {
                _velocity +=
                    _contactNormal *
                    (Vector3.Dot(gravity, _contactNormal) * Time.deltaTime);
            }
            else if (_desiresClimbing && OnGround)
            {
                _velocity +=
                    (gravity - _contactNormal * (maxClimbAcceleration * 0.9f)) *
                    Time.deltaTime;
            }
            else
            {
                _velocity += gravity * Time.deltaTime;
            }
            body.velocity = _velocity;
            // Lerp current velocity towards desired velocity
            _velocity = Vector3.Lerp(_velocity, _desiredVelocity, Time.deltaTime * maxAcceleration);
            ClearState();
        }

        #region States

        private void ClearState()
        {
            _lastContactNormal = _contactNormal;
            _lastSteepNormal = _steepNormal;
            _lastConnectionVelocity = _connectionVelocity;
            _groundContactCount = _steepContactCount = _climbContactCount = 0;
            _contactNormal = _steepNormal = _climbNormal = Vector3.zero;
            _connectionVelocity = Vector3.zero;
            _previousConnectedBody = _connectedBody;
            _connectedBody = null;
        }

        private void UpdateState()
        {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;
            _velocity = body.velocity;
            if (
                CheckClimbing() ||
                OnGround ||
                SnapToGround() ||
                CheckSteepContacts()
            )
            {
                _stepsSinceLastGrounded = 0;
                if (_stepsSinceLastJump > 1)
                {
                    _jumpPhase = 0;
                }
                if (_groundContactCount > 1)
                {
                    _contactNormal.Normalize();
                }
            }
            else
            {
                _contactNormal = _upAxis;
            }

            if (_connectedBody)
            {
                if (_connectedBody.isKinematic || _connectedBody.mass >= body.mass)
                {
                    UpdateConnectionState();
                }
            }
            shadowController.UpdateShadowStatus(OnGround);
        }

        private void UpdateConnectionState()
        {
            if (_connectedBody == _previousConnectedBody)
            {
                Vector3 connectionMovement =
                    _connectedBody.transform.TransformPoint(_connectionLocalPosition) -
                    _connectionWorldPosition;
                _connectionVelocity = connectionMovement / Time.deltaTime;
            }
            _connectionWorldPosition = body.position;
            _connectionLocalPosition = _connectedBody.transform.InverseTransformPoint(
                _connectionWorldPosition
            );
        }

        #endregion

        private bool CheckClimbing()
        {
            if (Climbing)
            {
                if (_climbContactCount > 1)
                {
                    _climbNormal.Normalize();
                    float upDot = Vector3.Dot(_upAxis, _climbNormal);
                    if (upDot >= _minGroundDotProduct)
                    {
                        _climbNormal = _lastClimbNormal;
                    }
                }
                _groundContactCount = 1;
                _contactNormal = _climbNormal;
                return true;
            }
            return false;
        }

        private bool SnapToGround()
        {
            if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2)
            {
                return false;
            }
            float speed = _velocity.magnitude;
            if (speed > maxSnapSpeed)
            {
                return false;
            }
            if (!Physics.Raycast(
                    body.position, -_upAxis, out RaycastHit hit,
                    probeDistance, probeMask, QueryTriggerInteraction.Ignore
                ))
            {
                return false;
            }

            float upDot = Vector3.Dot(_upAxis, hit.normal);
            if (upDot < GetMinDot(hit.collider.gameObject.layer))
            {
                return false;
            }

            _groundContactCount = 1;
            _contactNormal = hit.normal;
            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0f)
            {
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
            }
            _connectedBody = hit.rigidbody;
            return true;
        }

        private bool CheckSteepContacts()
        {
            if (_steepContactCount > 1)
            {
                _steepNormal.Normalize();
                float upDot = Vector3.Dot(_upAxis, _steepNormal);
                if (upDot >= _minGroundDotProduct)
                {
                    _steepContactCount = 0;
                    _groundContactCount = 1;
                    _contactNormal = _steepNormal;
                    return true;
                }
            }
            return false;
        }

        
        private void AdjustVelocity()
        {
            float acceleration, speed;
            Vector3 xAxis, zAxis;
            if (Climbing)
            {
                acceleration = maxClimbAcceleration;
                speed = maxClimbSpeed;
                xAxis = Vector3.Cross(_contactNormal, _upAxis);
                zAxis = _upAxis;
            }
            else
            {
                acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
                speed = OnGround && _desiresClimbing ? maxClimbSpeed : _currentSpeed;
                xAxis = _rightAxis;
                zAxis = _forwardAxis;
            }
            
            xAxis = ProjectDirectionOnPlane(xAxis, _contactNormal);
            zAxis = ProjectDirectionOnPlane(zAxis, _contactNormal);

            Vector3 relativeVelocity = _velocity - _connectionVelocity;

            Vector3 adjustment = default;
            adjustment.x =
                _inputVector.x * speed - Vector3.Dot(relativeVelocity, xAxis);
            adjustment.z =
                _inputVector.z * speed - Vector3.Dot(relativeVelocity, zAxis);
            adjustment =
                Vector3.ClampMagnitude(adjustment, acceleration * Time.deltaTime);

            _velocity += xAxis * adjustment.x + zAxis * adjustment.z;
            
        }
        
        public void Jump(Vector3 gravity)
        {
            Vector3 jumpDirection;
            if (OnGround)
            {
                jumpDirection = _contactNormal;
            }
            else if (OnSteep)
            {
                jumpDirection = _steepNormal;
                _jumpPhase = 0;
            }
            else if (maxAirJumps > 0 && _jumpPhase <= maxAirJumps)
            {
                if (_jumpPhase == 0)
                {
                    _jumpPhase = 1;
                }
                jumpDirection = _contactNormal;
            }
            else
            {
                return;
            }

            // play jump feedback here
            jumpFeedback?.PlayFeedbacks();

            _stepsSinceLastJump = 0;
            _jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
            jumpDirection = (jumpDirection + Vector3.up).normalized;
            float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
            if (alignedSpeed > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            _velocity += jumpDirection * jumpSpeed;
            
            // Set hasJumped to true after a successful jump
            _hasJumped = true;
        }

        #region Collision logic

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with the ground layer and if the playerColor has jumped
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _hasJumped)
            {
                // Call PlayFeedbacks() method on the LandingFeedback object
                landingFeedback?.PlayFeedbacks();

                // Set hasJumped back to false after playing the feedback
                _hasJumped = false;
            }

            EvaluateCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            EvaluateCollision(collision);
        }

        private void EvaluateCollision(Collision collision)
        {
            int layer = collision.gameObject.layer;
            float minDot = GetMinDot(layer);
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                if (normal.y >= 0.9f) _groundContactCount = 1;
                float upDot = Vector3.Dot(_upAxis, normal);
                if (upDot >= minDot)
                {
                    _groundContactCount += 1;
                    _contactNormal += normal;
                    _connectedBody = collision.rigidbody;
                }
                else
                {
                    //make -0.01f to activate wall jump on straight walls
                    if (upDot > 0.01f)  
                    {
                        _steepContactCount += 1;
                        _steepNormal += normal;
                        if (_groundContactCount == 0)
                        {
                            _connectedBody = collision.rigidbody;
                        }
                    }
                    if (
                        _desiresClimbing &&
                        upDot >= _minClimbDotProduct &&
                        (climbMask & (1 << layer)) != 0
                    )
                    {
                        _climbContactCount += 1;
                        _climbNormal += normal;
                        _lastClimbNormal = normal;
                        _connectedBody = collision.rigidbody;
                    }
                }
            }
        }
        
        private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }
        
        private float GetMinDot(int layer)
        {
            return (stairsMask & (1 << layer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
        }


        #endregion

        //
        //
        // UNITY EVENT METHODS
        //
        //
        public float GetMaxRollingSpeed()
        {
            return maxRollingSpeed;
        }
        public float GetMaxFloatingSpeed()
        {
            return maxFloatingSpeed;
        }
        public void SetCurrentSpeed(float speed)
        {
            _currentSpeed = speed;
        }
    }
}