using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRate;
    [SerializeField] private float damageAmmount;

    private float lastAttacked = 0f;

    private void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < attackDistance && Time.time > lastAttacked + attackRate)
        {
            target.GetComponent<IDamageable>().TakeDamage(damageAmmount);
            lastAttacked = Time.time;
        }
    }
}
