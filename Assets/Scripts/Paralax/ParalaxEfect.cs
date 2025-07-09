using UnityEngine;

public class ParalaxEfect : MonoBehaviour
{
    GameObject player;
    [SerializeField] float paralaxX;
    [SerializeField] float paralaxY;
    Vector3 startPosition;

    void Start()
    {
        player = FindAnyObjectByType<PlayerHandler>().gameObject;
        startPosition = transform.position; // Arka planın orijinal pozisyonu
    }

    void Update()
    {
        if (player != null)
        {
            float moveX = player.transform.position.x * paralaxX;
            float moveY = player.transform.position.y * paralaxY;

            transform.position = new Vector3(startPosition.x + moveX, startPosition.y + moveY, transform.position.z);
        }
    }

}
