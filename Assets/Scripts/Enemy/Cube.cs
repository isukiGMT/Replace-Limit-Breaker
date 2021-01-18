using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Introduction();
        print(Damage);
    }

    // Update is called once per frame
    void Update()
    {
        Working();
        if (Active == true)
        {
            if (Distance >= 3)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (CanAttack == true)
                {
                    LastTimeAttack = Time.time;
                    CanAttack = false;
                    PlayerScript.TakeDamage(Damage);
                }
                StopMoving();
            }
        }
    }
}
