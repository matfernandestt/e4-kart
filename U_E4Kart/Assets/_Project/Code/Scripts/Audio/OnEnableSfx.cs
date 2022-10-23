using UnityEngine;
using Random = UnityEngine.Random;

public class OnEnableSfx : MonoBehaviour
{
    [SerializeField] private bool randomOrder;
    [SerializeField] private AudioClip[] sfxOptions;

    private static int clipIndex = 0;

    private void OnEnable()
    {
        PlayAudio();
    }

    private void PlayAudio()
    {
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
