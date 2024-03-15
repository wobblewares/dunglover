using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wobblewares.Prototyping;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private ControllableActor initialControllable;

    private void Awake()
    {
        if (inputController != null && initialControllable != null)
        {
            inputController.AddControllable(initialControllable);
        }
    }
}
