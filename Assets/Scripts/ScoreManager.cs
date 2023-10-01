using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float scoreMultiplier = 5.0f;

    public bool IsRecording { get; private set; }

    public void StartRecording() => IsRecording = true;
    public void StopRecording() => IsRecording = false;

    private readonly string scoreTextFormat = "Score: {0:.00}";
    private readonly Color yellowColor = new(249.0f / 255, 194.0f / 255, 43.0f / 255);
    private readonly Color orangeColor = new(251.0f / 255, 107.0f / 255, 29.0f / 255);
    private readonly Color redColor = new(234.0f / 255, 79.0f / 255, 54.0f / 255);
    private float score = 0.0f;
    private float innerScore100 = 0.0f;
    private float innerScore1000 = 0.0f;
    private float innerScore10000 = 0.0f;

    void Start() {
        StartRecording();
    }

    void Update()
    {
        if (IsRecording) {
            score += Time.deltaTime * scoreMultiplier;
            innerScore100 += Time.deltaTime * scoreMultiplier;
            innerScore1000 += Time.deltaTime * scoreMultiplier;
            innerScore10000 += Time.deltaTime * scoreMultiplier;
            scoreText.SetText(String.Format(scoreTextFormat, score));

            if (innerScore1000 > 10000) {
                innerScore10000 = 0.0f;
                innerScore1000 = 0.0f;
                innerScore100 = 0.0f;
                GetSequence(redColor, 1.6f, 0.3f);
            } else if (innerScore100 > 1000) {
                innerScore1000 = 0.0f;
                innerScore100 = 0.0f;
                 GetSequence(orangeColor, 1.4f, 0.3f);
            } else if (innerScore100 > 100) {
                innerScore100 = 0.0f;
                GetSequence(yellowColor, 1.2f, 0.3f);
            }
        }
    }

    Sequence GetSequence(Color color, float maxScale, float duration) {
        Sequence sequence = DOTween.Sequence();
            sequence.Append(scoreText.transform.DOScale(maxScale, duration));
            sequence.Append(scoreText.transform.DOScale(1, duration));
            sequence.Insert(0, DOTween.Sequence()
                .Append(scoreText.DOColor(color, duration))
                .Append(scoreText.DOColor(Color.white, duration)));
        return sequence;
    }
}
