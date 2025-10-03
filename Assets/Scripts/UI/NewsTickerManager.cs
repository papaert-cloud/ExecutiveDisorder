using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsTickerManager : MonoBehaviour
{
    [SerializeField] private List<NewsTicker> m_Tickers = new List<NewsTicker>();

    private void OnEnable()
    {
        if (m_Tickers.Count == 0) return;

        float baseDuration = m_Tickers[0].scrollDuration;
        float delayStep = baseDuration / m_Tickers.Count;

        for (int i = 0; i < m_Tickers.Count; i++)
        {
            m_Tickers[i].scrollDuration = baseDuration;
            StartCoroutine(StartTickerWithDelay(m_Tickers[i], i * delayStep));
        }
    }

    private IEnumerator StartTickerWithDelay(NewsTicker ticker, float delay)
    {
        yield return new WaitForSeconds(delay);
        ticker.gameObject.SetActive(true);
        ticker.StartScrolling();
    }

    private void OnDisable()
    {
        foreach (var ticker in m_Tickers)
        {
            ticker.StopScrolling();
            ticker.gameObject.SetActive(false);
        }
    }
}
