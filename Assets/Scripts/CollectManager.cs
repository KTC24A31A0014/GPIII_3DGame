using UnityEngine;
using UnityEngine.UI;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;

    [SerializeField] private Text countText;
    [SerializeField] private int countMax;

    private int currentCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if ( currentCount == countMax)
        {
            GameManager.Instance.Clear();
        }
    }

    public void AddItem()
    {
        currentCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        countText.text = $"{currentCount} / {countMax}";
    }
}
