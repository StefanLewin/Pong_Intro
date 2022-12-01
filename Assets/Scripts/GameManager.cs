
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject AI_Player, Player, Ball, PlayerBall;
    public Text leftScoreText, rightScoreText;
    public GameObject LineContainer, PlayerContainer, BoundaryContainer, Camera, RightWall;

    private Ball ball;
    private PlayerBall playerBall;
    private Racket leftRacket, rightRacket;
    private SpriteRenderer leftRacketSprite, rightRacketSprite;
    private int _playerScore, _computerScore, _playerCount;
    private bool isExploded;
    

    private void Start()
    {
        DeactivateLineCollision();
        Instantiate(Ball, new Vector3(0, 0, 0), Quaternion.identity, PlayerContainer.transform);
        PlayerContainer.transform.GetChild(0).gameObject.name = "Ball";
        ball = PlayerContainer.transform.Find("Ball").gameObject.GetComponent<Ball>();
        _playerCount = 0;
        InitComputerPlayer(true);
        InitComputerPlayer(false);

        isExploded = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z)) 
        {
            if (!isExploded)
            {                
                StartCoroutine(Fade(leftRacketSprite));
                StartCoroutine(Fade(rightRacketSprite));

                leftRacket.FreeRacket();
                rightRacket.FreeRacket();
                Destroy(PlayerContainer.transform.Find("Ball").gameObject);
                InitPlayerBall();

                for (int i = 0; i <= 3; i++)
                {
                    Physics2D.IgnoreCollision(leftRacket.getCollider(), BoundaryContainer.transform.GetChild(i).GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(rightRacket.getCollider(), BoundaryContainer.transform.GetChild(i).GetComponent<Collider2D>());
                }

                ExplodeLines();

                RightWall.transform.position = new Vector3(90, 0, 0);
                
                
                
            }
            
            isExploded = true;



        } 
        else if (Input.GetKey(KeyCode.Alpha1) && _playerCount == 0)
        {
            InitHumanPlayer(true, true);
          
        } 
        else if (Input.GetKey(KeyCode.Alpha2) && _playerCount == 1)
        {
            InitHumanPlayer(false, false);
        }
    }

    public void LeftScores()
    {
        _playerScore++;
        leftScoreText.text = _playerScore.ToString();
        ResetRound();
        //Debug.Log(string.Format("Spieler hat getroffen: {0}", _playerScore));
    }

    public void RightScores()
    {
        _computerScore++;
        rightScoreText.text = _computerScore.ToString();
        ResetRound();
        //Debug.Log(string.Format("Computer hat getroffen: {0}", _computerScore));
    }

    IEnumerator Fade(SpriteRenderer sprite)
    {
        if(sprite != null)
        {
            Color initialColor = sprite.color;
            Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

            float elapsedTime = 0f, fadeDuration = 0.1f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                sprite.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
                yield return null;
            }
        }
    }

    private void ResetRound()
    {
        if (!isExploded)
        {
            this.ball.ResetPosition();
            leftRacket.ResetPosition();
            rightRacket.ResetPosition();
            this.ball.AddStartingForce();
        }
    }

    private void ExplodeLines()
    {
        float x, y, torque;

        for (int i = 1; i <= 9 ; i++)
        {
            if(LineContainer.transform.Find(string.Format("Line Segment {0}", i)) != null)
            {
                x = Random.Range(-50, 50);
                y = Random.Range(-50, 50);
                torque = Random.Range(-15, 15);
                StartCoroutine(Fade(LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<SpriteRenderer>()));
                LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Collider2D>().enabled = true;

                for (int j = 0; j <= 3; j++)
                {
                    Physics2D.IgnoreCollision(LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Collider2D>(), BoundaryContainer.transform.GetChild(j).GetComponent<Collider2D>());
                }
                
                LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Rigidbody2D>().AddForce(new Vector2(x * 10, y * 10));
                LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Rigidbody2D>().AddTorque(torque);
                LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            }
        }
    }

    private void DeactivateLineCollision()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (LineContainer.transform.Find(string.Format("Line Segment {0}", i)) != null)
            {
                LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private void InitComputerPlayer(bool onLeftSide)
    {
        Vector3 aiLocation = onLeftSide ? new Vector3(-8, 0, 0) : new Vector3(8, 0, 0);
        string aiName = onLeftSide ? "ComputerLeft" : "ComputerRight";

        Instantiate(AI_Player, aiLocation, Quaternion.identity, PlayerContainer.transform);
        PlayerContainer.transform.GetChild(PlayerContainer.transform.childCount - 1).gameObject.name = aiName;
        PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>().SetBall(ball);

        if (onLeftSide)
        {
            PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>().SetLeft(true);
            leftRacket = PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>();
            leftRacketSprite = PlayerContainer.transform.Find(aiName).GetComponent<SpriteRenderer>();
        }
        else
        {
            PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>().SetLeft(false);
            rightRacket = PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>();
            rightRacketSprite = PlayerContainer.transform.Find(aiName).GetComponent<SpriteRenderer>();
        }
    }

    private void InitHumanPlayer(bool onLeftSide, bool isFirstPlayer)
    {
        string computerName = onLeftSide ? "ComputerLeft" : "ComputerRight";
        string playerName = onLeftSide ? "PlayerLeft" : "PlayerRight";

        GameObject aiPlayerReplace = onLeftSide ?   PlayerContainer.transform.Find(computerName).gameObject : 
                                                    PlayerContainer.transform.Find("ComputerRight").gameObject;
        Instantiate(Player, aiPlayerReplace.transform.position, Quaternion.identity, PlayerContainer.transform);
        PlayerContainer.transform.GetChild(PlayerContainer.transform.childCount - 1).gameObject.name = playerName;

        if (isFirstPlayer) PlayerContainer.transform.Find(playerName).GetComponent<PlayerRacket>().setFirstPlayer();
        
        if (onLeftSide)
        {
            leftRacket = PlayerContainer.transform.Find(playerName).GetComponent<PlayerRacket>();
        } else
        {
            rightRacket = PlayerContainer.transform.Find(playerName).GetComponent<PlayerRacket>();
        }
       
        Destroy(PlayerContainer.transform.Find(computerName).gameObject);
        
        _playerCount++; ;
    }

    private void InitPlayerBall()
    {
        Instantiate(PlayerBall, new Vector3(0,0,0), Quaternion.identity, PlayerContainer.transform);
        PlayerContainer.transform.GetChild(PlayerContainer.transform.childCount - 1).gameObject.name = "PlayerBall";
        playerBall = PlayerContainer.transform.Find("PlayerBall").gameObject.GetComponent<PlayerBall>();

        Physics2D.IgnoreCollision(playerBall.GetComponent<Collider2D>(), leftRacket.getCollider());
        Physics2D.IgnoreCollision(playerBall.GetComponent<Collider2D>(), rightRacket.getCollider());

        for (int i = 1; i <= 9; i++)
        {
            Physics2D.IgnoreCollision(playerBall.GetComponent<Collider2D>(), LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Collider2D>());
        }

        ActivateCameraFollow();        
    }

    private void ActivateCameraFollow()
    {
        if(PlayerContainer.transform.Find("PlayerBall").gameObject != null)
        {
            Camera.AddComponent<CameraFollow>();
            Camera.GetComponent<CameraFollow>().setTarget(PlayerContainer.transform.Find("PlayerBall").transform);
        }
        else
        {
            Debug.Log("IS NULL");
        }
        
    }
}
