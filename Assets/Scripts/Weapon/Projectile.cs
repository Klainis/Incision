using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //damage

    [Header("Rigidbody")]
    [SerializeField] private Rigidbody _projectileRigidbody;


    public Rigidbody Rigidbody { get { return _projectileRigidbody; } }

    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time > 10)
            Destroy(gameObject);
    }
}
