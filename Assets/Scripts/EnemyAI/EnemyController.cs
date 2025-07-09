using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Raycast Ayarları")]
    [SerializeField] float _mesafe = 0.5f;
    [SerializeField] float _health;
    [SerializeField] Transform _frontRaycast;
    [SerializeField] int _yon = 1;
    [SerializeField] bool _onGroundTag;
    [SerializeField] Transform hitPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackDuration = 1f;

    [Header("Temel")]
    [SerializeField] float _hiz;
    [SerializeField] bool takipModu = false;
    [SerializeField] float takipHızı = 2f;
    GameObject player;

    [Header("References")]
    [SerializeField] Rigidbody2D _rb2d;
    [SerializeField] bool isPlatformGoblin = true;
    [SerializeField] float directionChangeInterval = 3f;
    float directionChangeTimer;

    Animator animator;
    [SerializeField] bool isAttacking = false;
    [SerializeField] float attackDelay = 1f;
    [Header("Ses")]
        [SerializeField] public AudioSource _audioSourceAttack; 
        [SerializeField] public AudioSource _audioSourceLive; 
        [SerializeField] public AudioClip _deathClip;
        [SerializeField] public AudioClip _liveClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        directionChangeTimer = directionChangeInterval;
        player = FindAnyObjectByType<PlayerHandler>().gameObject;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            if (takipModu)
                TakipEt();
            else
                movement();

            animator.SetBool("isMoving", true);
        }
        else
        {
            _rb2d.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }

        attackPlayer(); // Mutlaka çalışmalı
    }

    void TakipEt()
    {
        if (player == null) return;

        float fark = player.transform.position.x - transform.position.x;
        _yon = fark > 0 ? 1 : -1;

        _rb2d.linearVelocity = new Vector2(_yon * takipHızı, _rb2d.linearVelocity.y);

        // Sprite yönünü çevir
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -_yon;
        transform.localScale = scale;
    }

    void movement()
    {
        _rb2d.linearVelocity = new Vector2(_yon * _hiz, _rb2d.linearVelocity.y);

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -_yon;
        transform.localScale = scale;

        if (isPlatformGoblin)
        {
            RaycastHit2D[] frontHits = Physics2D.RaycastAll(_frontRaycast.position, Vector2.down, _mesafe);

            bool frontHasGround = false;
            foreach (RaycastHit2D hit in frontHits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    frontHasGround = true;
                    break;
                }
            }

            if (!frontHasGround)
            {
                _yon *= -1;
                Debug.Log("Önünde zemin yok, yön değiştirildi!");
            }

            Debug.DrawLine(
                _frontRaycast.position,
                _frontRaycast.position + Vector3.down * _mesafe,
                frontHasGround ? Color.green : Color.red
            );
        }
        else
        {
            // Platform goblini değilse süreyle yön değiştir
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0f)
            {
                _yon *= -1;
                directionChangeTimer = directionChangeInterval;
                Debug.Log("Platform goblini değil: yön değiştirildi");
            }
        }
    }

    void attackPlayer()
    {
        Vector2 direction = new Vector2(_yon, 0);
        RaycastHit2D hit = Physics2D.Raycast(hitPoint.position, direction, attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (!takipModu)
            {
                takipModu = true;
                Debug.Log("Takip moduna geçildi!");
            }

            if (!isAttacking)
            {
                isAttacking = true;
                _rb2d.linearVelocity = Vector2.zero;
                animator.SetBool("isMoving", false);
                StartCoroutine(DelayedAttack());
            }

            Debug.DrawLine(hitPoint.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(hitPoint.position, hitPoint.position + (Vector3)direction * attackRange, Color.yellow);
        }

        Debug.DrawRay(hitPoint.position, direction * attackRange, Color.cyan);
    }

    IEnumerator DelayedAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("attack");
        Debug.Log("Saldırı başladı!");

        yield return new WaitForSeconds(attackDuration);
        EndAttack();
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    public void EnemyAttackHit()
    {
        Vector2 direction = new Vector2(_yon, 0);
        RaycastHit2D hit = Physics2D.Raycast(hitPoint.position, direction, attackRange);

        Debug.DrawRay(hitPoint.position, direction * attackRange, Color.magenta, 0.5f);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Enemy'nin saldırısı Player'a isabet etti!");
            hit.collider.gameObject.GetComponent<PlayerHandler>().TakeHit();
        }
        else
        {
            Debug.Log("Enemy saldırdı ama Player'a isabet etmedi.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _onGroundTag = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _onGroundTag = false;
        }
    }
}
