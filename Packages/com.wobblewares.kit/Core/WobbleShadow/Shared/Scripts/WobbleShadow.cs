using System;

namespace Wobblewares.Kit
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WobbleShadow : MonoBehaviour
    {
        #region Inspector Variables

        public enum BoundsMode
        {
            MeshRenderer,
            Collider
        }
        
        public BoundsMode boundsMode = BoundsMode.MeshRenderer;
        public Transform target;
        public float offset = 0.1f;
        public Vector3 direction = Vector3.down;
        public bool useLocalBounds = false;
        #endregion

        #region Public API

        
        #endregion

        #region Private

        private void Awake()
        {
            if (target == null)
                target = transform.parent;

            RefreshReferences();
            UpdateShadowPosition();
        }

        private void Update()
        {
            UpdateShadowPosition();
        }

        private void OnValidate()
        {
            if (target == null) return;
            
            // rename automatically
            gameObject.name = $"WobbleShadow - {target.name}";
            RefreshReferences();
        }

        private void RefreshReferences()
        {
            _meshRenderer = target.GetComponent<MeshRenderer>();
            _collider = target.GetComponent<Collider>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
          
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            if (target == null)
                target = transform.parent;
            
            UpdateShadowPosition();
        }

        private Bounds GetBounds()
        {
            if (target == null)
                return new Bounds();
            
            switch (boundsMode) 
            {
                case BoundsMode.MeshRenderer:
                    if (_meshRenderer == null) return new Bounds();

                    if (!useLocalBounds)
                        return _meshRenderer.bounds;
                    Bounds bounds = _meshRenderer.localBounds;
                    bounds.center += target.transform.position;
                    bounds.size = new Vector3(bounds.size.x * target.transform.lossyScale.x,
                        bounds.size.y * target.transform.lossyScale.y,
                        bounds.size.z * target.transform.lossyScale.z);
                    return bounds;
                case BoundsMode.Collider:
                    if (_collider == null) return new Bounds();
                    return _collider.bounds;
            }
            return new Bounds();
        }
        
        private void UpdateShadowPosition()
        {
            if (target == null)
                return;

            bounds = GetBounds();
            transform.position = new Vector3(bounds.center.x, (bounds.center.y - bounds.size.y / 2.0f) - offset, bounds.center.z);
            transform.rotation = Quaternion.LookRotation(Vector3.down, target.transform.forward);
        }
        

        private Bounds bounds;
        private MeshRenderer _meshRenderer = null;
        private Collider _collider = null;

        #endregion
    }
}