using UnityEngine;

public class Transitioner : MonoBehaviour
{
    [SerializeField] private Animator transitionAnim;

    public static bool IsTransitioning;
    private static Transitioner i;
    
    private static readonly int AnimHash_Transition = Animator.StringToHash("Transition");
    private static readonly int AnimHash_FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int AnimHash_FadeOut = Animator.StringToHash("FadeOut");

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public static void Begin()
    {
        if (IsTransitioning)
        {
            Debug.LogWarning("Already transitioning!");
            return;
        }
        IsTransitioning = true;
        i.transitionAnim.SetTrigger(AnimHash_Transition);
    }

    public void TransitionMidPoint()
    {
        IsTransitioning = false;
    }

    public static void FadeIn()
    {
        i.transitionAnim.SetTrigger(AnimHash_FadeIn);
    }
    
    public static void FadeOut()
    {
        i.transitionAnim.SetTrigger(AnimHash_FadeOut);
    }
}
