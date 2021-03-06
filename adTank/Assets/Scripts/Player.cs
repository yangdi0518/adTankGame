using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 属性
    public float moveSpeed=3;
    private Vector3 bullutEulerAngules;
    private float timeVal;
    private float defendTimeVal=3;
    private bool isDefended=true;

    // 引用
    private SpriteRenderer sr;
    public Sprite[] tankSprite;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject defendEffectPrefab;
    
    public AudioSource moveAudio;
    public AudioClip[] tankAudio;

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
        // 判断是否处于无敌状态
        if(isDefended) {
            defendTimeVal -= Time.deltaTime;
            defendEffectPrefab.SetActive(true);
            if(defendTimeVal <=0) {
                isDefended=false;
                defendEffectPrefab.SetActive(false);
            }
        }
    }
    private void FixedUpdate() {
        if (PlayerManager.Instance.isDefeat)
        {
            return;
        }


        // 固定物理帧生命周期函数，保证每一帧的刚体受力相等,防止抖动效果。
        TankMove();
        // TankAttack();
        // 攻击CD
        if(timeVal >= 0.8f) {
            TankAttack();
        } else {
            timeVal += Time.fixedDeltaTime;
        }

    }

    private void TankAttack() {
        if(Input.GetKeyDown(KeyCode.Space)) {

            // 子弹产生的角度 = 当前坦克的角度 + 旋转的角度
            Quaternion bulletRotation = Quaternion.Euler(transform.eulerAngles + bullutEulerAngules);
            Instantiate(bulletPrefab, transform.position, bulletRotation);
            timeVal = 0;
        }
    }
    private void TankMove() {
        float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up*v*moveSpeed*Time.fixedDeltaTime, Space.World);

        if (v < 0) {
            sr.sprite = tankSprite[2];
            bullutEulerAngules = new Vector3(0, 0, -180);
        } else if (v > 0) {
            sr.sprite = tankSprite[0];
            bullutEulerAngules = new Vector3(0, 0, 0);
        }

        if (Mathf.Abs(v)>0.05f)
        {
            moveAudio.clip = tankAudio[1];
            
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

        if (v != 0) {
            // 上下优先级高，防止多方向同时移动
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right*h*moveSpeed*Time.fixedDeltaTime, Space.World);

        if(h < 0) {
            sr.sprite = tankSprite[3];
            bullutEulerAngules = new Vector3(0, 0, 90);
        } else if (h > 0) {
            sr.sprite = tankSprite[1];
            bullutEulerAngules = new Vector3(0, 0, -90);
        }

        if (Mathf.Abs(h) > 0.05f)
        {
            moveAudio.clip = tankAudio[1];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }
        else
        {
            moveAudio.clip = tankAudio[0];

            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
        }

    }

    private void Die() {

        if(isDefended) {
            return;
        }

        PlayerManager.Instance.isDead = true;
        // 产生爆炸特效
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(gameObject);

    }

}
