using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playercontrol : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private Collider2D discoll, discoll1;
    [SerializeField] private Animator anima;

    public float speed, jumpForce, horizontalMove;
    public Transform groundCheck, headCheck, neckCheck;
    public LayerMask ground;

    public PhysicsMaterial2D forCircle;

    public float SkillTime;//技能发动时间
    private float SkillTimeLeft;//技能剩余时间
    private float lastSkill = -10f;//上一次使用技能的时间点
    public float SkillCoolDown;//技能cd
    public float dashSpeed;

    public bool isGround, isJump, isHurt, isOut, isCrouch, isPause, isGoback, isDash, isHeal, isSlowTime, usingSkill, isSkill;
    public bool isGetskill;
    public int cherry = 0, gem = 0, skillNumber = 4;

    public GameObject ForSkill;
    private Queue<Vector3 > playerPosition = new Queue<Vector3>();
    public Vector3 setPosition,lastPosition;

    public Text CherryNum, GemNum;

    bool jumpPressed;
    int jumpCount;

    private static Playercontrol instanse;
    public static Playercontrol Instance
    {
        get
        {
            return instanse;
        }
    }

    private void Awake()
    {
        instanse = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt && !isPause)
        {

            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0 && !isCrouch)
            {
                jumpPressed = true;
                Jump();
            }
            if (isGround)
            {
                jumpCount = 2;
                isJump = false;
            }
            GroundMovement();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetSkill();
        }
        SwitchAnima();
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground) && rb.velocity.y <= 0;
        LastPosition();
        Skill();
    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }

        Crouch();
    }
    void Crouch()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                isCrouch = true;
                discoll.enabled = false;
                anima.SetBool("crouch", true);
            }
            else if (isCrouch && !Physics2D.OverlapCircle(headCheck.position, 0.1f, ground) && !Input.GetKey(KeyCode.S))
            {
                isCrouch = false;
                discoll.enabled = true;
                anima.SetBool("crouch", false);
                anima.SetBool("idle", true);
            }
        }

        if (!isGround && !isCrouch )
        {
            NeckCheck();
        }

    }
    void Jump()
    {
        if (jumpPressed && isGround)
        {
            jumpPressed = false;
            isGround = false;
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            SoundManage.soundmanage.JumpAudio();

        }
        else if (jumpPressed && jumpCount > 0 && !isGround)
        {
            jumpPressed = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            SoundManage.soundmanage.JumpAudio();
        }
    }

    //选择动画
    void SwitchAnima()
    {
        anima.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (!isHurt)
        {
            if (isGround)
            {
                anima.SetBool("falling", false);
                anima.SetBool("jumping", false);
                anima.SetBool("idle", true);
            }
            else if (!isGround && rb.velocity.y > 0.1)
            {
                anima.SetBool("idle", false);
                anima.SetBool("jumping", true);
                anima.SetBool("falling", false);
            }
            else if (rb.velocity.y < 0)
            {
                anima.SetBool("jumping", false);
                anima.SetBool("falling", true);
            }
        }
        else
        {
            anima.SetBool("hurt", true);
            if (isOut)
            {
                anima.SetBool("idle", false);
            }
            else if (Mathf.Abs(rb.velocity.x) < 0.5)
            {
                isHurt = false;
                anima.SetBool("hurt", false);
            }
        }


    }

    //收集物品与跟敌人的互动  与触发器碰撞触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Col-Cherry")
        {
            Destroy(collision.gameObject);
            cherry++;
            CherryNum.text = (string)(":" + cherry);
            SoundManage.soundmanage.CollectAudio();
        }
        if (collision.tag == "Col-Gem")
        {
            Destroy(collision.gameObject);
            gem++;
            GemNum.text = (string)(":" + gem);
            SoundManage.soundmanage.CollectAudio();
        }
        if (collision.tag == "enemy" || collision.tag == "eagle")
        {
            cherry++;
            CherryNum.text = (string)(":" + cherry);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.2f);
            SoundManage.soundmanage.JumpAudio();
        }
        if (collision.tag == "out")
        {
            rb.velocity = new Vector2(rb.velocity.x, 30);
            isHurt = isOut = true;
            anima.SetBool("out", true);
            SoundManage.soundmanage.HurtAudio();
            Invoke(nameof(Playerout), 2f);
        }
        if (collision .tag == "skill")
        {
            Destroy(collision.gameObject);
            isGetskill = true;
            SoundManage.soundmanage.CollectAudio();
        }
    }
    //                        与碰撞器碰撞触发
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.tag == "eagle")
        {
            isHurt = true;
            int anix = (int)((transform.position.x - collision.gameObject.transform.position.x) / Mathf.Abs(transform.position.x - collision.gameObject.transform.position.x));
            rb.velocity = new Vector2(anix * 10, jumpForce / 2);
            SoundManage.soundmanage.HurtAudio();
            cherry--;
            CherryNum.text = (string)(":" + cherry);
            if (cherry <= 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 30);
                isOut = true;
                anima.SetBool("out", true);
                SoundManage.soundmanage.HurtAudio();
                Invoke(nameof(Playerout), 2f);
            }

        }
        if(!isGround && collision .gameObject .tag == "ground")
        {
            rb.velocity = new Vector2(-1 * horizontalMove, rb.velocity.y);
        }
    }

    void NeckCheck()
    {
        if (Physics2D.OverlapCircle(neckCheck.position, 0.1f, ground))
        {
            discoll.enabled = false;
            rb.velocity = new Vector2( -1 * horizontalMove , rb.velocity.y);
        }
        else
        {
            discoll.enabled = true;
        }
    }
    
    /*void NeckCheck()
    {
        if (!isGround)
        {
            forCircle.friction = 0;
        }
        else
        {
            forCircle.friction = 0.4f;
        }
    }*/


    void Playerout()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.localPosition;
    }
    
    void SetSkill()  //抽牌以获得技能
    {

        if (isGetskill)
        {
            if (!isSkill)
            {
                CanSkill();
            }
            if (isSkill && !usingSkill)
            {
                switch (skillNumber)
                {
                    case 0:
                        usingSkill = true;
                        isDash = true;
                        break;
                    case 1:
                        usingSkill = true;
                        isGoback = true;
                        break;
                    case 2:
                        usingSkill = true;
                        isSlowTime = true;
                        break;
                    case 3:
                        usingSkill = true;
                        isHeal = true;
                        break;
                    default:
                        skillNumber = (int)UnityEngine.Random.Range(0f, 4f);
                        anima.SetBool("getskill", true);
                        break;
                }
            }
        }
        else
        {
            skillNumber = 5;
        }
        ForSkill.SetActive(true);
    }

    void Skill()
    {
        if (isSkill)
        { 
            if (isDash)
            {
                Dash();
            }
            else if (isGoback)
            {
                Goback();
            }
            else if (isSlowTime)
            {
                SlowerTime();
            }
            else if (isHeal)
            {
                GetHeal();
            }
        }
    }    //使用技能

    void CanSkill()
    {
        if(Time.time >= (lastSkill + SkillCoolDown) && !usingSkill )
        {
            isGoback = false;
            isSkill = true;
            SkillTimeLeft = SkillTime;
            lastSkill = Time.time;
        }
        else
        {
            skillNumber = 6;
        }
    } //能否使用技能

    void Goback()
    {
        if (SkillTimeLeft > 0)
        {
            rb.velocity = new Vector2((lastPosition .x  - transform.position.x)*speed, (lastPosition.y - transform.position.y)*speed);

            SkillTimeLeft -= Time.deltaTime;
            ShadowPool.instance.GetFormPool();
            discoll.enabled = false;
            discoll1.enabled = false;
        }
        if (SkillTimeLeft <= 0)
        {
            isSkill = false;
            isGoback = false;
            usingSkill = false;
            discoll.enabled = true;
            discoll1.enabled = true;
            skillNumber = 4;
        }
    }

    void SlowerTime()
    {
        Time.timeScale = 0.5f;
        StartCoroutine(CountTime1());
    }

    private IEnumerator CountTime1()
    {
        yield return new WaitForSecondsRealtime(SkillTimeLeft*20);
        Time.timeScale = 1f;
        skillNumber = 4;
        isSkill = false;
        isSlowTime = false;
        usingSkill = false;
        isDash = false;
    }

    private IEnumerator CountTime2()
    {
        yield return new WaitForSecondsRealtime(SkillTimeLeft * 10);
    }

    void LastPosition() //将当前位置加入队列
    {
        playerPosition.Enqueue(GetPosition());
        Invoke(nameof(SetLastposition), 2f);  // 延时2s执行SetLastposition()
    }

    void SetLastposition() //将队首位置输出队列
    {
        lastPosition = playerPosition.Dequeue();
    }

    void GetHeal()
    {
        cherry = cherry+10;
        CherryNum.text = (string)(":" + cherry);
        skillNumber = 4;
        isSkill = false;
        isHeal  = false;
        usingSkill = false;
    }

    void Dash()
    {
        if (isDash)
        {
            if (SkillTimeLeft > 0)
            {

                if (rb.velocity.y > 0 && !isGround)
                {
                    rb.velocity = new Vector2(dashSpeed * speed * transform .localScale .x/1.5f , jumpForce*1.5f);//在空中Dash向上
                }
                rb.velocity = new Vector2(dashSpeed * speed * transform.localScale.x, rb.velocity.y);//地面Dash

                SkillTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFormPool();
            }
            if (SkillTimeLeft <= 0)
            {
                isDash = false;
                skillNumber = 4;
                isSkill = false;
                usingSkill = false;
            }
        }

    }

    void EndAnima()
    {
        anima.SetBool("getskill", false);
    }
}
