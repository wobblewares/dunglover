#nullable enable

using System;
using UnityEngine;
using UnityEngine.Serialization;
using Wobblewares.Prototyping;

public class Beetle : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float _attachDistance = 0.75f;
    [SerializeField] private float _detachForce = 10.0f;
    [SerializeField] private float _positionLerp = 10.0f;
    [SerializeField] private float _rotationLerp = 10.0f;
    #endregion

    #region Private
    private InputController _inputController;
    private ControllableActor _controllableActor;
    private ControllableActor? _controlledActor = null;
    private PhysicalBody _physicalBody = null;
    private float _lastDetachTime = 0.0f;
    private const float minimumTimeBeforeReattach = 0.25f;
    #endregion
    
    public bool IsAttached => _controlledActor != null;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputController = FindObjectOfType<InputController>();
        _controllableActor = GetComponent<ControllableActor>();
        _physicalBody = GetComponent<PhysicalBody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsAttached)
        {
            // current direction
            Vector3 currVecToBall = (_controllableActor.Rigidbody.position - _controlledActor.Rigidbody.position).normalized * _attachDistance;
            Vector3 desiredVecToBall = (_controlledActor.Rigidbody.velocity.normalized + _controlledActor.GetComponent<PhysicalBody>().desiredDirection).normalized * _attachDistance;
            Vector3 targetDirection = Vector3.Lerp(currVecToBall, desiredVecToBall, _positionLerp * Time.deltaTime);
            

            Quaternion targetRotation = Quaternion.LookRotation(-targetDirection, Vector3.up);
            // Move the beetle position based on the velocity of the controlled actor
            _controllableActor.Rigidbody.position = _controlledActor.transform.position + targetDirection.normalized * _attachDistance;
            _controllableActor.Rigidbody.rotation = Quaternion.Slerp(_controllableActor.Rigidbody.rotation, targetRotation, Time.deltaTime * _rotationLerp);
        }   
    }

    private void Update()
    {
        // Temporarily detach on E key.
        if (Input.GetKeyDown(KeyCode.E))
        {
            Detach();
        }
    }

    private void Attach(ControllableActor actor)
    {
        
        // don't reattach if we've just detached
        if(Time.time - _lastDetachTime < minimumTimeBeforeReattach) return;
        
        if (IsAttached) return; // Already attached
        
        // Transfer control
        _inputController.RemoveControllable(_controllableActor);
        _inputController.AddControllable(actor);
        
        // Set controlled actor
        _controlledActor = actor;
        
        // Attach to the controllable
        transform.SetParent(actor.gameObject.transform);
        _physicalBody.TogglePhysics(false);
    }

    private void Detach()
    {
        if (!IsAttached) return;
        
        // Transfer control back
        _inputController.RemoveControllable(_controlledActor);
        _inputController.AddControllable(_controllableActor);

        // Detach from the controllable
        transform.SetParent(null);
        _physicalBody.TogglePhysics(true);
        _controllableActor.Rigidbody.velocity = Vector3.zero;
        
        // Apply a split force
        _controllableActor.Rigidbody.AddForce(transform.forward * -_detachForce, ForceMode.Impulse);
        
        // Set controlled actor to null
        _controlledActor = null;
        _lastDetachTime = Time.time;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;
        
        if (collision.rigidbody.GetComponent<ControllableActor>() is {} controllable)
        {
            Attach(controllable);
        }
    }
}
