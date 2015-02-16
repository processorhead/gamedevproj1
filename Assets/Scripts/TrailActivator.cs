using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class TrailActivator : MonoBehaviour 
{
    public GameObject trail;
    public GameController control;
    public int MapWidth = 50;
    public float eps = 5;
    public float len_diff = 0.15f;
    public Text timertext;
    public CurveController cc;

    GameObject TrailActive;
    List<Vector3> Curve;
    
    bool GoodCurve;

    public int LevelDuration = 50;
    public int TimePerLevel = 50;
    float ClockSpeed = 0.3f;
    public int TimerDecreasing = 2;

    void Awake()
    {
        InvokeRepeating("Clock", 0, ClockSpeed);
    }

    void Clock()
    {
        timertext.text = "Time: " + TimePerLevel;
        if (control.GameStarted)
        {
            if (TimePerLevel > 0)
                TimePerLevel--;
            if (TimePerLevel == 0)
                GameOver();
        }
    }

    void GameOver()
    {
        Destroy(TrailActive);
    }

    void Start()
    {
        cc.LoadCurve(cc.LevelIndex);
        Curve = new List<Vector3>();
    }

    void LevelUp()
    {
        try
        {
            cc.LoadCurve(++cc.LevelIndex);
        }
        catch
        {
            cc.LevelIndex = 1;
            cc.LoadCurve(cc.LevelIndex);
        }
        cc.Score += 1;
        TimePerLevel = LevelDuration-cc.Score*TimerDecreasing;
        Debug.ClearDeveloperConsole();
        Debug.Log("LEVEL " + cc.LevelIndex);
    }

	void Update() 
    {
        if (control.GameStarted)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 pos = r.GetPoint(10);
                Vector3 pos2 = new Vector3(pos.x, pos.y, -0.5f);
                TrailActive = (GameObject)Instantiate(trail, pos2, transform.rotation);
            }
            if (Input.GetButton("Fire1"))
            {
                if (Curve.Count > 0)
                {
                    if (Vector3.Distance(Curve[Curve.Count - 1], Input.mousePosition) > 3)
                        Curve.Add(Input.mousePosition);
                }
                else
                    Curve.Add(Input.mousePosition);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                Destroy(TrailActive);

                float topx, topy, botx, boty = 0;
                topx = Curve.Max<Vector3>(a => a.x);
                topy = Curve.Max<Vector3>(a => a.y);
                botx = Curve.Min<Vector3>(a => a.x);
                boty = Curve.Min<Vector3>(a => a.y);
                float sqlen = Mathf.Max(topx - botx, topy - boty);
                Vector3 center = new Vector3((topx + botx) / 2, (topy + boty) / 2, 0);
                for (int i = 0; i < Curve.Count; i++)
                    Curve[i] -= center;
                for (int i = 0; i < Curve.Count; i++)
                    Curve[i] *= ((float)MapWidth / (sqlen));
                for (int i = 0; i < Curve.Count; i++)
                    Curve[i] += new Vector3(MapWidth / 2, MapWidth / 2, 0);
                bool wrong = false;
                for (int j = 0; j < Curve.Count; j++)
                {
                    Vector2 c = new Vector2(Curve[j].x, Curve[j].y);
                    wrong = true;
                    for (int i = 0; i < cc.EtalonCurve.Count - 1; i++)
                    {
                        float a = Vector2.Angle(cc.EtalonCurve[i + 1] - cc.EtalonCurve[i], c - cc.EtalonCurve[i]);
                        if (Vector2.Distance(cc.EtalonCurve[i], c) <= eps || Vector2.Distance(cc.EtalonCurve[i + 1], c) <= eps)
                        {
                            wrong = false;
                        }
                        else
                        {
                            if (a < 90 && (cc.EtalonCurve[i + 1] - cc.EtalonCurve[i]).magnitude > (c - cc.EtalonCurve[i]).magnitude)
                            {
                                float dist = (cc.EtalonCurve[i] - c).magnitude * Mathf.Sin(a * 3.14f / 360f);
                                if (dist <= eps)
                                {
                                    wrong = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (wrong)
                    {
                        break;
                    }
                }
                float len1 = 0, len2 = 0;
                for (int i = 0; i < cc.EtalonCurve.Count - 1; i++)
                    len1 += (cc.EtalonCurve[i + 1] - cc.EtalonCurve[i]).magnitude;
                for (int i = 0; i < Curve.Count-1; i++)
                    len2 += (Curve[i + 1] - Curve[i]).magnitude;
                if (Mathf.Abs(len2 - len1) / len1 > len_diff)
                {
                    wrong = true;
                }
                if (!wrong)
                {
                    LevelUp();
                }
                Curve.Clear();
            }
        }
	}
}
