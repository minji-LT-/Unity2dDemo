using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int cherry, Diamond;

    private bool isHurt;
    public float speed;
    public float jumpforce;
    public LayerMask ground;
    public Collider2D circelCollider;
    public Collider2D cubeCollider;
    public Transform ceilPoint;

    public UnityEngine.UI.Text CherryNumberLabel;
    public UnityEngine.UI.Text DiamondNumberLabel;

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        
        if (!isHurt)
        {
            Movement();
        }
        SwitchAnim();
    }
    void Movement()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        // 角色移动
        if (horizontalMove != 0)
        {
            rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
            int direction = horizontalMove > 0 ? 1 : -1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
            anim.SetFloat("running", Mathf.Abs(horizontalMove));
        }
        // 角色跳跃
        if (Input.GetKey(KeyCode.Space) && circelCollider.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
            anim.SetBool("jumping", true);
            audio.Play();
        }

        Crouch();
    }
    void SwitchAnim()
    {
        anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !circelCollider.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1)
            {
                isHurt = false;
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
            }
            
        } 
        else if (circelCollider.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag =="DeadLine")
        {
            Invoke(nameof(Restart), 2);
            return;
        }
        if (collision.gameObject.tag == "Collection")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            CherryNumberLabel.text = cherry.ToString();
        } else if (collision.gameObject.tag == "Collection2")
        {
            Destroy(collision.gameObject);
            ++Diamond;
            DiamondNumberLabel.text = Diamond.ToString();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "EnemyFrog" || collision.gameObject.tag == "EnemyEagle")
        {
            if (anim.GetBool("falling"))
            {
                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
                anim.SetBool("jumping", true);
            }
            else
            {
                if (transform.position.x < collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(-5, rb.velocity.y);
                    isHurt = true;
                }
                else if (transform.position.x > collision.gameObject.transform.position.x)
                {
                    rb.velocity = new Vector2(5, rb.velocity.y);
                    isHurt = true;
                }
            }
        }
        
    }
    // 人物趴下
    public void Crouch()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("crouching", true);
            cubeCollider.enabled = false;
        } else 
        {
            if (!Physics2D.OverlapCircle(ceilPoint.position, 0.2f, ground))
            {
                anim.SetBool("crouching", false);
                cubeCollider.enabled = true;
            }
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
