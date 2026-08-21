using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text timerText;
    [SerializeField] private Text clearText;
    [SerializeField] private Button restartBt;
    [SerializeField] private Button quitBt;
    [SerializeField] private Text GameoverText;
    [SerializeField] private Text HPText;

    private float _Time = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        clearText.gameObject.SetActive(isRunning);
        restartBt.gameObject.SetActive(isRunning);
        quitBt.gameObject.SetActive(isRunning);
        GameoverText.gameObject.SetActive(isRunning);
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
        clearText?.gameObject.SetActive(!isRunning);
        restartBt?.gameObject.SetActive(!isRunning);
        quitBt?.gameObject.SetActive(!isRunning);
    }

    public void GameOver()
    {
        isRunning = false;
        Time.timeScale = 0f;
        GameoverText?.gameObject.SetActive(!isRunning);
        StartCoroutine(LoadScene());
    }

    public void Reset()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayScene");
    }

    private void UpdateTimerUI()
    {
        if ( timerText != null)
        {
            timerText.text = $"TIME： {_Time:F2} 秒";
        }
    }

    public void UpdateHPUI(int hp)
    {
        if ( HPText != null )
        {
            HPText.text = $"HP： {hp}";
        }
    }
}
