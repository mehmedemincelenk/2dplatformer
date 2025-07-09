using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("Kılıç Ayarları")]
    public bool _isAttacking;                 // Şu an saldırı animasyonu oynuyor mu?
    [SerializeField] BoxCollider2D _collider;

    [SerializeField] Animator anim;    // Hasar için kullanılan collider (isTrigger olmalı)

    void Awake()
    {
        _isAttacking = false;
        _collider.enabled = false;      
                     // Başta kapalı olsun
    }


    // 🟥 Kılıç saldırısı başladığında çağrılır
    public void SwordAttack()
    {
        anim.SetTrigger("attack");
        Hasar();
    }

    public void Hasar()
    {
        // Burada animasyon tetiklenebilir (eğer animator varsa)
        // _animator.SetTrigger("Attack");
        _collider.enabled = true;                    // Kılıç saldırısı başladığında collider aktif olur
        _isAttacking = true;                          // Bilgi olarak saldırı başladı
    }

    // 🟩 Kılıç saldırısı bittiğinde çağrılır (animasyon sonunda)
    public void CloseSwordCollider()
    {
        _collider.enabled = false;                   // Kılıç hasar alanı kapanır
        _isAttacking = false;                         // Saldırı bitti bilgisi
    }

    // 🟥 Kılıç hasarı: collider bir nesneye çarptığında tetiklenir
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isAttacking)  // Eğer çarpan "Player" ve gerçekten saldırı halindeysek
        {
            Debug.Log("Kılıçla vuruldu: " + collision.name);

            // Gerçek oyunda burada hasar fonksiyonu çağrılmalı
            // collision.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);

            collision.gameObject.GetComponent<PlayerHandler>().DeathPlayer(); // Şimdilik direkt yok etme
        }
    }
}
