using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    public Sprite characterIcon;
    public string characterName;
    public BaseKart characterPrefab;

    [Space]
    public AudioClip sfx_OnSelectCharacter;
}