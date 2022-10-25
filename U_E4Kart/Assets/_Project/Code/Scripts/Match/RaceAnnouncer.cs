using UnityEngine;

public class RaceAnnouncer : MonoBehaviour
{
    [SerializeField] private AudioClip beep;
    [SerializeField] private AudioClip startRace;
    
    public void Beep3()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = beep;
        src.pitch = .9f;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
    }
    
    public void Beep2()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = beep;
        src.pitch = 1f;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
    }
    
    public void Beep1()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = beep;
        src.pitch = 1.1f;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
    }
    
    public void CountdownEnd()
    {
        var src = Pool_SFX.Instance.GetInstance(Vector3.zero);
        src.clip = startRace;
        src.Play();
        Pool_SFX.Instance.ReturnInstanceWhenConcludePlaying(src);
        
        
        RaceController.onStartRace?.Invoke();
    }
}
