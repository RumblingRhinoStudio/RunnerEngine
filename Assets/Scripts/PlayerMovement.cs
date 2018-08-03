using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsDead = false;    
    public FloatReference MovementSpeed;
    public RunnerPlayerWeapon Weapon;

    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isAttackReloading = false;
    private bool isDying = false;

    // Update is called once per frame
    void Update()
    {
        if (!isDying && !IsDead)
        {
            move();
            attack();
        }
    }

    private void move()
    {
        if (isMoving)
        {
            this.transform.Translate(Vector3.forward * MovementSpeed.Value * Time.deltaTime);
        }
    }

    private void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking && !isAttackReloading)
            {
                Transform target = getTargetUnderMouse();
                if (target != null)
                {
                    isAttacking = true;
                    isAttackReloading = true;
                    GameObject currentProjectile = Instantiate(Weapon.Prefab, transform.position + Vector3.forward * 1.5f, Quaternion.identity);
                    currentProjectile.transform.LookAt(target.transform.position);
                    ProjectileBehaviour projectileBehaviour = currentProjectile.GetComponent<ProjectileBehaviour>();
                    projectileBehaviour.Speed = Weapon.Speed;
                    projectileBehaviour.Damage = Weapon.Damage.Value;
                    projectileBehaviour.PierceEnemy = Weapon.PierceEnemy;
                    projectileBehaviour.stickToTarget = Weapon.StickToTarget;
                    projectileBehaviour.Target = target;
                    StartCoroutine(resetAttack());
                }
            }
        }
    }

    private Transform getTargetUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
    }

    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;
        isAttackReloading = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isMoving = true;
        }
    }
}
