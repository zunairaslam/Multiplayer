using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 direction = transform.position - cam.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
