using System;
using TMPro;
using UnityEngine;

public class KartVisualController : MonoBehaviour
{
    [SerializeField] private Animator charAnimator;
    [SerializeField] private TextMeshProUGUI firstLetter;
    private static readonly int AnimHash_IsMoving = Animator.StringToHash("isMoving");

    private void Awake()
    {
        var playerKart = GetComponentInParent<PlayerKart>();
        if (playerKart != null)
        {
            SetFirstLetter(playerKart.photonPlayer.NickName.ToCharArray()[0].ToString());
        }
    }

    public void SetFirstLetter(string letter)
    {
        firstLetter.text = letter;
    }

    public void SetIsMoving(bool isMoving)
    {
        charAnimator.SetBool(AnimHash_IsMoving, isMoving);
    }
}
