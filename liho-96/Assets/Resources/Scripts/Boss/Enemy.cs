using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar hpBar;
    public Vector3 targetPoint = new Vector3(0f, 5f, -10f);

    public float hp = 50;
    public float damage = 5;
    public float secondsBeforeNextShot = 2f;
    public float maxHidingSeconds = 3f;
    public float minHidingSeconds = 0.5f;

    private float _currentTime;
    private float _minY = 2;
    private float _maxY = 5;
    
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
        StartCoroutine(Hide());
    }

    private void Shoot()
    {
        Vector3 fromPosition = transform.position;
        Vector3 direction = targetPoint - fromPosition;
        Ray ray = new Ray(transform.position, direction);
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
    
    private IEnumerator Hide()
    {
        var positionBefore = transform.position;
        transform.position = new Vector3(positionBefore.x, _minY, positionBefore.z);
        yield return new WaitForSeconds(RandomHidingTime());
        var positionAfter = transform.position;
        transform.position = new Vector3(positionAfter.x, _maxY, positionAfter.z);
    }

    private float RandomHidingTime()
    {
        return Random.Range(minHidingSeconds, maxHidingSeconds);
    }
}