using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Linq;

namespace Wobblewares.Prototyping
{
    /// <summary>
    /// Component used for controlling an object.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        #region Private

        [SerializeField] private bool getControllablesOnInit = false;
        private List<IControllable> controllableObjects = new List<IControllable>();

        #endregion

        #region Public API

        public void AddControllable(IControllable controllable) => controllableObjects.Add(controllable);
        
        public void RemoveControllable(IControllable controllable) => controllableObjects.Remove(controllable);
        
        /// <summary>
        /// Invoked when moving
        /// </summary>
        public Action<Vector3> Move;

        #endregion

        #region Private

        private Vector2 movementInput = Vector3.zero;

        #endregion

        #region Unity Functions

        public void OnMove(InputValue input)
        {
            if (!enabled)
                return;

            movementInput = input.Get<Vector2>();
        }

        public void OnJump()
        {
            foreach (var controllable in controllableObjects)
                controllable.Jump();
        }

        private void Awake()
        {
            if(getControllablesOnInit)
                controllableObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IControllable>().ToList();
        }

        private void FixedUpdate()
        {
            if (movementInput.magnitude > 0.01f)
            {
                foreach (var controllable in controllableObjects)
                    controllable.Move(movementInput);
            }
        }

        #endregion
    }
}