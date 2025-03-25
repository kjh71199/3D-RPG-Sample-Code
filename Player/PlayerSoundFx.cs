using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFx : SoundFx
{
    public enum PLAYERSOUND { ATTACK, DASH, HIT, POTION, DIE }

    [SerializeField] AudioClip[] playerAudios;
}
