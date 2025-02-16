using UnityEngine;

public class Oscillator : MonoBehaviour
{
    float movementFactor;
    Vector3 startPosition;
    Vector3 endPosition;
    [SerializeField] float speed = 2f;
    [SerializeField] Vector3 movemenVector;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movemenVector;
    }

    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
