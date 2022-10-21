using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Cheat", menuName = "Cheats/Cheat")]
public class CheatData : ScriptableObject
{
    public CheatInput[] inputSequence;
    public bool[] cheatChecker;

    [Header("temp")]
    public CharacterData unlockableCharacter;

    private int currentCheckerIndex;
    public void InitializeCheat()
    {
        currentCheckerIndex = 0;
        cheatChecker = new bool[inputSequence.Length];
    }

    public bool AddInput(CheatInput input)
    {
        var completed = false;
        if (inputSequence[currentCheckerIndex] == input)
        {
            cheatChecker[currentCheckerIndex] = true;
            currentCheckerIndex++;

            if (currentCheckerIndex >= inputSequence.Length)
            {
                completed = true;
                onExecuteCheat?.Invoke();
            }
        }
        else
        {
            InitializeCheat();
        }

        return completed;
    }
    
    public Action onExecuteCheat;
}