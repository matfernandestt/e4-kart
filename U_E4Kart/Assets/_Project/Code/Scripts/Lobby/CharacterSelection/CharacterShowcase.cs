using UnityEngine;

public class CharacterShowcase : MonoBehaviour
{
    [SerializeField] private Transform characterSlot;
    [SerializeField] private Animator anim;
    
    private static readonly int AnimHash_End = Animator.StringToHash("End");

    private void Awake()
    {
        RemoveCharacterFromSlot();
    }

    private void RemoveCharacterFromSlot()
    {
        foreach (Transform child in characterSlot)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateCharacter(GameObject newCharacter)
    {
        RemoveCharacterFromSlot();
        
        if (newCharacter != null)
        {
            Instantiate(newCharacter, characterSlot);
        }
    }

    public void EndShowcase()
    {
        anim.SetTrigger(AnimHash_End);
    }
}
