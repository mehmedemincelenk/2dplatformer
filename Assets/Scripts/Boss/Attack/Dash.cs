using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] GameObject _player;                         // Oyuncunun referansı
    [SerializeField] Rigidbody2D _rb;           // Boss'un fizik bileşeni
    [SerializeField] Animator _animator;        // Boss'un animasyon sistemi
    [SerializeField] BoxCollider2D _dashCollider; // Dash sırasında aktif olan hasar alanı (isTrigger)

    [Header("Dash Ayarları")]
    [SerializeField] float _dashSpeed;       // Dash hızı
    [SerializeField] float _dashDuration;   // Dash süresi (zaman tabanlı değil, mesafeye göre hareket ediyoruz)
    [SerializeField] float _dashMesafe;       // Dash mesafesi
    [SerializeField] LayerMask _enemyLayer;        // Sadece player'a çarpsın, diğer her şeyden geçsin

    [Header("Bilgilendirme")]
    public bool _isDashing = false;                // Şu an dash yapılıyor mu?
    float _distance;                               // Oyuncu ile boss arasındaki anlık mesafe

    void Awake()
    {
        _dashCollider.enabled = false;                               // Başta kapalı
    }

    void Update()
    {
        // Boss ile oyuncu arasındaki mesafeyi sürekli ölç
        _distance = Vector2.Distance(_player.transform.position, transform.position);
    }


    // 🟥 Dash saldırısını başlat
    public void DashAttack()
    {
        // Yön: oyuncuya doğru normalize edilmiş vektör (sabit uzunluk)
        Vector2 rawDir = (_player.transform.position - transform.position);
        Vector2 direction = new Vector2(Mathf.Sign(rawDir.x), 0f); // sadece X yönünde dash


        if (!_isDashing)
            StartCoroutine(DashCoroutine(direction));
    }

    // 🟩 Dash süreci: ileri doğru kayarak gitme işlemi
    IEnumerator DashCoroutine(Vector2 direction)
    {

        _isDashing = true;
        _animator.SetTrigger("dash");
        _rb.gravityScale = 0;                    // Yerçekimini geçici kapat (daha düzgün hareket için)
        _dashCollider.enabled = true;            // Hasar alanını aktif et

        Vector2 startPos = _rb.position;                                 // Başlangıç noktası
        Vector2 targetPos = startPos + direction * _dashMesafe;         // Gideceği yer

       float timer = 0f;
float maxDashTime = 1.5f;

while (Vector2.Distance(_rb.position, targetPos) > 0.1f && timer < maxDashTime)
{
    timer += Time.fixedDeltaTime;
    Vector2 newPos = Vector2.MoveTowards(_rb.position, targetPos, _dashSpeed * Time.fixedDeltaTime);
    _rb.MovePosition(newPos);
    yield return new WaitForFixedUpdate();
}

        _rb.gravityScale = 1;              // Yerçekimini geri aç
        _dashCollider.enabled = false;     // Hasar alanını kapat
        print("Dash bittiiiiiiiiiiiiiiiiiii");
        _isDashing = false;
                _animator.SetTrigger("dashend");

    }

    // 🟥 Dash sırasında çarpışma olursa
    void OnTriggerEnter2D(Collider2D collider)
    {
        // 6: Player Layer
        if (collider.gameObject.layer == 6 && _isDashing)
        {
            Debug.Log("Dash ile hasar verildi: " + collider.gameObject.name);
            collider.gameObject.GetComponent<PlayerHandler>().DeathPlayer();  // Oyuncuyu yok et (gerçek oyunda burada can azaltılır)
        }
    }
}
