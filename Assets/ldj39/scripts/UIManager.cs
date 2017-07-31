using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text powerCounter;

	void Start ()
    {
		
	}

	void Update ()
    {
        int power = GameManager.gm.curPower;
        int max = GameManager.gm.maxPower;

        powerCounter.text = power + " / " + max;
	}
}
