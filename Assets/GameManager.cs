using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float startTime = 100;
    int coins = 0, score = 0;

    [SerializeField]
    Transform camTransform, playerTransform;
    float initDist;


    [SerializeField]
    private TextMeshProUGUI timeText, coinText, scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initDist = camTransform.position.x - playerTransform.position.x;   
    }

    // Update is called once per frame
    void Update()
    {
        camTransform.position = new Vector3(playerTransform.position.x + initDist, camTransform.position.y, camTransform.position.z);
        coinText.text = coins.ToString();
        scoreText.text = score.ToString();
        startTime -= Time.deltaTime;
        if (startTime > 0)
            timeText.text = ((int)startTime).ToString();
        else
        {
            Debug.Log("Game Over"); //for now
           Restart();
        }

    }

    public void AddPoints(int _score, int _coins) 
    {
        score += _score;
        coins += _coins;
    }

    public void Restart() 
    {
        SceneManager.LoadScene(0);
    }
}
