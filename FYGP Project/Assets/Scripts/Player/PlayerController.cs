using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GamepadInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0, 20)] private float speed;
    [SerializeField, Range(0, 10)] private float playerHeightOffset;
    public float playerHealth;

    [SerializeField] private GameObject gunRef;

    private Rigidbody rb;
    private GamepadInput controllerInput;
    private PlayerCamera playerCamera;
    private List<GameObject> previouslyHitObjects = new List<GameObject>();

    [SerializeField] private float raycastLength;
    private float gravitationalForce = -9.81f;
    
    private bool isGrounded = false;

    private void Start()
    {
        VariableComponents();
        AnnouncePlayerSpawn();
        StartCoroutine(ActivatePlayerInput());
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        CheckShooting();
        HandleObstruction();
        EnsureGrounded();
    }

    private void VariableComponents()
    {
        controllerInput = GetComponent<GamepadInput>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        rb = GetComponent<Rigidbody>();
    }

    private void CheckShooting()
    {
        if (controllerInput.RightTrigger > 0.5f)
        {
            ShootingFunction();
        }
    }

    private IEnumerator ActivatePlayerInput()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<PlayerInput>().enabled = true;
    }

    private void ShootingFunction()
    {
        gunRef.GetComponent<GunBase>().Fire();
    }

    private void AnnouncePlayerSpawn()
    {
        GamepadInputManager.OnPlayerSpawn?.Invoke(GetComponent<PlayerInput>().playerIndex, transform);
    }

    private void HandleMovement()
    {
        Vector3 moveDir = new Vector3(controllerInput.MovementInput.x, 
            0, controllerInput.MovementInput.y);

        Vector3 moveOffset = moveDir * speed * Time.deltaTime;
        Vector3 newPos = transform.position + moveOffset;
        Vector3 viewPos = Camera.main.WorldToViewportPoint(newPos);

        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);

        newPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = newPos;

        RestrictMovementWithinCameraView();
    }

    private void HandleRotation()
    {
        if (controllerInput.RotationInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(controllerInput.RotationInput.x,
                controllerInput.RotationInput.y) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(-90f, angle, 0);
        }
    }

    private void HandleObstruction()
    {
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        RaycastHit[] hits;

        hits = Physics.RaycastAll(Camera.main.transform.position, direction, distance);
        Debug.DrawRay(Camera.main.transform.position, direction * distance, Color.red);

        List<GameObject> currentHitObjects = new List<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ObstructingWalls"))
            {
                //Debug.Log("Hit object: " + hit.collider.gameObject.name);
                ChangeTransparency(hit.collider.gameObject, 0.5f);
                currentHitObjects.Add(hit.collider.gameObject);
            }
        }

        foreach (GameObject obj in previouslyHitObjects)
        {
            if (!currentHitObjects.Contains(obj))
            {
                ChangeTransparency(obj, 1f);
            }
        }

        previouslyHitObjects = currentHitObjects;
    }

    private void ChangeTransparency(GameObject obj, float alpha)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            //Debug.Log("Changing transparency of " + obj.name + " to " + alpha);
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    private void RestrictMovementWithinCameraView()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp(viewPos.x, playerCamera.leftBoundaryOffset, 
            1 - playerCamera.rightBoundaryOffset);

        viewPos.y = Mathf.Clamp(viewPos.y, playerCamera.bottomBoundaryOffset, 
            1 - playerCamera.topBoundaryOffset);

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    public void ApplyDamage(float damageAmount)
    {
        playerHealth -= damageAmount;

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void EnsureGrounded()
    {
        RaycastHit hit;
        LayerMask ground = LayerMask.GetMask("Ground");
        Vector3 rayStart = transform.position;
        Vector3 rayDir = -Vector3.up;

        Color rayColour = isGrounded ? Color.yellow : Color.red;
        Debug.DrawRay(rayStart, rayDir, rayColour);

        if (Physics.Raycast(rayStart, rayDir, out hit, raycastLength, ground))
        {
            if (!isGrounded)
            {
                isGrounded = true;
            }

            transform.position = hit.point + Vector3.up * playerHeightOffset;
        }
        else
        {
            if (isGrounded)
            {
                isGrounded = false;
            }

            ApplyGravitationalForce();
        }
    }

    private void ApplyGravitationalForce()
    {
        rb.AddForce(new Vector3(0, gravitationalForce * Time.deltaTime, 0), 
            ForceMode.VelocityChange);
    }

    private void Die()
    {
        Debug.Log("Player Died!");
    }
}