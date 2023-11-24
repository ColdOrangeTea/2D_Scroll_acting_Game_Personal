using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    void PlayFootStepSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Walk, gameObject.transform);
    }
    void PlayJumpSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Jump, gameObject.transform);
    }
    // void PlayJumpOverSound()
    // {
    //     AudioManager.Instance.PlaySound(AudioType.tags.Player_JumpOver, gameObject.transform);
    // }
    void PlayPunchSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Punch00, gameObject.transform);
    }
}
