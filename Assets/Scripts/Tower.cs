using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("塔の")]
    [SerializeField] int towerHP = 5;
    [SerializeField] float intervalMax = 1f;

    private Renderer rend;
    private Material normalMate;

    public Material HitMate;

    float interval = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 pos = transform.position;

        rend = GetComponent<Renderer>();
        normalMate = rend.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -6)
        {
            // 子オブジェクトを切り離す
            transform.DetachChildren();

            Destroy(gameObject);
        }

        if (interval > 0f)
        {
            interval -= Time.deltaTime;
        }
        else if (interval <= 0f)
        {
            rend.material = normalMate;
        }
    }

    // 被弾処理
    private void OnCollisionStay(Collision collision)
    {
        var attackObj = collision.gameObject.GetComponent<AttackObject>();
        if ( attackObj != null && interval <= 0 && !attackObj.CompareTag("Enemy"))
        {
            towerHP -= attackObj.power;
            interval = intervalMax;
            TowerDown(towerHP);
            rend.material = HitMate;

            if ( interval <= 0 )
            {
                Destroy(gameObject);
            }
        }
    }

    private void TowerDown(int _towerHP)
    {
        Vector3 pos = transform.position;
        pos.y = _towerHP;
        transform.position = pos;
    }
}
