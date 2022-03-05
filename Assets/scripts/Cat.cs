using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private const float JUMP_AMOUNT = 20f;
    private Rigidbody2D CatRigidbody2D;
    public static Cat instance;

    private void Awake() {
        CatRigidbody2D = GetComponent<Rigidbody2D>();
        instance = this;
        Level.instance.GameOverEvent += OnGameOver;

    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Jump();
        }
    }

    private void OnGameOver() {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void Jump() {
        CatRigidbody2D.velocity = new Vector2(0, 1) * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Level.OnGameOver();
    }
}
