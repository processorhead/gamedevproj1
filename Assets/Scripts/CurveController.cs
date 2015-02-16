using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class CurveController : MonoBehaviour 
{
    public List<Vector2> EtalonCurve;
    public int LevelIndex = 1;
    public int Score = 0;
    public LineRenderer CurveImage;

    void Awake()
    {
        CurveImage.SetWidth(0.15f, 0.15f);
    }

    public void LoadCurve(int index)
    {
        EtalonCurve = new List<Vector2>();
        string s = "";
        using (StreamReader sr = File.OpenText("game_Data/Curves/" + index + ".txt"))
        {
            bool ok = true;
            do
            {
                s = sr.ReadLine();
                try
                {
                    EtalonCurve.Add(new Vector2(float.Parse(s.Split('/')[0]), float.Parse(s.Split('/')[1])));
                }
                catch
                {
                    ok = false;
                }
            }
            while (ok);
        } 
        CurveImage.SetVertexCount(EtalonCurve.Count);
        for (int i = 0; i < EtalonCurve.Count; i++)
        {
            CurveImage.SetPosition(i, EtalonCurve[i]/15);
        }
    }

}
