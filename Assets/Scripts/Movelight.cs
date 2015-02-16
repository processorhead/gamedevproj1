using UnityEngine;
using System.Collections;

public class Movelight : MonoBehaviour {

    public float Distance = 10;
	
	void Update () 
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = r.GetPoint(Distance);
        transform.position = new Vector3(pos.x,pos.y,-0.5f);
	}

}
