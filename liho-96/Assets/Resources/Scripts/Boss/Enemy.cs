using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemiesHealthBar hpBar;

    public float hp = 50;
    
    public void Hit(float damage)
    {
        var hpDamage = damage;
        
        hp -= damage;

        if (hp <= 0)
        {
            hpDamage = damage + hp;
            Destroy(gameObject);
        }
        
        hpBar.Damage(hpDamage);
    }
}