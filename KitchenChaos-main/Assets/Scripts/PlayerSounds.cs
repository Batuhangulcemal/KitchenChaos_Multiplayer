using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

        if( footstepTimer < 0f)
        {
            if (player.IsWalking())
            {
                footstepTimer = footstepTimerMax;
                SoundManager.Instance.PlayFootstepSound(player.transform.position);
            }

        }
    }


}
