using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0, 20)]
    private float speed;

    private GamepadInput controllerInput;

    private void Awake()
    {
        controllerInput = GetComponent<GamepadInput>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        CheckShooting();
    }

    private void CheckShooting()
    {
        if (controllerInput.ShootValue > 0.5f)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        //Add Shooting Function
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(controllerInput.CurrentMovementInput.x, 0, controllerInput.CurrentMovementInput.y);
        Vector3 moveOffset = moveDirection * speed * Time.deltaTime;
        transform.position += moveOffset;
    }

    private void HandleRotation()
    {
        if (controllerInput.CurrentRotationInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(controllerInput.CurrentRotationInput.x, controllerInput.CurrentRotationInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
