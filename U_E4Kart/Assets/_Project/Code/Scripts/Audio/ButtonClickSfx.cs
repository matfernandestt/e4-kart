using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSfx : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;
    
    private Button selfButton;

    private void Awake()
    {
        selfButton = GetComponent<Button>();
        
        selfButton.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = sfx;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
    }
}
