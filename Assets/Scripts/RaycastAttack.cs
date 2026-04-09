using Fusion;
using UnityEngine;

public class RaycastAttack : NetworkBehaviour
{
    public float Damage = 10;
    public FirstPersonController PlayerMovement;

    private void Update()
    {
        if (HasStateAuthority == false)
            return;

        Ray ray = PlayerMovement.playerCamera.ScreenPointToRay(Input.mousePosition);
        ray.direction += PlayerMovement.playerCamera.transform.forward;



        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
            if (Physics.Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.TryGetComponent<FirstPersonController>(out var firstPersonController))
                {
                    firstPersonController.DealDamageRpc(Damage);
                }
            }

        }

    }
}