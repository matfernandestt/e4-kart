using System;
using UnityEngine;

public class SprayBase : MonoBehaviour
{
    private AudioSource src;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlaySfx()
    {
        if (src == null) return;
        src.Play();
    }
}
