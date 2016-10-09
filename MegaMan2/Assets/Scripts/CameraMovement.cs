using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 k_PosOffset;
    [SerializeField]
    private GameObject k_Target;

    void Update()
    {
        k_PosOffset = new Vector3(k_Target.transform.position.x, k_PosOffset.y, k_PosOffset.z);
        transform.position = k_PosOffset;
    }
}