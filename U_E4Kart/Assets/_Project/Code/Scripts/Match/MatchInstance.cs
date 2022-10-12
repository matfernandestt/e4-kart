using System;
using UnityEngine;

public class MatchInstance : MonoBehaviour
{
    public static Match CurrentMatch;

    public static void NewMatch() { CurrentMatch = new Match(); }
}

public class Match
{
    public bool playerIsLeader;

    public Action onStartMatch;
}
