using UnityEngine;
using UnityEngine.UI;

public class ToggleSelectSfx : MonoBehaviour
{
    [SerializeField] private bool randomOrder;
    [SerializeField] private AudioClip[] sfxOptions;

    private Toggle selfToggle;
    private static int clipIndex = 0;

    private void Awake()
    {
        selfToggle = GetComponent<Toggle>();
        
        selfToggle.onValueChanged.AddListener(PlayAudio);
    }

    private void PlayAudio(bool value)
    {
        if (!value) return;
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        if (randomOrder)
        {
            src.clip = sfxOptions[Random.Range(0, sfxOptions.Length)];
        }
        else
        {
            if (clipIndex >= sfxOptions.Length)
                clipIndex = 0;
            src.clip = sfxOptions[clipIndex];
            clipIndex++;
        }
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
    }
}
