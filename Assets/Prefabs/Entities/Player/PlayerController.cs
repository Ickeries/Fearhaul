using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public float lerpedHealth = 100.0f;
    [SerializeField]
    private Weapon currentWeapon;
    [SerializeField]
    private PlayerCamera camera;
    [SerializeField]
    private float boatSpeed = 10f;
    [SerializeField]
    private float boatRotationSpeed = 15f;
    [SerializeField]
    private float boatChargeSpeedMultiplier = 2.0f;
    public Animator animator;
    // States
    private enum States {idle, move, charge, aim}
    private States state = States.idle;

    // Inputs
    private PlayerInput playerInput;
    private InputAction move;
    private InputAction jump;
    private InputAction fire;
    private InputAction aim;
    private InputAction look;
    private InputAction charge;
    private InputAction interact;

    private Rigidbody rigidbody;
    private Buoyancy buoyancy;


    private Vector2 aimPosition;

    public Transform weaponsTransform;
    public Transform testTransform;

    [SerializeField] private LayerMask aimLayerMask;

    private GameObject target;
    [SerializeField] private AudioClip[] chargeHitSounds;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private AudioSource motorAudioSource;
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI dialoguePopup;
    [SerializeField] Animator dialoguePopupAnimator;

    bool aiming = false;
    void Start()
    {
        playerInput = this.GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        jump = playerInput.actions["Jump"];
        fire = playerInput.actions["Fire"];
        aim  = playerInput.actions["Aim"];
        look = playerInput.actions["Look"];
        charge = playerInput.actions["Charge"];
        interact = playerInput.actions["Interact"];
        rigidbody = this.GetComponent<Rigidbody>();
        buoyancy = GetComponent<Buoyancy>();
    }


    public Rigidbody getRigidbody() 
    { 
        return rigidbody; 
    }


    void FixedUpdate()
    {
        lerpedHealth = Mathf.Lerp(lerpedHealth, (float)health, 2.5f * Time.deltaTime);
        if (lerpedHealth <= 0.0f)
        {
            this.gameObject.SetActive(false);
        }
        Vector2 movement = move.ReadValue<Vector2>();

        // State specific logic
        switch(state)
        { 
            case States.idle:
                if (movement.y != 0.0f)
                {
                    changeState(States.move);
                }
                motorAudioSource.volume = Mathf.Lerp(motorAudioSource.volume, 0.0f, 2.5f * Time.deltaTime);
                motorAudioSource.pitch = Mathf.Lerp(motorAudioSource.pitch, 0.5f, 2.5f * Time.deltaTime);
                //modelTransform.eulerAngles = Vector3.Lerp(modelTransform.eulerAngles, new Vector3(0f, modelTransform.eulerAngles.y, modelTransform.eulerAngles.z), 2.0f * Time.deltaTime);
                break; 
            case States.move:
                if (movement.y == 0.0f)
                {
                    changeState(States.idle);
                }
                if (charge.ReadValue<float>() == 1.0f)
                {
                    changeState(States.charge);
                }
                motorAudioSource.pitch = Mathf.Lerp(motorAudioSource.pitch, 1.5f, 2.5f * Time.deltaTime);
                motorAudioSource.volume = Mathf.Lerp(motorAudioSource.volume, 0.3f, 2.5f * Time.deltaTime);
                //modelTransform.eulerAngles = Vector3.Lerp(modelTransform.eulerAngles, new Vector3(10f, modelTransform.eulerAngles.y, modelTransform.eulerAngles.z), 2.0f * Time.deltaTime);
                if (buoyancy.is_underwater() == true)
                {
                    rigidbody.AddRelativeTorque(new Vector3(90f, 0, 0), ForceMode.Force);
                    rigidbody.AddForce(transform.forward * movement.y * boatSpeed, ForceMode.Force);
                }
                break;
            case States.charge:
                if (movement.y == 0.0f)
                {
                    changeState(States.idle);
                }
                if (charge.ReadValue<float>() == 0.0f)
                {
                    changeState(States.move);
                }
                motorAudioSource.volume = Mathf.Lerp(motorAudioSource.volume, 0.3f, 2.5f * Time.deltaTime);
                motorAudioSource.pitch = Mathf.Lerp(motorAudioSource.pitch, 2.5f, 2.5f * Time.deltaTime);
                if (buoyancy.is_underwater() == true)
                {
                    rigidbody.AddRelativeTorque(new Vector3(90f, 0, 0), ForceMode.Force);
                    rigidbody.AddForce(transform.forward * movement.y * boatSpeed * boatChargeSpeedMultiplier, ForceMode.Force);
                }
                break;
            case States.aim:
                break;
        }

        if (movement.x != 0.0f)
        {
            rigidbody.AddRelativeTorque(new Vector3(0, 0, 20f * movement.x), ForceMode.Force);
            rigidbody.AddTorque(new Vector3(0.0f, movement.x * 100.0f, 0.0f));
        }
        // Aiming
        Plane plane = new Plane(Vector3.up, weaponsTransform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300.0f, aimLayerMask))
        {
            if (aim.ReadValue<float>() == 1.0f)
            {
                target = hit.collider.gameObject;
            }
            if (interact.WasPressedThisFrame())
            {
                if (hit.collider.GetComponent<Dialogue>() != null)
                {
                    dialoguePopup.text = hit.collider.GetComponent<Dialogue>().getText();
                    dialoguePopupAnimator.Play("popup", 0, 0.0f);
                }
            }
        }
        if (aim.ReadValue<float>() == 0.0f)
        {
            target = null;
        }

        if (aiming == true)
        {
            if (target == null)
            {
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    Vector3 weaponDirection = (hitPoint - weaponsTransform.position).normalized;
                    weaponsTransform.forward = Vector3.Lerp(weaponsTransform.forward, new Vector3(weaponDirection.x, 0.0f, weaponDirection.z), 15.0f * Time.deltaTime);
                }
            }
            else
            {
                weaponsTransform.LookAt(new Vector3(target.transform.position.x, 0.0f, target.transform.position.z));
            }
        }
        else
        {
            weaponsTransform.forward = Vector3.Lerp(weaponsTransform.forward, this.transform.forward, 5.0f * Time.deltaTime);
        }


        // Firing
        if (fire.ReadValue<float>() == 1.0f)
        {
            if (aiming == true)
            {
                currentWeapon.Fire();
            }
        }


        // Jumping
        if (jump.ReadValue<float>() == 1.0f && buoyancy.is_underwater() && aiming == false)
        {
            rigidbody.AddForce(new Vector3(0.0f, 5.0f, 0.0f), ForceMode.Impulse);
        }


        //Aiming
        if(aim.ReadValue<float>() == 1.0f)
        {
            aiming = true;
        }
        else
        {
            aiming = false;
        }
    }

    void changeState(States newState)
    {
        switch(newState)
        { 
            case States.idle:
                Camera.main.GetComponent<PlayerCamera>().setFov(50f);
                break;
            case States.move:
                Camera.main.GetComponent<PlayerCamera>().setFov(55f);
                break;
            case States.charge:
                Camera.main.GetComponent<PlayerCamera>().setFov(65f);
                break;
        }
        state = newState;
    }

    public GameObject getTarget()
    {
        return target;
    }

    Vector3 flattenedForward()
    {
        return new Vector3(transform.forward.x, 0.0f, transform.forward.z);
    }

    public Vector3 getMovementVector()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        Vector3 forward = Camera.main.transform.forward * moveInput.y;
        Vector3 right = Camera.main.transform.right * moveInput.x;
        Vector3 result = (forward + right).normalized;
        return new Vector3(result.x, 0.0f, result.z);
    }

    public Vector3 getLookVector()
    {
        Vector2 lookInput = Input.mousePosition - Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 forward = Camera.main.transform.forward * lookInput.y;
        Vector3 right = Camera.main.transform.right * lookInput.x;
        Vector3 result = (forward + right);
        return new Vector3(result.x, 0.0f, result.z);
    }

    public void hurt(int amount)
    {
        animator.Play("hurt", 0, 0.0f);
        health -= amount;
    }

    public Vector3 getAimPosition()
    {
        if (aiming == true)
            return transform.position + weaponsTransform.forward * 8.0f;
        else
        {
            return transform.position;
        }

    }

    void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Stats>() != null)
        {
            Stats stats = other.collider.GetComponent<Stats>();
            if (stats.setKnockBack((other.transform.position - this.transform.position).normalized * 16.0f) == true)
            {
                AudioSource.PlayClipAtPoint(chargeHitSounds[Random.Range(0, chargeHitSounds.Length-1)], this.transform.position, 1.0f);
                stats.addHealth(-50f);
            }
        }
    }
}