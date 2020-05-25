using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxis
{
    MouseH,
    MouseV
}
public class MouseLook : MonoBehaviour
{
    [Header("Rotation Variables")]
    //vertical axis on camera, horizontal on mouse
    public RotationAxis axis = RotationAxis.MouseH;
    [Range(0, 200)]
    public float sensitivity = 100;
    public float minY = -60, maxY = 60;
    private float _rotY = 0;

    void Start()
    {
        //if on player, freeze rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        //if on camera, set axis to vertical
        if (GetComponent<Camera>())
        {
            axis = RotationAxis.MouseV;
        }
    }
    void Update()
    {
        
        if (axis == RotationAxis.MouseH)
        {
            //get horizontal mouse input and rotate y axis
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime, 0);
        }
        else
        {
            //get vertical mouse input, clamp between min and max and rotate x axis
            _rotY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            _rotY = Mathf.Clamp(_rotY, minY, maxY);
            transform.localEulerAngles = new Vector3(-_rotY, 0, 0);
        }
    }
}













