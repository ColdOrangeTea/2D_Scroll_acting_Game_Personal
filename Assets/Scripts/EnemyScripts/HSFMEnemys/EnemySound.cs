using UnityEngine;

public class EnemySound : MonoBehaviour
{
    void PlayMeleeMachine_Walk()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.MeleeMachine_Walk, gameObject.transform);
    }
    void PlayMusketeer_Shot()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.Musketeer_Shot, gameObject.transform);
    }
    void PlayMechanicHound_Bark()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.MechanicHound_Bark, gameObject.transform);
    }
    void PlayMachineBear_Walk()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.MachineBear_Walk, gameObject.transform);
    }
    void PlayMachineRabbit_Throw()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.MachineRabbit_Throw, gameObject.transform);
    }
    void PlayMachineDancer_Slash()
    {
        AudioManager.Instance.PlaySound(AudioType.tags.MachineDancer_Slash, gameObject.transform);
    }
}
