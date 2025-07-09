using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float y;
    [SerializeField] Camera main;
    GameObject Player;
    void Start()
    {
        Player = FindAnyObjectByType<PlayerHandler>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(Mathf.Clamp(Player.transform.position.x, minX, maxX) , y,-10);
        main.gameObject.transform.position = pos;
    }
}
