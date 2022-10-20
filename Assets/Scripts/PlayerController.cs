using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Info")]
    public int speed;
    public int jumpForce;
    public int lives;
    public float levelTime; //seconds
    public Canvas canvas;

    private Rigidbody2D rb;
    private GameObject foot;
    private bool isJumping;
    private SpriteRenderer sprite;
    private Animator animation;
    private HUDController hud;

    private float startTime;
    private int spentTime;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //find the reference to the foot gameoject child
        foot = transform.Find("foot").gameObject;
        //get the sprite componente of the child sprite object
        sprite = gameObject.transform.Find("player-idle-1").GetComponent<SpriteRenderer>();

        animation = gameObject.transform.Find("player-idle-1").GetComponent<Animator>();

   

        // get the HUD controller
        hud = canvas.GetComponent<HUDController>();
        hud.SetLiveText(lives);

        
            
    }

    private void FixedUpdate()
    {
        //get the value from -1 to 1 of the horizontal move
        float inputX = Input.GetAxis("Horizontal");
        //apply phisic velocity to the object with the move value * speed
        //the y coordenate is the same
        rb.velocity = new Vector2 (inputX * speed, rb.velocity.y);
    }

    private void Update()
    {
        //pressing space and touching the ground
        if (Input.GetKeyDown(KeyCode.Space) && TouchGround() && !isJumping)
        {
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //moving right
        if (rb.velocity.x > 0) sprite.flipX = false;
        //moving left
        else if (rb.velocity.x < 0) sprite.flipX = true;

        //Play Animations
        PlayerAnimate();

        //Calculate if the time is finished
      //  int timeLeft = levelTime - (int)(Time.time);

        if (levelTime <= 0)
        {

            WinLevel(false);
            Debug.Log("LOSE, TIME IS OVER");

        } 
        else
        {
            levelTime -= Time.deltaTime;
            hud.SetTimeText((int)levelTime);
        }

        //TODO: End Game
        int minutes = (int)levelTime / 60;
        int seconds = (int)levelTime % 60;
       
    }
    /// <summary>
    /// Check if touching the ground
    /// </summary>
    /// <returns>if touching or not</returns>
    private bool TouchGround()
    {
        //Send a imaginary line down 0.2f distance 
        RaycastHit2D hit = Physics2D.Raycast(foot.transform.position , Vector2.down, 0.2f);
        
        // touching something
        return hit.collider != null;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping=false;
        }
    }

    /// <summary>
    /// Reduce live to the player
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        lives -= damage;
        hud.SetLiveText((int)lives);
        Debug.Log("Player Lives: " + lives);

        if(lives == 0)
        {
            WinLevel(false);
            Debug.Log("LOSE!!!!");

            //TODO: change scene
        }
    }

    /// <summary>
    /// Control by code all the animation states
    /// </summary>
    private void PlayerAnimate() 
    {
        // Player jumping
        if (!TouchGround()) animation.Play("Player_Jump");
        // Player running
        else if ((rb.velocity.x > 0) || (rb.velocity.x < 0) && (rb.velocity.y == 0))
            animation.Play("Player_Running");
        // Player idle
        else if (TouchGround() && Input.GetAxisRaw("Horizontal") == 0)
            animation.Play("Player_idle");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {

            // Drestroy the PowerUp
            Destroy(collision.gameObject);
            Invoke(nameof(InfoPowerUp), 0.1f);
   
        }
    }

    private void InfoPowerUp()
    {
        //TODO: write in HUD how many PowerUp left
       int powerUpsNum = GameObject.FindGameObjectsWithTag("PowerUp").Length;
        hud.SetPowerUpsText(powerUpsNum);

        Debug.Log("PowerUps: " + powerUpsNum);

        if (powerUpsNum == 0)
        {
            WinLevel(true);
            Debug.Log("WIN!!!!");
        }
    }

    private void WinLevel(bool win)
    {
        GameManager.instance.Win = true;
        GameManager.instance.Score = (lives * 1000) + ((int)levelTime * 100);

    }

}
