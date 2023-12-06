using UnityEngine;
/// <summary>
/// This class is based off of the MMFollowTarget script from the MoreMountains Tools.
/// I've extracted only the parts that I need for this project and made some modifications.
/// </summary>
public class CustomHeadMovement : MonoBehaviour
{
    public Transform target;
    [Range(0, 1)]
    public float followSpeed = 1f;
    // The damping applied to the spring motion, affecting how quickly the head will stop bouncing
    public float springDamping = 0.3f;
    public float springDampingY = 0.6f;
    // The frequency of the spring motion, affecting how quickly the head will bounce
    public float springFrequency = 3f;

    private Vector3 _velocity = Vector3.zero;

    private Vector3 _initialPosition;
    private Vector3 _offset;

    private void Start()
    {
        Initialization();
    }

    /// <summary>
    /// Initializes the initial position and offset of this GameObject relative to the target.
    /// </summary>
    private void Initialization()
    {
        _initialPosition = transform.position;

        if (target != null)
        {
            _offset = transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // Calculate the target position and current position
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;

        // Apply spring motion to follow the target
        Spring(ref currentPosition, targetPosition, ref _velocity, springDamping, springDampingY, springFrequency, followSpeed, Time.deltaTime);
        transform.position = currentPosition;
    }

    // Applies a spring motion to the current value to make it follow the target value, based on the MMMaths.Spring() method
    private void Spring(ref Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float damping, float dampingY, float frequency, float speed, float deltaTime)
    {
        Vector3 initialVelocity = velocity;
        velocity.x = SpringVelocity(currentValue.x, targetValue.x, velocity.x, damping, frequency, speed, deltaTime);
        velocity.y = SpringVelocity(currentValue.y, targetValue.y + _offset.y, velocity.y, dampingY, frequency, speed, deltaTime); // Use dampingY for Y velocity
        velocity.z = SpringVelocity(currentValue.z, targetValue.z, velocity.z, damping, frequency, speed, deltaTime);
        velocity.x = Lerp(initialVelocity.x, velocity.x, speed, Time.deltaTime);
        velocity.y = Lerp(initialVelocity.y, velocity.y, speed, Time.deltaTime);
        velocity.z = Lerp(initialVelocity.z, velocity.z, speed, Time.deltaTime);
        currentValue += deltaTime * velocity;
    }

    // Calculates the velocity for the spring motion
    private float SpringVelocity(float currentValue, float targetValue, float velocity, float damping, float frequency, float speed, float deltaTime)
    {
        float difference = targetValue - currentValue;
        velocity += difference * frequency * frequency * deltaTime;
        velocity *= 1 - damping * deltaTime;
        return velocity;
    }

    // Linearly interpolates between the start value and end value by the specified speed
    private float Lerp(float startValue, float endValue, float speed, float deltaTime)
    {
        return (1 - speed) * startValue + speed * endValue;
    }
}