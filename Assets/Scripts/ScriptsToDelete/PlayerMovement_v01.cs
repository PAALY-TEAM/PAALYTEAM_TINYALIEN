using MoreMountains.Feedbacks;
using Movement;
using UnityEngine;

namespace ScriptsToDelete
{ //Refactored script form https://catlikecoding.com/unity/tutorials/movement/
    public class PlayerMovementV01 : MonoBehaviour {
       
    [Header("Feedbacks")]
        /// a MMFeedbacks effects for playerColor interactions
        [SerializeField] private MMFeedbacks jumpFeedback;
        [SerializeField] private MMFeedbacks landingFeedback;
    
        [SerializeField] private Transform playerInputSpace = default, ball = default;

        [SerializeField, Range(0f, 100f)] private float maxSpeed = 10f, maxClimbSpeed = 4f;

        [SerializeField, Range(0f, 100f)] private float
            maxAcceleration = 10f,
            maxAirAcceleration = 1f,
            maxClimbAcceleration = 40f;

        [SerializeField, Range(0f, 10f)] private float jumpHeight = 2f;

        [SerializeField, Range(0, 5)] private int maxAirJumps = 0;

        [SerializeField, Range(0, 90)] private float maxGroundAngle = 25f, maxStairsAngle = 50f;

        [SerializeField, Range(90, 170)] private float maxClimbAngle = 140f;

        [SerializeField, Range(0f, 100f)] private float maxSnapSpeed = 100f;

        [SerializeField, Min(0f)] private float probeDistance = 1f;

        [SerializeField] private LayerMask probeMask = -1, stairsMask = -1, climbMask = -1;
        
        [SerializeField, Min(0.1f)] private float ballRadius = 0.5f;

        [SerializeField, Min(0f)] private float ballAlignSpeed = 180f;

        [SerializeField, Min(0f)] private float
            ballAirRotation = 0.5f;

        private Rigidbody _body, _connectedBody, _previousConnectedBody;

        private Vector3 _playerInput;

        private Vector3 _velocity, _connectionVelocity;

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

        public void PreventSnapToGround () {
            _stepsSinceLastJump = -1;
        }

        private void OnValidate () {
            _minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
            _minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
            _minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
        }

        private void Awake () {
            _body = GetComponent<Rigidbody>();
            _body.useGravity = false;
            _meshRenderer = ball.GetComponent<MeshRenderer>();
            OnValidate();
        }

        private void Update () {
            _playerInput.x = Input.GetAxis("Horizontal");
            _playerInput.z = Input.GetAxis("Vertical");
            _playerInput = Vector3.ClampMagnitude(_playerInput, 1f);

            if (playerInputSpace) {
                _rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, _upAxis);
                _forwardAxis =
                    ProjectDirectionOnPlane(playerInputSpace.forward, _upAxis);
            }
            else {
                _rightAxis = ProjectDirectionOnPlane(Vector3.right, _upAxis);
                _forwardAxis = ProjectDirectionOnPlane(Vector3.forward, _upAxis);
            }
            
                _desiredJump |= Input.GetButtonDown("Jump");
                _desiresClimbing = Input.GetButton("Climb");

            UpdateBall();
        }

        private void UpdateBall () {
            Vector3 rotationPlaneNormal = _lastContactNormal;
            float rotationFactor = 1f;
            
            if (!OnGround) {
                if (OnSteep) {
                    rotationPlaneNormal = _lastSteepNormal;
                }
                else {
                    rotationFactor = ballAirRotation;
                }
            }

            Vector3 movement =
                (_body.velocity - _lastConnectionVelocity) * Time.deltaTime;
            movement -=
                rotationPlaneNormal * Vector3.Dot(movement, rotationPlaneNormal);

            float distance = movement.magnitude;

            Quaternion rotation = ball.localRotation;
            if (_connectedBody && _connectedBody == _previousConnectedBody) {
                rotation = Quaternion.Euler(
                    _connectedBody.angularVelocity * (Mathf.Rad2Deg * Time.deltaTime)
                ) * rotation;
                if (distance < 0.001f) {
                    ball.localRotation = rotation;
                    return;
                }
            }
            else if (distance < 0.001f) {
                return;
            }

            float angle = distance * rotationFactor * (180f / Mathf.PI) / ballRadius;
            Vector3 rotationAxis =
                Vector3.Cross(rotationPlaneNormal, movement).normalized;
            rotation = Quaternion.Euler(rotationAxis * angle) * rotation;
            if (ballAlignSpeed > 0f) {
                rotation = AlignBallRotation(rotationAxis, rotation, distance);
            }
            ball.localRotation = rotation;
        }

