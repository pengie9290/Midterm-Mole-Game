using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    //Creates a singleton
    public static PlayerControl Instance = null;

    //Determines player's movement speed
    public float Acceleration = 1;

    //Determines player's turning speed
    public float TurnSpeed = 1;

    //Determines maximum speed the player can move at
    public float MaxSpeed = 1;

    //Stores player's rigidbody 
    public Rigidbody2D rb;

    //Stores live player sprite 
    public SpriteRenderer LiveMole;

    //Stores dead player sprite
    public GameObject DeadMole;

    //Keeps track of whether or not player is alive
    public bool Alive = true;

    //Keeps track of life count
    public int Lives = 3;

    //Keeps track of amount of food
    public int FoodCount = 4;

    //Stores Life Count Message
    public Text LifeCountMessage;

    //Stores Victory Message
    public Text VictoryMessage;

    //Stores Game Over Message
    public Text GameOverMessage;

    //Stores "Press Space" Message
    public Text PressSpaceMessage;

    //Defines the game's states
    public enum GameStates
    {
        Playing,
        Victory,
        YouLose
    }

    //Keeps track of current gamestate
    private GameStates gameState;

    //Allows reading and changing of current gamestate
    public GameStates GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            GameStates OldState = gameState;
            if (value != gameState)
            {
                gameState = value;
                StateHasChanged(OldState, gameState);
            }
        }
    }

    //Responds to changes in state
    void StateHasChanged(GameStates OldValue, GameStates NewValue)
    {
        switch (NewValue)
        {
            case GameStates.Playing:
                StartPlaying();
                break;
            case GameStates.Victory:
                LoadVictory();
                break;
            case GameStates.YouLose:
                LoadGameOver();
                break;
            default:
                break;
        }
    }

    //Starts the game
    void StartPlaying()
    {
        VictoryMessage.gameObject.SetActive(false);
        GameOverMessage.gameObject.SetActive(false);
        PressSpaceMessage.gameObject.SetActive(false);
        Lives = 3;
        UpdateLifeCounter();
        RestartScene();
    }

    //Loads Victory
    void LoadVictory()
    {
        VictoryMessage.gameObject.SetActive(true);
        PressSpaceMessage.gameObject.SetActive(true);
    }

    //Loads Game Over
    void LoadGameOver()
    {
        GameOverMessage.gameObject.SetActive(true);
    }

    void Start()
    {
        //Sets the player as a Singleton
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //Determines player's rigidbody
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Makes sure player starts alive
        Alive = true;
    }

    void FixedUpdate()
    {
        switch (GameState)
        {
            case GameStates.Playing:
                Playing();
                break;
            case GameStates.Victory:
            case GameStates.YouLose:
            default:
                WaitForRestart();
                break;
        }
    }

    void Playing()
    {
        if (Alive)
        {
            //Determines when to have player move
            if (Input.GetKey(KeyCode.W)) PlayerRun(-1);
            else if (Input.GetKey(KeyCode.S)) PlayerRun(1);
            else Decelerate();

            //Determines when to have player turn
            if (Input.GetKey(KeyCode.D)) PlayerTurn(-1);
            if (Input.GetKey(KeyCode.A)) PlayerTurn(1);

            //Enforces max player speed
            if (rb.velocity.magnitude > MaxSpeed) rb.velocity = rb.velocity.normalized * MaxSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
            if (Lives > 0 && Input.GetKeyDown(KeyCode.Space))
            {
                RevivePlayer();
            }
        }
    }

    //Makes player move
    void PlayerRun(int direction)
    {
        Vector3 NewForce = transform.up * Acceleration * 1000 * Time.deltaTime * direction;
        rb.AddForce(NewForce);
    }

    //Slows player down when no key is held
    void Decelerate()
    {
        if (rb.velocity.magnitude > 0) rb.velocity = rb.velocity.normalized * rb.velocity.magnitude / 2;
    }

    //Makes player turn
    void PlayerTurn(int turn)
    {
        transform.Rotate(transform.forward, TurnSpeed * turn * Time.deltaTime * 100);
    }

    //Tells player object what to do when caught
    public void PlayerCaught()
    {
        if (Alive)
        {
            PressSpaceMessage.gameObject.SetActive(true);
            Lives--;
            UpdateLifeCounter();
            Alive = false;
            LiveMole.enabled = false;
            DeadMole.SetActive(true);
            if (Lives < 1) GameState = GameStates.YouLose;
        }
    }

    //Revives player at the cost of a life
    void RevivePlayer()
    {
        PressSpaceMessage.gameObject.SetActive(false);
        Alive = true;
        LiveMole.enabled = true;
        DeadMole.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
    }

    //Tells player object that they caught food
    public void FoodCaught()
    {
        FoodCount--;
        if (FoodCount < 1) GameState = GameStates.Victory;
    }

    //Tells the game when to restart the scene
    void WaitForRestart()
    {
        //Stops player from sliding after victory or death
        rb.velocity = Vector3.zero;

        //Changes game state to playing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameState = GameStates.Playing;
        }
    }

    //Updates the life counter
    void UpdateLifeCounter()
    {
        LifeCountMessage.text = "Lives: " + Lives;
    }

    //Restarts the scene
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
