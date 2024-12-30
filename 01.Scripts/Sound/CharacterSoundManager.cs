using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] biteSFX, jumpSFX, moveSFX, landSFX;


    public void PlayBiteSFX()
    {
        EazySoundManager.PlaySound(biteSFX[Random.Range(0, biteSFX.Length)]);
    }
    public void PlayWalkSFX()
    {
        EazySoundManager.PlaySound(moveSFX[Random.Range(0, moveSFX.Length)]);
    }

    public void PlayJumpSFX()
    {
        EazySoundManager.PlaySound(jumpSFX[Random.Range(0, jumpSFX.Length)]);

    }

    public void PlayLandSFX()
    {
        EazySoundManager.PlaySound(landSFX[Random.Range(0, landSFX.Length)]);

    }
}