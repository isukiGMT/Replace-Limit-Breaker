using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public PlayerSet.Player _player;
    public GameObject player;
    private float Damage;
    public float DamageBonus;
    private bool Attacking;
    private BoxCollider Hb;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _player = player.GetComponent<PlayerSet.Player>();
        Hb = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Damage = _player.BaseDamage + DamageBonus;
        ///Hb.enabled = _player.Attacking;
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") && _player.Attacking == true)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(_player.BaseDamage + DamageBonus);
        }
    }
}
