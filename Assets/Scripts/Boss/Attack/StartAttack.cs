using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAttack : MonoBehaviour
{
    [Header("Saldırı Bileşenleri")]
    [SerializeField] Dash _dashScript;
    [SerializeField] Sword _swordScript;
    [SerializeField] Alan _alanScript;
    [SerializeField] List<Action> _fonksiyonlar = new List<Action>();

    [Header("Hareket")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 2f;
    [SerializeField] float range = 10f;

    [Header("Referanslar")]
    [SerializeField] Animator anim;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform bossTransform;
    [SerializeField] GameObject alert;
    [SerializeField] GameObject sceneto;

    public int can = 10;
    bool died = false;

    void Awake()
    {
        _fonksiyonlar.Add(_dashScript.DashAttack);
        _fonksiyonlar.Add(_swordScript.SwordAttack);
        _fonksiyonlar.Add(_alanScript.AlanAttack);
    }

    void Start()
    {
        StartCoroutine(RandomAttackRoutine());
    }

    void Update()
    {
        FollowPlayer();
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        anim.SetBool("isMoving", isMoving);
        if(can<= 0 && died == false)
        {
            anim.SetTrigger("die");
            died = true;
        }
        if (rb.linearVelocity.x < -0.1f)
        {
            // Sola gidiyor → X scale negatif
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (rb.linearVelocity.x > 0.1f)
        {
            // Sağa gidiyor → X scale pozitif
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }
    public void DestroyMe()
    {
        sceneto.GetComponent<SceneTo>().ChangeScene();
        Destroy(this.gameObject);

    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > 1f && distance < range)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * speed, 0f);
        }
        else if (distance <= 1f)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    IEnumerator RandomAttackRoutine()
    {
        while (true) // Buraya "player is alive" kontrolü eklenecek
        {
            float mesafe = Vector3.Distance(bossTransform.position, playerTransform.position);

            if (mesafe < 100f)
            {
                // 1️⃣ ALERT AÇ
                alert.SetActive(true);
                yield return new WaitForSeconds(0.3f); // 1 saniye bekle

                // 2️⃣ ALERT KAPAT
                alert.SetActive(false);

                // 3️⃣ SALDIR
                int rastgeleSkill = UnityEngine.Random.Range(0, _fonksiyonlar.Count);
                _fonksiyonlar[rastgeleSkill]();
            }

            float beklemeSuresi = UnityEngine.Random.Range(2f, 5f);
            yield return new WaitForSeconds(beklemeSuresi);
        }
    }


    // Animasyon event'leri
    public void EndofSwordAttack() => _swordScript.CloseSwordCollider();
    public void endofarea() => _alanScript.CloseAlanHasarCollider();
    public void openArea() => _alanScript.openColliderArea();
}
