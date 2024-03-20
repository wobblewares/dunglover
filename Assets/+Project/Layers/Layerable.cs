#nullable enable

using System.Collections.Generic;
using UnityEngine;

public class Layerable : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Collider _collider;
    
    
    public List<LayerConfig> layers = default;

    public void Add(LayerConfig layer)
    {
        _renderer.material.SetColor("_Color", layer.Color);
        _renderer.material.SetColor("_Shadow_Color", layer.Color * 0.9f);
        _collider.material = layer.Material;
        layers.Add(layer);
    }

    public void Remove(LayerConfig layer)
    {
        if (!layers.Contains(layer))
        {
            Debug.LogError("Attempting to remove layer that is not attached.");
            return;
        }

        layers.Remove(layer);
    }

    public void Clear()
    {
        layers.Clear();
    }
}
