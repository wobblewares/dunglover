using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls application of dung layers to this object.
/// </summary>
[RequireComponent(typeof(Layerable))]
public class DungApplicator : MonoBehaviour
{
    [SerializeField] private LayerConfig _debugLayerConfig;
    
    private Layerable _layerable;

    private void Awake()
    {
        _layerable = GetComponent<Layerable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Apply set layer
            _layerable.Add(_debugLayerConfig);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // Clear layers
            _layerable.Clear();
        }
    }
}
