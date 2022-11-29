using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject AI_Player, Player, Ball, PlayerBall;
    public Text leftScoreText, rightScoreText;
    public GameObject LineContainer, PlayerContainer, Camera, RightWall;

    private Ball ball;
    private PlayerBall playerBall;
    private Racket leftRacket, rightRacket;
    private int _playerScore, _computerScore, _playerCount;
    private bool isExploded;
    

    private void Start()
    {
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
                RightWall.transform.position = new Vector3(90, 0, 0);
                Destroy(PlayerContainer.transform.Find("Ball").gameObject);
                InitPlayerBall();
                
            }
            
            isExploded = true;

            leftRacket.DeactivateComputer();
            leftRacket.FreeRacket();

            rightRacket.DeactivateComputer();
            rightRacket.FreeRacket();

            ExplodeLines();

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
            x = Random.Range(-15, 15);
            y = Random.Range(-15, 15);
            torque = Random.Range(-15, 15);
            LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Rigidbody2D>().AddForce(new Vector2(x * 10, y * 10));
            LineContainer.transform.Find(string.Format("Line Segment {0}", i)).GetComponent<Rigidbody2D>().AddTorque(torque);
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
        }
        else
        {
            PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>().SetLeft(false);
            rightRacket = PlayerContainer.transform.Find(aiName).GetComponent<ComputerRacket>();
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
