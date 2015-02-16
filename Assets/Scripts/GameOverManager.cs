using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour 
{
    public float restartDelay = 5f;
    public TrailActivator tact;
    public Text GameOverText;
    public GameController gc;
    public CurveController cc;
    public Button RestartButton;

    Animator anim;
    float restartTimer;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
    }

    void Update ()
    {
        if (gc.GameStarted) 
            if(tact.TimePerLevel <= 0)
            {
                GameOverText.text="GAME OVER!";
                GameOverText.text += "\n";
                GameOverText.text += "Your score: " + cc.Score;
                anim.SetTrigger ("GameOver");
                restartTimer += Time.deltaTime;
                if (restartTimer >= restartDelay)
                {
                    RestartButton.active = true;
                }
            }
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
