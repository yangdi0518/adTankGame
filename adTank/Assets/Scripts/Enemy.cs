using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 属性
    public float moveSpeed=3;
    private Vector3 bullutEulerAngules;
    private float v = -1;
    private float h;

    // 引用
    public Sprite[] tankSprite;
    private SpriteRenderer sr;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;


    // 计时器
    private float timeVal;
    private float timeValChangeDirection;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 攻击时间间隔
        if(timeVal >= 3f) {
            TankAttack();
        } else {
            timeVal += Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        // 固定物理帧生命周期函数，保证每一帧的刚体受力相等,防止抖动效果。
        TankMove();

    }

    private void TankAttack() {
        Quaternion bulletRotation = Quaternion.Euler(transform.eulerAngles + bullutEulerAngules);
        Instantiate(bulletPrefab, transform.position, bulletRotation);
        timeVal = 0;
    }
    private void TankMove() {

        if(timeValChangeDirection >=4) {
            int num = Random.Range(0, 8);

            if(num >=5) {
                v = -1;
                h = 0;
            } else if(num == 0) {
                v = 1;
                h = 0;
            } else if (num >=1 && num <= 2) {
                h = -1;
                v = 0;
            } else if (num >=3 && num <= 4) {
                h = 1;
                v = 0;
            }
            timeValChangeDirection = 0;
        } else {
            timeValChangeDirection += Time.fixedDeltaTime;
        }


        transform.Translate(Vector3.up*v*moveSpeed*Time.fixedDeltaTime, Space.World);

        if (v < 0) {
            sr.sprite = tankSprite[2];
            bullutEulerAngules = new Vector3(0, 0, -180);
        } else if (v > 0) {
            sr.sprite = tankSprite[0];
            bullutEulerAngules = new Vector3(0, 0, 0);
        }

        transform.Translate(Vector3.right*h*moveSpeed*Time.fixedDeltaTime, Space.World);

        if(h < 0) {
            sr.sprite = tankSprite[3];
            bullutEulerAngules = new Vector3(0, 0, 90);
        } else if (h > 0) {
            sr.sprite = tankSprite[1];
            bullutEulerAngules = new Vector3(0, 0, -90);
        }
    }

    private void Die() {

        PlayerManager.Instance.playerScore++;
        // 产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Enemy")
        {
            timeValChangeDirection = 4;
        }
    }

}
