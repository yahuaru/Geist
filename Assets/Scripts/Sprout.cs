using UnityEngine;
using System.Collections;

public class Sprout : MonoBehaviour
{

    private bool _floating = true;
    public float FloatingSpeed = 0.005f;
    private Vector3 _newPosition;
    public float LifeTime = 10.0f;
    private float _lifeTimer;
    private Vector3 _initialPos;
    // Use this for initialization
    void Start()
    {
        _lifeTimer = LifeTime;
        _initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer < 0)
        {
            _lifeTimer = LifeTime;
            transform.position = _initialPos;
        }

        if (_floating)
        {
            _newPosition = transform.position;
            _newPosition.y += FloatingSpeed;
            transform.position = _newPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D objCollider2D)
    {
        if (objCollider2D.tag == "Player")
        {
            _floating = true;
        }
    }
}
