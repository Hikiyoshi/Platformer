using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockBack = Vector2.zero;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Having somthing was hitted
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null)
        {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            
            //Hit the target
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockBack);

            if(gotHit)
                Debug.Log("Hit Damage : " + attackDamage);
        }
    }
}
