using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    float startX;
    public float endX;
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        if (transform.position.x > endX)
        {
            speed = -1 * Mathf.Abs(speed);
        }
        if (transform.position.x < startX)
        {

            speed = Mathf.Abs(speed);
        }
    }
}
