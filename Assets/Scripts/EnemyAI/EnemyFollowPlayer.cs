using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] public float _speed;
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] public float _range;

    void Update()
    {
        FollowPlayer(_player);
    }

    public void FollowPlayer(Transform target)
    {
        float _distance = Vector2.Distance(transform.position, target.position);

        Vector2 direction = (target.position - transform.position).normalized;
        if (_distance > 1 && _distance < _range)
        {
            _rb2d.linearVelocity = new Vector2(direction.x * 2f, 0);
            _distance -= direction.x * 2f;
        }
        if (_distance < 1f)
        {
            _rb2d.linearVelocity = Vector2.zero;
        }
    }

    
}
