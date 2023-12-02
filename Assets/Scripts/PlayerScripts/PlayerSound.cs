using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void PlayFootStepSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Walk, gameObject.transform);
    }
    public void PlayJumpSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Jump, gameObject.transform);
    }
    // void PlayJumpOverSound()
    // {
    //     AudioManager.Instance.PlaySound(AudioType.tags.Player_JumpOver, gameObject.transform);
    // }
    public void PlayPunchSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Punch00, gameObject.transform);
    }
    public void PlayThunderSound()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Player_Thunder, gameObject.transform);
    }
}
