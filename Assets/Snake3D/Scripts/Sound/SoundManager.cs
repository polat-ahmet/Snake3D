using UnityEngine;

namespace Snake3D.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] public AudioSource SoundSource;
        
        public void SetClip(AudioClip clip)
        {
            SoundSource.clip = clip;
        }
        
        public void PlaySound(AudioClip clip)
        {
            SetClip(clip);
            SoundSource.PlayOneShot(clip);
        }
    }
}