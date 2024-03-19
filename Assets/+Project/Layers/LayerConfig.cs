using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dunglover/Layers/New Layer Config")]
public class LayerConfig : ScriptableObject
{
    [SerializeField] private Color color;
    [SerializeField] private PhysicMaterial material;

    public Color Color => color;
    public PhysicMaterial Material => material;
}
