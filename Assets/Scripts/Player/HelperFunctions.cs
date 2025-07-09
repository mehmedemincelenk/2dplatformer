using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    private Transform _transformPlayer;

    void Awake()
    {
        _transformPlayer = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GetTransform();
    }

    public Transform GetTransform()
    {
        return _transformPlayer;
    }
}
