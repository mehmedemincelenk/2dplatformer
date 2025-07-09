using UnityEngine;

public class Alan : MonoBehaviour
{
    [Header("Alan Hasarı Ayarları")]
    [SerializeField] public BoxCollider2D _collider;      // Oyuncuya çarpan görünmez alan
    [SerializeField] public LayerMask playerLayer;        // Sadece "Player" katmanını hedef alır
    [SerializeField] public Animator animator;            // Boss’un animasyon sistemi
    bool stopChar = false;
    bool playerVuruldu = false;                           // Oyuncu daha önce vuruldu mu?
    [SerializeField] Rigidbody2D rb;
    void Awake()
    {
        // Oyuna başlarken collider kapalı başlasın
        _collider.enabled = false;
    }

    void Start()
    {
        
    }
    private void Update()
    {
        if (stopChar) {
            rb.linearVelocity = Vector3.zero;
        
        }

    }

    // Oyuncu alana girerse
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Alanla vuruldu: " + collision.name);

            // Oyuncuyu yok et
            collision.GetComponent<PlayerHandler>().DeathPlayer();
        }
    }

    // 🔴 Bu fonksiyon animasyondan çağrılır (örneğin: boss yer vurma animasyonunda)
    public void AlanAttack()
    {
        AlanHasarCollider();
    }

    // ✅ Bu fonksiyon animasyonun tam vurduğu anda çağrılır (Event üzerinden)
    public void AlanHasarCollider()
    {
        
        stopChar = true;
        animator.SetTrigger("area");
        print("Alaaaaaaaaaaaaaaaaan");
    }
    public void openColliderArea()
    {
        _collider.enabled = true;  // Collider açılır ve hasar verme aktif olur
    }
    // ❌ Bu fonksiyon animasyon sonunda çağrılır (Event üzerinden)
    public void CloseAlanHasarCollider()
    {
        _collider.enabled = false; // Collider kapanır, tekrar zarar vermez
        stopChar = false;
        animator.SetTrigger("areaend");
    }
}
