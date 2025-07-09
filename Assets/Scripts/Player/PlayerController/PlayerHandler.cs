    using System.Collections;
    using UnityEngine;
using UnityEngine.SceneManagement;

    public class PlayerHandler : MonoBehaviour
    {

        [Header("Movement")]
        [SerializeField] float playerSpeed;
        [SerializeField] SpriteRenderer spriteRenderer;
        [Header("Jump")]
        [SerializeField] Transform jumpController;
        [SerializeField] bool canJump;
        [SerializeField] float jumpPower;
        [SerializeField] bool hasDoubleJump;
        [SerializeField] bool canDoubleJump;
        [SerializeField] float fallMultiplier = 2.5f;
        [SerializeField] float lowJumpMultiplier = 2f;
        [SerializeField] bool _isGrounded = true;

        [Header("Animator")]
        bool _isMoving = false;
        Animator animator;
        [Header("Attack")]
        [SerializeField] Transform attackPoint;
        [SerializeField] float attackRange;
        [Header("Dash")]
        [SerializeField] TrailRenderer trail;
        [SerializeField] float dashPower = 10f;
        [SerializeField] float dashDuration = 0.2f;
        [SerializeField] float dashCooldown = 1f;
        [SerializeField] GameObject dashSlashEffect;
        [SerializeField] float slashRadius = 1.5f;
        private Collider2D _colliderPlayer;

        bool isDashing = false;
        float dashCooldownTimer = 0f;
        [SerializeField] GameObject dieEffect;

    [Header("Parry")]
        [SerializeField] Transform parryPoint;
        [SerializeField] float parryTime;
        [SerializeField] public bool isParrying;
        [SerializeField] float parryCooldown = 1f;
        [SerializeField] float parryRadius = 1f;
        [Header("Raycast")]
        [SerializeField] public float _range;
        [Header("Ses")]
        [SerializeField] public AudioSource _audioSource; 
        [SerializeField] public AudioClip _attackClip;
        [SerializeField] public AudioClip _deathClip;
        [SerializeField] public AudioClip _parryClip;

        // Dropdown sistemi
        private bool _onPlatform = false;
        public int _platformLayer = 3; 

        bool canParry = true;

        Rigidbody2D rb;
        void Start()
        {
            _colliderPlayer = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            trail = GetComponent<TrailRenderer>();
            trail.enabled = false;
        }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            MovementHandler();
            JumpControlHandler();
            
            AttackHandler();
            ParrySkill();
        }
        JumpHandler();
        BetterJumpHandler();
        DashSkill(); // burada çağır
        Slash();
        AnimationHandler();

        // Assagi in
        if (Input.GetKeyDown(KeyCode.S) && _onPlatform)
        {
            StartCoroutine(DropDown());
        }

    }
        void MovementHandler()
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(horizontalAxis * playerSpeed, rb.linearVelocityY);
            if (horizontalAxis != 0)
            {
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
            if (horizontalAxis > 0)
            {
                spriteRenderer.flipX = false;
            }
            if (horizontalAxis < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        void JumpControlHandler()
        {
            float rayLength = 0.1f;
            RaycastHit2D[] hits = Physics2D.RaycastAll(jumpController.position, Vector2.down, rayLength);

            Debug.DrawRay(jumpController.position, Vector2.down * rayLength, Color.red);

            bool grounded = false;
            _isGrounded = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    _isGrounded = true;
                    grounded = true;
                    break;
                }
            }

            // Sadece aşağı düşerken veya dikey hız çok düşükken "yerde" say
            if (grounded && rb.linearVelocity.y <= 0.01f)
            {
                if (!canJump) // sadece havadayken yere inince
                {
                    canJump = true;
                    if (canDoubleJump)
                        hasDoubleJump = true;
                }
            }
            else
            {
                canJump = false;
            }
        }
        void AttackHandler()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _audioSource.PlayOneShot(_attackClip);

                animator.SetTrigger("attack");
            }
        }
    public void DeathPlayer()
            {
                // Ölüm sesini çal
                _audioSource.PlayOneShot(_deathClip);

                // Sesin bitmesini bekleyip sonra objeyi yok et
                Destroy(gameObject, _deathClip.length);
                PlayerPrefs.SetString("GeldigiSahne", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("Siyah_Ekran");


    }
        void DashSkill()
        {
            dashCooldownTimer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 ) // Yere basarken dash için
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                Vector2 dashDirection = new Vector2(horizontal, vertical).normalized;

                if (dashDirection == Vector2.zero)
                {
                    // Eğer yön verilmediyse bakılan yöne dash
                    dashDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
                }

                StartCoroutine(PerformDash(dashDirection));
            }
        }
        void ParrySkill()
        {
            if (Input.GetMouseButtonDown(1) && canParry)
            {
                StartCoroutine(PerformParry());
            }
        }
        public void AttackEnemy()
        {
            Vector2 yon = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D[] _isHit = Physics2D.RaycastAll(transform.position, yon, _range);
            Debug.DrawRay(transform.position, yon * _range, Color.red, 0.5f);
            foreach (var i in _isHit)
            {
                Debug.Log("Çarpılan: " + i.collider.gameObject.name + " / Tag: " + i.collider.tag);
                if (i.collider.CompareTag("Enemy"))
                {
                    Destroy(i.collider.gameObject);
                    Instantiate(dieEffect, i.collider.gameObject.transform.position, Quaternion.identity);

            }
            if (i.collider.CompareTag("Boss"))
            {
                i.collider.gameObject.GetComponent<StartAttack>().can -= 1;
            }
        }
        }

        void JumpHandler()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canJump)
                {
                    animator.SetTrigger("jump");
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    canJump = false; // normal zıplama hakkı gitti
                }
                else if (hasDoubleJump)
                {
                    animator.SetTrigger("jump");
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // double jump öncesi yukarı ivmeyi resetle
                    rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    hasDoubleJump = false; // double jump hakkı bitti
                }
            }
        }
        void BetterJumpHandler()
        {
            // Eğer oyuncu düşüyorsa (y-ekseni negatif), daha hızlı düşür
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            // Eğer oyuncu zıplıyor ama Space bırakılmışsa, düşük zıplama uygula
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        void AnimationHandler()
        {
            animator.SetBool("isMoving", _isMoving);
            animator.SetFloat("yVel", rb.linearVelocityY);
            animator.SetBool("grounded", _isGrounded);

        }
        void Slash()
        {
            if (!isDashing) return;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slashRadius);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // Düşmanı yok et
                    Destroy(hit.gameObject);

                    // Slash efekti oluştur
                    if (dashSlashEffect != null)
                    {
                        Instantiate(dashSlashEffect, hit.transform.position, Quaternion.identity);
                    }

                    Debug.Log("Enemy slashed!");
                }
            }
        }
        public void TakeHit()
        {
            animator.SetTrigger("takehit");
            print("hit taken");
        DeathPlayer();
        }
        System.Collections.IEnumerator PerformDash(Vector2 direction)
        {
            trail.enabled = true;
            isDashing = true;
            dashCooldownTimer = dashCooldown;

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f; // havada kaymasın diye yerçekimini kapat

            rb.linearVelocity = direction * dashPower;
            animator.SetTrigger("dash");

            yield return new WaitForSeconds(dashDuration);
            trail.enabled = false;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = originalGravity;
            isDashing = false;
        }
        System.Collections.IEnumerator PerformParry()
        {
            _audioSource.PlayOneShot(_parryClip);
            canParry = false;
            isParrying = true;
            animator.SetTrigger("parry");

            float timer = 0f;

            while (timer < parryTime)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(parryPoint.position, parryRadius);

                if (hits.Length > 0)
                {
                    Debug.Log("Parry Success! Projectiles detected:");

                    foreach (var proj in hits)
                    {
                        if (proj.gameObject.CompareTag("Projectile"))
                        {
                            if (proj.gameObject.GetComponent<ProjectileScript>().isParried == false)
                            {
                                proj.gameObject.GetComponent<Rigidbody2D>().linearVelocity = -1 * proj.gameObject.GetComponent<Rigidbody2D>().linearVelocity;
                                proj.gameObject.GetComponent<ProjectileScript>().isParried = true;
                            }
                        }
                    }

                    // İstersen burada projectile'ı yok edebilirsin:
                    // foreach (var proj in hits) Destroy(proj.gameObject);
                }


                timer += Time.deltaTime;
                yield return null;
            }

            isParrying = false;

            yield return new WaitForSeconds(parryCooldown);
            canParry = true;
        }

        void OnDrawGizmosSelected()
        {
            if (parryPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(parryPoint.position, parryRadius);
            }
        }

        private IEnumerator DropDown()
        {
            _colliderPlayer.enabled = false;  // Collider kapandı, platformun altına geçilebilir
            yield return new WaitForSeconds(0.5f); // Yarım saniye bekle
            _colliderPlayer.enabled = true;   // Collider tekrar açıldı
        }


        void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.layer == _platformLayer) {
                _onPlatform = true;
            }
        }

        void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.layer == _platformLayer) {
                _onPlatform = false;
            }
        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "portal")
        {
            collision.gameObject.GetComponent<SceneTo>().ChangeScene();
        }
    }





}
