#nullable enable

using System.Collections.Generic;
using UnityEngine;

public class Layerable : MonoBehaviour
{
    public List<LayerConfig> layers = default;

    public void Add(LayerConfig layer)
    {
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
