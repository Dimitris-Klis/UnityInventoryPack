using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 input;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera main;
    // Start is called before the first frame update
    void Start()
    {
        if (main == null) main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 dir = Input.mousePosition - main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void FixedUpdate()
    {
        rb.velocity = 100 * speed * Time.fixedDeltaTime * input.normalized;
    }
}
