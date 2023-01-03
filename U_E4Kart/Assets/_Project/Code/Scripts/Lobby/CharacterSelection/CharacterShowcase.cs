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

    public void UpdateCharacter(KartVisualController newCharacter, string nickname)
    {
        RemoveCharacterFromSlot();
        
        if (newCharacter != null)
        {
            var kart = Instantiate(newCharacter, characterSlot);
           kart.SetFirstLetter(nickname.ToCharArray()[0].ToString());
        }
    }

    public void EndShowcase()
    {
        anim.SetTrigger(AnimHash_End);
    }
}