        private Quaternion AlignBallRotation (
            Vector3 rotationAxis, Quaternion rotation, float traveledDistance
        ) {
            Vector3 ballAxis = ball.up;
            float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            float maxAngle = ballAlignSpeed * traveledDistance;

            Quaternion newAlignment =
                Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
            if (angle <= maxAngle) {
                return newAlignment;
            }
            else {
                return Quaternion.SlerpUnclamped(
                    rotation, newAlignment, maxAngle / angle
                );
            }
        }

        private void FixedUpdate () {
            Vector3 gravity = CustomGravityV01.GetGravity(_body.position, out _upAxis);
            UpdateState();

            AdjustVelocity();

            if (_desiredJump)
            {
                _desiredJump = false;
                Jump(gravity);
            }
            
            if (Climbing) {
                _velocity -=
                    _contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
            }
            else if (OnGround && _velocity.sqrMagnitude < 0.01f) {
                _velocity +=
                    _contactNormal *
                    (Vector3.Dot(gravity, _contactNormal) * Time.deltaTime);
            }
            else if (_desiresClimbing && OnGround) {
                _velocity +=
                    (gravity - _contactNormal * (maxClimbAcceleration * 0.9f)) *
                    Time.deltaTime;
            }
            else {
                _velocity += gravity * Time.deltaTime;
            }
            _body.velocity = _velocity;
            ClearState();
        }

        private void ClearState () {
            _lastContactNormal = _contactNormal;
            _lastSteepNormal = _steepNormal;
            _lastConnectionVelocity = _connectionVelocity;
            _groundContactCount = _steepContactCount = _climbContactCount = 0;
            _contactNormal = _steepNormal = _climbNormal = Vector3.zero;
            _connectionVelocity = Vector3.zero;
            _previousConnectedBody = _connectedBody;
            _connectedBody = null;
        }

        private void UpdateState () {
            _stepsSinceLastGrounded += 1;
            _stepsSinceLastJump += 1;
            _velocity = _body.velocity;
            if (
                CheckClimbing() ||
                OnGround || SnapToGround() || CheckSteepContacts()
            ) {
                _stepsSinceLastGrounded = 0;
                if (_stepsSinceLastJump > 1) {
                    _jumpPhase = 0;
                }
                if (_groundContactCount > 1) {
                    _contactNormal.Normalize();
                }
            }
            else {
                _contactNormal = _upAxis;
            }
		
            if (_connectedBody) {
                if (_connectedBody.isKinematic || _connectedBody.mass >= _body.mass) {
                    UpdateConnectionState();
                }
            }
        }

        private void UpdateConnectionState () {
            if (_connectedBody == _previousConnectedBody) {
                Vector3 connectionMovement =
                    _connectedBody.transform.TransformPoint(_connectionLocalPosition) -
                    _connectionWorldPosition;
                _connectionVelocity = connectionMovement / Time.deltaTime;
            }
            _connectionWorldPosition = _body.position;
            _connectionLocalPosition = _connectedBody.transform.InverseTransformPoint(
                _connectionWorldPosition
            );
        }

        private bool CheckClimbing () {
            if (Climbing) {
                if (_climbContactCount > 1) {
                    _climbNormal.Normalize();
                    float upDot = Vector3.Dot(_upAxis, _climbNormal);
                    if (upDot >= _minGroundDotProduct) {
                        _climbNormal = _lastClimbNormal;
                    }
                }
                _groundContactCount = 1;
                _contactNormal = _climbNormal;
                return true;
            }
            return false;
        }

