using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] private AudioSource source1;

    private static BgmManager i;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
            
            GlobalSettingsData.Instance.Set_BGM_Volume(GlobalSettingsData.Instance.Get_BGM_Volume());
            GlobalSettingsData.Instance.Set_SFX_Volume(GlobalSettingsData.Instance.Get_SFX_Volume());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlayBGM(AudioClip clip)
    {
        i.source1.clip = clip;
        i.source1.Play();
    }
}
