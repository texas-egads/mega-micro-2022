using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace noahtest{
    public class justspamit : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _spamText;
        [SerializeField] private int _spacesToWin;
        [SerializeField] private AudioClip _spaceClip;
        [SerializeField] private AudioClip _winClip;
        private AudioSource _audioSrc;
        private int _spaces;
        private bool _allowingInput;

        public void Awake(){
            _spaces = 0;
            _allowingInput = true;
            _audioSrc = Managers.AudioManager.CreateAudioSource();
            _audioSrc.clip = _spaceClip;
        }
        
        public void Update(){
            if(Input.GetButtonDown("Space") && _allowingInput){
                _audioSrc.Play();
                if(++_spaces >= _spacesToWin){
                    _allowingInput = false;
                    Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    Managers.MinigamesManager.EndCurrentMinigame(1.25f);
                    StartCoroutine(PlayWinClipAfterShortDelay());
                }
                _spamText.text = $"SPAM THE SPACEBAR.\n\n{_spaces}/{_spacesToWin}";
            }
        }

        private IEnumerator PlayWinClipAfterShortDelay(){
            yield return new WaitForSeconds(0.25f);
            _audioSrc.clip = _winClip;
            _audioSrc.Play();
        }
    }
}