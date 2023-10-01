using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    [Header("Score")]
    public TMP_Text scoreText;
    public float scoreMultiplier = 5.0f;

    [Header("Game Over")]
    public TMP_Text gameOverText;
    public List<string> gameOverInsults;

    public bool IsRecording { get; private set; }

    public void StartRecording() {
        IsRecording = true;
        scoreText.DOFade(1f, 1f);
    }
    public void StopRecording() {
        IsRecording = false;
        scoreText.DOFade(0f, 1f);
        score = 0.0f;
        innerScore100 = 0.0f;
        innerScore1000 = 0.0f;
        innerScore10000 = 0.0f;
    }

    private readonly string scoreTextFormat = "Score: {0:.00}";
    private readonly Color yellowColor = new(249.0f / 255, 194.0f / 255, 43.0f / 255);
    private readonly Color orangeColor = new(251.0f / 255, 107.0f / 255, 29.0f / 255);
    private readonly Color redColor = new(234.0f / 255, 79.0f / 255, 54.0f / 255);
    private float score = 0.0f;
    private float innerScore100 = 0.0f;
    private float innerScore1000 = 0.0f;
    private float innerScore10000 = 0.0f;

    void Start() {
        gameOverText.gameObject.SetActive(false);
        IsRecording = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            StartRecording();
        }
        if (IsRecording) {
            score += Time.deltaTime * scoreMultiplier;
            innerScore100 += Time.deltaTime * scoreMultiplier;
            innerScore1000 += Time.deltaTime * scoreMultiplier;
            innerScore10000 += Time.deltaTime * scoreMultiplier;
            scoreText.SetText(String.Format(scoreTextFormat, score));

            if (innerScore10000 > 10000) {
                innerScore10000 = 0.0f;
                innerScore1000 = 0.0f;
                innerScore100 = 0.0f;
                GetSequence(redColor, 1.6f, 0.3f);
            } else if (innerScore1000 > 1000) {
                innerScore1000 = 0.0f;
                innerScore100 = 0.0f;
                 GetSequence(orangeColor, 1.4f, 0.3f);
            } else if (innerScore100 > 100) {
                innerScore100 = 0.0f;
                GetSequence(yellowColor, 1.2f, 0.3f);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            StopRecording();
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Y)) { ResetGame(); }
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

    string GetRandomInsult() {
        if (gameOverInsults.Count > 0) {
            return gameOverInsults[UnityEngine.Random.Range(0, gameOverInsults.Count)];
        }
        return "Game Over";
    }

    void GameOver() {
        gameOverText.gameObject.SetActive(true);
        gameOverText.SetText(GetRandomInsult());
        gameOverText.DOFade(1f, 1f);
    }

    void ResetGame() {
        gameOverText.gameObject.SetActive(false);
        gameOverText.alpha = 0.0f;
    }
}
