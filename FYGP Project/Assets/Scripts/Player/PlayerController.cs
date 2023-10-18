using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GamepadInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0, 20)]
    private float speed;
    [SerializeField] private GameObject gunRef;
    private GamepadInput controllerInput;

    private List<GameObject> previouslyHitObjects = new List<GameObject>();


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
    }

    private void VariableComponents()
    {
        controllerInput = GetComponent<GamepadInput>();
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
        CameraIndexManager.OnPlayerSpawn?.Invoke(GetComponent<PlayerInput>().playerIndex, transform);
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(controllerInput.MovementInput.x, 0, 
            controllerInput.MovementInput.y);

        Vector3 moveOffset = moveDirection * speed * Time.deltaTime;
        transform.position += moveOffset;
    }

    private void HandleRotation()
    {
        if (controllerInput.RotationInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(controllerInput.RotationInput.x, 
                controllerInput.RotationInput.y) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, angle, 0);
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
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
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
            Debug.Log("Changing transparency of " + obj.name + " to " + alpha);
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }
}
