using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] public Vector2 vel;
    public bool isParried = false;
    [SerializeField] GameObject dieEffect;
    [SerializeField] GameObject child;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = vel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isParried)
        {
            rb.linearVelocity = vel;

        }
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        child.transform.rotation = Quaternion.Euler(0f, 0f, angle + 120);
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHandler>().TakeHit();
            
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(dieEffect , collision.gameObject.transform.position , Quaternion.identity);
            Destroy(collision.gameObject);
        }
        Destroy(this.gameObject);
    }
}
