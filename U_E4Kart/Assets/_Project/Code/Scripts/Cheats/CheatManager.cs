using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatManager : MonoBehaviour
{
    [SerializeField] private CheatData[] availableCheats;
    
    private InputMapping input;

    private void Awake()
    {
        foreach (var cheat in availableCheats)
        {
            cheat.onExecuteCheat += ReinitializeInputs;
        }
        ReinitializeInputs();

        input = new InputMapping();
        input.Enable();

        input.Player.Movement.performed += DirectionalInput;
        input.Player.Accelerate.performed += InputA;
        input.Player.Brake.performed += InputB;
    }

    private void OnDestroy()
    {
        input.Player.Movement.performed -= DirectionalInput;
        input.Player.Accelerate.performed -= InputA;
        input.Player.Brake.performed -= InputB;
        foreach (var cheat in availableCheats)
        {
            cheat.onExecuteCheat -= ReinitializeInputs;
        }
    }

    private void ReinitializeInputs()
    {
        foreach (var cheat in availableCheats)
        {
            cheat.InitializeCheat();
        }
    }

    private void DirectionalInput(InputAction.CallbackContext context)
    {
        var value = input.Player.Movement.ReadValue<Vector2>();
        if(value == new Vector2(1, 0))
            CheatChecker(CheatInput.Right);
        else if (value == new Vector2(-1, 0))
            CheatChecker(CheatInput.Left);
        else if(value == new Vector2(0, 1))
            CheatChecker(CheatInput.Up);
        else if(value == new Vector2(0, -1))
            CheatChecker(CheatInput.Down);
    }

    private void InputA(InputAction.CallbackContext context)
    {
        CheatChecker(CheatInput.Accelerate);
    }
    
    private void InputB(InputAction.CallbackContext context)
    {
        CheatChecker(CheatInput.Brake);
    }

    private void CheatChecker(CheatInput cheatInput)
    {
        foreach (var cheat in availableCheats)
        {
            var completed = cheat.AddInput(cheatInput);
            if (completed)
            {
                UnlockSecretCharacter(cheat);
            }
        }
    }

    private void UnlockSecretCharacter(CheatData data)
    {
        CharacterSelection.onSelectSecretCharacter?.Invoke(data.unlockableCharacter);
    }
}

public enum CheatInput
{
    Up,
    Down,
    Left,
    Right,
    Accelerate,
    Brake,
}