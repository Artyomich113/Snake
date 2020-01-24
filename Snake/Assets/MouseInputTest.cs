using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputTest : MonoBehaviour
{
    void Update()
    {
		Debug.Log(Input.GetAxis("Mouse X") + " " + Input.GetAxis("Mouse Y"));	
    }
}
