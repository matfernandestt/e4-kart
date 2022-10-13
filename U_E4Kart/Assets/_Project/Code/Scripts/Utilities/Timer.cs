using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerLabel;
    
    private Coroutine countdownRoutine;
    private WaitForSeconds oneSecond = new WaitForSeconds(1f);

    public void Countdown(int time, Action onFinishCountingDown = null)
    {
        var currentTime = time;

        if(countdownRoutine != null)
            StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(Counting());
        IEnumerator Counting()
        {
            while (currentTime >= 0)
            {
                timerLabel.text = currentTime.ToString();
                yield return new WaitForSeconds(1f);
                currentTime--;
            }
            onFinishCountingDown?.Invoke();
        }
    }
}
