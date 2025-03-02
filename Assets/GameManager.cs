using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void IncrementScore(){
        score++;
        scoreText.text = $"Score: {score}";
    }
}
