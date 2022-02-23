using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator anim;
    private bool faceLeft;
    private bool faceTop;
    public Transform leftPoint, rightPoint;
    public Transform topPoint, bottomPoint;
    public LayerMask ground;
   

    public float speed = 1.5f;
    public float jumpForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        faceLeft = Random.value < 0.5;
        faceTop = Random.value < 0.5;

        if (gameObject.tag == "EnemyFrog")
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (faceLeft ? 1 : -1), transform.localScale.y, transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementEagle();
        SwitchAnim();
    }
    void Movement()
    {
        if (gameObject.tag == "EnemyFrog")
        {
            if (transform.position.x - 1.5 < leftPoint.position.x)
            {
                faceLeft = false;

                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (transform.position.x + 1.5 > rightPoint.position.x)
            {
                faceLeft = true;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (faceLeft)
            {
                
                
                if (collider.IsTouchingLayers(ground))
                {
                    anim.SetBool("jumping", true);
                    rb.velocity = new Vector2(-speed, jumpForce);
                }
            }
            else
            {
                if (collider.IsTouchingLayers(ground))
                {
                    anim.SetBool("jumping", true);
                    rb.velocity = new Vector2(speed, jumpForce);
                }
                
            }            
            
        }        
    }
    void MovementEagle()
    {
        if (gameObject.tag == "EnemyEagle")
        {
            if (faceTop)
            {
                rb.velocity = new Vector2(rb.velocity.x, speed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -speed);
            }
            if (transform.position.y < bottomPoint.position.y)
            {
                faceTop = true;
            }
            else if (transform.position.y > topPoint.position.y)
            {
                faceTop = false;
            }
        }
    }
    void SwitchAnim()
    {
        if (gameObject.tag == "EnemyFrog")
        {
            if (anim.GetBool("jumping"))
            {
                if (rb.velocity.y < 0.1f)
                {
                    anim.SetBool("falling", true);
                    anim.SetBool("jumping", false);
                }
            }
            if (collider.IsTouchingLayers(ground) && anim.GetBool("falling"))
            {
                anim.SetBool("falling", false);
            }
        }
    }
}
