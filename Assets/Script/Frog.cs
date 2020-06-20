using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D discoll1,discoll2;
    [SerializeField] private Animator anima;

    public LayerMask ground;

    private float leftx, rightx;
    public float speed, jumpForce;
    public Transform leftpoint,rightpoint,GroundCheck;
    public bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();

        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }


    // Update is called once per frame
    //void Update()
    //{
    //    SwitchAni();
    //}

    private void FixedUpdate()
    {
        SwitchAni();
    }

    void Movement()
    {
        isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, ground) && rb.velocity.y <= 0;
        if ( isGround )
        {
            float anix = 1;
            if (transform .position .x < leftx)
            {
                anix = 1;
            }
            if (transform .position .x > rightx)
            {
                anix = -1;
            }
            rb.velocity = new Vector2( anix*speed, jumpForce);
            transform.localScale = new Vector3( -anix , 1, 1);
        }
    }
    void SwitchAni()
    {
        if (isGround)
        {
            anima.SetBool("idle", true);
            anima.SetBool("falling", false);
        }
        else if(rb.velocity .y > 0)
        {
            anima.SetBool("idle", false);
            anima.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anima.SetBool("falling", true);
            anima.SetBool("jumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anima.SetBool("death", true);
            discoll1.enabled = false;
            discoll2.enabled = false;
            rb.gravityScale = 0;
            SoundManage.soundmanage.DestroyAudio();
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
