using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar hpBar;

    public float hp = 50;
    public float damage = 5;
    public float secondsBeforeNextShot = 2f;

    private float _currentTime;
    
    private void Update()
    {
        _currentTime += Time.deltaTime;
 
        if (_currentTime >= secondsBeforeNextShot) {
            _currentTime -= secondsBeforeNextShot;
            Shoot();
        }
    }
    
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

    private void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var courier = hit.collider.gameObject.GetComponent<Courier>();
            if (courier != null)
            {
                courier.Hit(damage);
            }
        }
    }
}