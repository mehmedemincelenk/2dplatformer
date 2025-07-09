using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedEnemyHandler : MonoBehaviour
{
    GameObject player;
    [SerializeField]Transform turner;
    [SerializeField]Transform shotPoint;
    [SerializeField] float range;
    [SerializeField] float distance;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float shotInterval;
    [SerializeField] bool canShot = true;
    [SerializeField] float projectileSpeed;
    [SerializeField] float movementRange;
    [SerializeField] float moveSpeed;
    [Header("Ses")]
        [SerializeField] public AudioSource _audioSourceAttack; 
        [SerializeField] public AudioSource _audioSourceLive; 
        [SerializeField] public AudioClip _attackClip;
        [SerializeField] public AudioClip _liveClip;


    Rigidbody2D rb;
    void Start()
    {
        player = FindAnyObjectByType<PlayerHandler>().gameObject;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        HeadTurn();
        dedectDistance();
        ShotProjectile();
        Movement();
    }

    void HeadTurn()
    {
        if (player != null && turner != null)
        {
            Vector2 direction = (player.transform.position - turner.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Eğer turner yukarı bakıyorsa, 90 derece kaydır
            turner.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    void dedectDistance()
    {
        distance = Vector2.Distance(player.transform.position, this.gameObject.transform.position);
    }

    void ShotProjectile()
    {
        if(canShot && distance < range)
        {
            _audioSourceAttack.PlayOneShot(_attackClip);
            GameObject instantiatedShot = Instantiate(projectilePrefab, shotPoint.position, Quaternion.identity);
            instantiatedShot.GetComponent<ProjectileScript>().vel = shotPoint.up * projectileSpeed;
            StartCoroutine(nameof(shotInt));
        }
    }
    void Movement()
    {
        if (distance < movementRange && distance > range - 1)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // Aşağıya raycast atarak ground mesafesini kontrol et
            RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 4f);
            Debug.DrawRay(transform.position, Vector2.down * 4f, Color.green);

            if (groundHit.collider != null && groundHit.collider.CompareTag("Ground"))
            {
                // Eğer yere çok yakınsa, aşağı gitme (y bileşenini zorla yukarıya)
                direction.y = Mathf.Max(direction.y, 0.2f); // yukarı çıkmayı garanti et
            }

            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // menzil dışında hareketi durdur
        }
    }

    IEnumerator shotInt()
    {
        canShot = false;
        yield return new WaitForSeconds(shotInterval);  
        canShot = true;
    }
}
