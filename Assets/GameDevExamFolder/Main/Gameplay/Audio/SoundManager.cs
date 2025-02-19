using UnityEngine;
using NF.Main.Core;

namespace NF.Main.Gameplay.Audio
{
    public class SoundManager : SingletonPersistent<SoundManager>
    {
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioClip _backgroundMusic;
        private bool _isMuted;

        protected override void Awake()
        {
            base.Awake();
            if (_bgmSource == null)
            {
                _bgmSource = gameObject.AddComponent<AudioSource>();
                _bgmSource.loop = true;
            }
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            PlayBackgroundMusic();
        }

        private void PlayBackgroundMusic()
        {
            if (_backgroundMusic == null)
            {
                Debug.LogError("No background music assigned!");
                return;
            }

            _bgmSource.clip = _backgroundMusic;
            _bgmSource.Play();
        }

        public void ToggleMute()
        {
            _isMuted = !_isMuted;
            _bgmSource.mute = _isMuted;
        }

        public bool IsMuted()
        {
            return _isMuted;
        }
    }
}
