using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsJumpCollider : MonoBehaviour {


    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayCharacter>();
        if (player)
        {
            player.Hp -= 20;
            if (player.Hp<=0)
            {
                player.Die();
            }
        }
    }

}
