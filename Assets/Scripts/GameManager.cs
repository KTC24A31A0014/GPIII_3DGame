using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text timerText;
    [SerializeField] private Text clearText;
    [SerializeField] private Text HPText;

    private float _Time = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        clearText.gameObject.SetActive(isRunning);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            _Time += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        _Time = 0f;
        isRunning = true;
    }

    public void Clear()
    {
        isRunning = false;
        Time.timeScale = 0f;
        clearText.gameObject.SetActive(!isRunning);
    }

    public void GameOver()
    {
        isRunning = false;
        SceneManager.LoadScene("PlayScene");
    }

    private void UpdateTimerUI()
    {
        if ( timerText != null)
        {
            timerText.text = $"TIMEÅF {_Time:F2} ïb";
        }
    }

    public void UpdateHPUI(int hp)
    {
        if ( HPText != null )
        {
            HPText.text = $"HPÅF {hp}";
        }
    }
}