        private bool SnapToGround () {
            if (_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <= 2) {
                return false;
            }
            float speed = _velocity.magnitude;
            if (speed > maxSnapSpeed) {
                return false;
            }
            if (!Physics.Raycast(
                    _body.position, -_upAxis, out RaycastHit hit,
                    probeDistance, probeMask, QueryTriggerInteraction.Ignore
                )) {
                return false;
            }

            float upDot = Vector3.Dot(_upAxis, hit.normal);
            if (upDot < GetMinDot(hit.collider.gameObject.layer)) {
                return false;
            }

            _groundContactCount = 1;
            _contactNormal = hit.normal;
            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0f) {
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
            }
            _connectedBody = hit.rigidbody;
            return true;
        }

        private bool CheckSteepContacts () {
            if (_steepContactCount > 1) {
                _steepNormal.Normalize();
                float upDot = Vector3.Dot(_upAxis, _steepNormal);
                if (upDot >= _minGroundDotProduct) {
                    _steepContactCount = 0;
                    _groundContactCount = 1;
                    _contactNormal = _steepNormal;
                    return true;
                }
            }
            return false;
        }

        private void AdjustVelocity () {
            float acceleration, speed;
            Vector3 xAxis, zAxis;
            if (Climbing) {
                acceleration = maxClimbAcceleration;
                speed = maxClimbSpeed;
                xAxis = Vector3.Cross(_contactNormal, _upAxis);
                zAxis = _upAxis;
            }
            else {
                acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
                speed = OnGround && _desiresClimbing ? maxClimbSpeed : maxSpeed;
                xAxis = _rightAxis;
                zAxis = _forwardAxis;
            }
            xAxis = ProjectDirectionOnPlane(xAxis, _contactNormal);
            zAxis = ProjectDirectionOnPlane(zAxis, _contactNormal);

            Vector3 relativeVelocity = _velocity - _connectionVelocity;

            Vector3 adjustment = default;
            adjustment.x =
                _playerInput.x * speed - Vector3.Dot(relativeVelocity, xAxis);
            adjustment.z =
                _playerInput.z * speed - Vector3.Dot(relativeVelocity, zAxis);
            adjustment =
                Vector3.ClampMagnitude(adjustment, acceleration * Time.deltaTime);

            _velocity += xAxis * adjustment.x + zAxis * adjustment.z;
            
        }

        private void Jump (Vector3 gravity) {
            Vector3 jumpDirection;
            if (OnGround) {
                jumpDirection = _contactNormal;
            }
            else if (OnSteep) {
                jumpDirection = _steepNormal;
                _jumpPhase = 0;
            }
            else if (maxAirJumps > 0 && _jumpPhase <= maxAirJumps) {
                if (_jumpPhase == 0) {
                    _jumpPhase = 1;
                }
                jumpDirection = _contactNormal;
            }
            else {
                return;
            }
            
            // play jump feedback here
            jumpFeedback?.PlayFeedbacks();
            
            _stepsSinceLastJump = 0;
            _jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
            jumpDirection = (jumpDirection + _upAxis).normalized;
            float alignedSpeed = Vector3.Dot(_velocity, jumpDirection);
            if (alignedSpeed > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            _velocity += jumpDirection * jumpSpeed;
             // Set hasJumped to true after a successful jump
            _hasJumped = true;
        }

        private void OnCollisionEnter (Collision collision) {
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

        private void OnCollisionStay (Collision collision) {
            EvaluateCollision(collision);
        }

        private void EvaluateCollision (Collision collision) {
            int layer = collision.gameObject.layer;
            float minDot = GetMinDot(layer);
            for (int i = 0; i < collision.contactCount; i++) {
                Vector3 normal = collision.GetContact(i).normal;
                float upDot = Vector3.Dot(_upAxis, normal);
                if (upDot >= minDot) {
                    _groundContactCount += 1;
                    _contactNormal += normal;
                    _connectedBody = collision.rigidbody;
                }
                else {
                    if (upDot > -0.01f) {
                        _steepContactCount += 1;
                        _steepNormal += normal;
                        if (_groundContactCount == 0) {
                            _connectedBody = collision.rigidbody;
                        }
                    }
                    if (
                        _desiresClimbing && upDot >= _minClimbDotProduct &&
                        (climbMask & (1 << layer)) != 0
                    ) {
                        _climbContactCount += 1;
                        _climbNormal += normal;
                        _lastClimbNormal = normal;
                        _connectedBody = collision.rigidbody;
                    }
                }
            }
        }

        private Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }

        private float GetMinDot (int layer) {
            return (stairsMask & (1 << layer)) == 0 ?
                _minGroundDotProduct : _minStairsDotProduct;
        }
    }
}
