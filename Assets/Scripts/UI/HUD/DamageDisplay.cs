using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{

    public float timeOnScreen = 1.5f;

    public float speed = 1.0f;
    public Vector3 direction = Vector3.up;

    // Update is called once per frame
    void Awake()
    {
        Invoke("DestroyMe", timeOnScreen);
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        transform.LookAt(Camera.main.transform);
        transform.Rotate( 0, -180, 0 );
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

}
