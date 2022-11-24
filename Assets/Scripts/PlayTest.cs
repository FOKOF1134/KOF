using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System;

public class PlayTest : MonoBehaviour
{
    public List<Sprite> test;
    public float speed = 6f;
    public float jumpForce = 11f;

    [SerializeField] private BaShenSO BaShenSO;
    private SpriteRenderer spriteRenderer;
    private Dictionary<ActionType, List<Sprite>> mp = new Dictionary<ActionType, List<Sprite>>();
    private ActionType currentAction = ActionType.idle;
    private Rigidbody2D rb;
    private Collider2D coll;
    private bool onGround;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Type t = BaShenSO.GetType();
        var idle = t.GetField("idle");
        var jump = t.GetField("jump");
        var run = t.GetField("run");
        var idleList = idle.GetValue(BaShenSO) as List<Sprite>;
        var jumpList = jump.GetValue(BaShenSO) as List<Sprite>;
        var runList = run.GetValue(BaShenSO) as List<Sprite>;

        mp.Add(ActionType.idle, idleList);
        mp.Add(ActionType.run, runList);
        mp.Add(ActionType.jump, jumpList);
        PlaySprites();

    }

    private void Update()
    {
        if(onGround){
            if (Input.GetKey(KeyCode.D))
            {
                if (transform.localScale.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                currentAction = ActionType.run;
                //Debug.Log(currentAction);
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                currentAction = ActionType.jump;
                Debug.Log(currentAction);
                rb.velocity = new Vector2(rb.velocity.x,jumpForce);
                Debug.Log("11111111");
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (transform.localScale.x > 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                currentAction = ActionType.run;
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }
    }

    private async void PlaySprites()
    {
        while (true)
        {
            await PlaySingleAction(mp[currentAction]);
        }
    }

    private async Task PlaySingleAction(List<Sprite> sprites)
    {
        var last = currentAction;
        for (int i = 0; i < sprites.Count; i++)
        {
            spriteRenderer.sprite = sprites[i];
            if (last != currentAction) return;
            await UniTask.DelayFrame(50);
        }
        currentAction = ActionType.idle;
    }

    void OnCollisionEnter2D(Collision2D collision){
        isOnGround(collision,true);
    }
    void OnCollisionStay2D(Collision2D collision){
        isOnGround(collision,true);
    }
    void OnCollisionExit2D(Collision2D collision){
        isOnGround(collision,false);
    }

    void isOnGround(Collision2D coll,bool state){
        if((coll.gameObject.layer == LayerMask.NameToLayer("ground"))&&state){
            onGround = true;
        }
        else{
            onGround = false;
        } 
    }
    
    //private async void DoJump()
    //{
    //    isJumping = true;
    //    while(transform.position.y < 3.4)
    //    {
    //        await UniTask.Yield(PlayerLoopTiming.LastUpdate);
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 3.5f, transform.position.z), Time.deltaTime * 10);
    //    }
    //    while (transform.position.y > 0)
    //    {
    //        await UniTask.Yield(PlayerLoopTiming.LastUpdate);
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0, transform.position.z), Time.deltaTime * 10);
    //    }
    //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    //    isJumping = false;
    //}
}


public enum ActionType
{
    idle,
    run,
    jump,
}