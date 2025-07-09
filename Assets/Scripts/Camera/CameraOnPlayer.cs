using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    [SerializeField] public Transform _target;            // Takip edilecek karakter
    [SerializeField] public float _smoothSpeed = 0.125f; // Yumuşaklık katsayısı (0-1 arası)
    [SerializeField] public Vector3 _offset;              // Kamera ile hedef arasındaki mesafe

    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
