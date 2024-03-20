using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Layerable))]
public class Ball : MonoBehaviour
{
    private Layerable _layerable;
    
    private void Awake()
    {
        _layerable = GetComponent<Layerable>();
    }

    private void OnTriggerEnter(Collider collider)
    {
      if (collider.GetComponent<LayerApplyer>() is {} controllable)
      {
          _layerable.Clear();
          _layerable.Add(controllable.Layer);
      }
    }
}
