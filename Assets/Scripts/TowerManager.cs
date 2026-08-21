using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("タワー")]
    [SerializeField] private List<Tower> Towers = new List<Tower>();

    [Header("有効化される数")]
    [SerializeField] private int activeCount = 3;

    private List<Tower> currentTowers = new List<Tower>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupRandomTowers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupRandomTowers()
    {
        // リストをコピーしてシャッフル
        List<Tower> shuffled = new List<Tower>(Towers);
        Shuffle(shuffled);

        // いったんすべて無効化
        foreach (var obj in Towers)
        {
            if (obj != null) obj.gameObject.SetActive(false);
        }

        currentTowers.Clear();

        // シャッフル後の先頭から指定の数だけ有効化
        int count = Mathf.Min(activeCount, shuffled.Count);
        for (int i = 0; i < count; i++)
        {
            if (shuffled[i] != null)
            {
                shuffled[i].gameObject.SetActive(true);
                currentTowers.Add(shuffled[i]);
            }
        }
    }

    private void Shuffle(List<Tower> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
