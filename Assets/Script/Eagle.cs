using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private Collider2D discoll1,discoll2,discoll3;
    [SerializeField] private Animator anima;
    private Vector3 startingPosition;
    public float speed;

    void Start()
    {
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        //Fly();
    }

    public static Vector3 GetRoadomDir()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public void Moveto(Vector3 targetPosition)
    {
        
            rb.velocity = new Vector2((targetPosition.x - transform.position.x)/speed,(targetPosition.y - transform.position.y)/speed );
        
        if (rb.velocity.x != 0)
        {
            float _scale = transform.localScale.y;
            transform.localScale = new Vector3((-rb.velocity.x / Mathf.Abs(rb.velocity.x)) * _scale, _scale, 1);
        }
    }
    private Vector3 GetRoamingPosition()
    {
        return startingPosition + GetRoadomDir() * Random.Range(10f, 30f);
    }

    void Fly()
    {
        float targetRange = 20f;
        if (Vector3.Distance(transform.position, Playercontrol.Instance.GetPosition()) < targetRange )
        {
            speed = 1;
            Moveto(Playercontrol.Instance.GetPosition());
        }
        else if (Vector3 .Distance (transform .position ,startingPosition )<10f)
        {
            speed = 5;
            Vector3 aimPosition = GetRoamingPosition();
            Moveto(aimPosition);  
        }
        else
        {
            Moveto(startingPosition);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" )
        {
            discoll1.enabled = false;
        }
    } //碰撞体相互碰撞

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "enemy")
        {
            discoll1.enabled = true;
        }
    }     //碰撞体离开触发器

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float _scale = transform.localScale.y;
            if (_scale == 1)
            {
                anima.SetBool("death", true);
                SoundManage.soundmanage.DestroyAudio();
                discoll1.enabled = false;
                discoll2.enabled = false;
                discoll3.enabled = false;
            }
            else
            {

                _scale--;

                int anix = (int)((transform.position.x - collision.gameObject.transform.position.x) / Mathf.Abs(transform.position.x - collision.gameObject.transform.position.x));
                rb.velocity = new Vector2(anix * 20, 30);

                transform.localScale= new Vector3(anix * _scale, _scale, 1);
            }
            
        }

    }    //碰撞体碰撞触发器

    void Destroy()
    {
        Destroy(gameObject);
    }
}
