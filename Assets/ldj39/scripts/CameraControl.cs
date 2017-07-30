using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dxRate = 1;
    public float dzRate = 1;

    private Vector3 translation = new Vector3();

	void Start ()
    {
		
	}
	
	void Update ()
    {
        checkInput();	
	}

    private void checkInput()
    {
        float dx = Input.GetAxis("Horizontal") * dxRate * Time.deltaTime;
        float dz = Input.GetAxis("Vertical") * dzRate * Time.deltaTime;
        
        if (dx != 0 || dz != 0)
        {
            translation.Set(dx, 0, dz);
            transform.Translate(translation, Space.World);
        }
    }
}
