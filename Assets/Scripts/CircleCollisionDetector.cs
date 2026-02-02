using UnityEngine;

public class CircleCollisionDetector : MonoBehaviour
{
    public float radius = 0.4f;
    public Vector2 offset = new Vector2(0, -0.7f);
    public LayerMask mask = ~0;
    public bool isColliding;
    bool wasColliding;

    Collider2D ownCollider; // Para excluir nuestro propio collider

    public bool startedCollidingThisFrame
    {
        get { return isColliding && !wasColliding; }
    }
    public bool stoppedCollidingThisFrame
    {
        get { return !isColliding && wasColliding; }
    }

    private void Start()
    {
        // Guardar referencia a nuestro propio collider
        ownCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        wasColliding = isColliding;
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position + (Vector3)offset, radius, mask);

        // Verificar si hay colisiones que NO sean nuestro propio collider
        isColliding = false;
        foreach (Collider2D col in collisions)
        {
            if (col != ownCollider)
            {
                isColliding = true;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isColliding)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position + (Vector3)offset, radius);
    }
}