using UnityEngine;

public class TestCamera : MonoBehaviour
{
    private float degrees = 1;
	
	void Update ()
    {
        gameObject.transform.RotateAround(new Vector3(), new Vector3(0, 1, 0), degrees);
	}
}
