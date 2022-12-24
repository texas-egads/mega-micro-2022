using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace dashi
{
    public enum CapybaraPosition
    {
        left = 0, center = 1, right = 2
    }
    public class GameManager : MonoBehaviour
    {
        private MinigamesManager _minigameManager;
        private bool _gameWon = false;
        private CapybaraPosition _currentPosition = CapybaraPosition.center;
        [SerializeField] private GameObject _capybara;
        [SerializeField][Range(0, 2f)] private float _timeForMove = 0.3f;
        [SerializeField][Range(0, 90f)] private float _tiltForMove = 0f;
        [SerializeField] private AnimationCurve _curveForMovePosition;
        [SerializeField] private AnimationCurve _curveForMoveRotation;
        [SerializeField] private Transform _leftPosition;
        [SerializeField] private Transform _centerPosition;
        [SerializeField] private Transform _rightPosition;
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private float _timeBetweenSpawns = 2f;
        [SerializeField] private GameObject _beePrefab;
        [SerializeField] private AudioClip _bgMusic;
        [SerializeField] private AudioClip _endMusic;
        private Coroutine _spawner;
        private Coroutine _timer;
        private AudioSource _bgAudioSource;
        private AudioSource _sfxAudioSource;
        private bool isMoving = false;
        void Start()
        {
            _spawner = StartCoroutine(SpawnInBees());
            _timer = StartCoroutine(Timer());
            _bgAudioSource = Managers.AudioManager.CreateAudioSource();
            _sfxAudioSource = Managers.AudioManager.CreateAudioSource();
           
            _bgAudioSource.PlayOneShot(_bgMusic,.4f);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && _currentPosition != CapybaraPosition.left)
            {
                MoveCapyBaraHelper(true);
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && _currentPosition != CapybaraPosition.right)
            {
                MoveCapyBaraHelper(false);
            }
        }
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(9f);
            StopCoroutine(_spawner);
            Managers.MinigamesManager.DeclareCurrentMinigameWon();
        }
        private void MoveCapyBaraHelper(bool left)
        {
            if(!isMoving)
            {
                isMoving = true;
                StartCoroutine(MoveCapyBara(left));
            }
        }
        private IEnumerator MoveCapyBara(bool left)
        {
            Tween moveTween = _capybara.transform.DOLocalRotate(new Vector3(0, 0, 0), .05f);
            if(left)
            {
                if(_currentPosition == CapybaraPosition.right)
                {
                    _currentPosition = CapybaraPosition.center;
                    moveTween = _capybara.transform.DOMove(_centerPosition.position, _timeForMove).SetEase(_curveForMovePosition);
                    _capybara.transform.DOLocalRotate(new Vector3(-_tiltForMove, 0, 0), _timeForMove).SetEase(_curveForMoveRotation);
                }
                else if(_currentPosition == CapybaraPosition.center)
                {
                    _currentPosition = CapybaraPosition.left;
                    moveTween = _capybara.transform.DOMove(_leftPosition.position, _timeForMove).SetEase(_curveForMovePosition);
                    _capybara.transform.DOLocalRotate(new Vector3(-_tiltForMove, 0, 0), _timeForMove).SetEase(_curveForMoveRotation);
                }
            }
            else
            {
                if(_currentPosition == CapybaraPosition.left)
                {
                    _currentPosition = CapybaraPosition.center;
                    moveTween = _capybara.transform.DOMove(_centerPosition.position, _timeForMove).SetEase(_curveForMovePosition);
                    _capybara.transform.DOLocalRotate(new Vector3(_tiltForMove, 0, 0), _timeForMove).SetEase(_curveForMoveRotation);
                }
                else if(_currentPosition == CapybaraPosition.center)
                {
                    _currentPosition = CapybaraPosition.right;
                    moveTween = _capybara.transform.DOMove(_rightPosition.position, _timeForMove).SetEase(_curveForMovePosition);
                    _capybara.transform.DOLocalRotate(new Vector3(_tiltForMove, 0, 0), _timeForMove).SetEase(_curveForMoveRotation);
                }
            }
            yield return moveTween.WaitForCompletion();
            // Tween reset = _capybara.transform.DORotate(new Vector3(-90, 0, 90), .1f);
            // yield return 
            isMoving = false;
        }
        private IEnumerator SpawnInBees()
        {
            float timePassed = 0f;
            while(timePassed <= 8f)
            {
                timePassed += Time.deltaTime;
                if(Random.Range(0f, 1f) < .5f)
                {
                    int firstPos = Random.Range(0, 3);
                    int secondPos = Random.Range(0, 3);
                    while(firstPos == secondPos)
                    {
                        secondPos = Random.Range(0, 3);
                    }
                    Vector3 spawnPos = _spawnPositions[firstPos].position;
                    GameObject newBee = Instantiate(_beePrefab, spawnPos, Quaternion.identity);
                    newBee.GetComponent<Bee>().SpawnIn(-2.35f);
                    spawnPos = _spawnPositions[secondPos].position;
                    newBee = Instantiate(_beePrefab, spawnPos, Quaternion.identity);
                    newBee.GetComponent<Bee>().SpawnIn(-2.35f);
                }
                else
                {
                    Vector3 spawnPos = _spawnPositions[Random.Range(0, 3)].position;
                    GameObject newBee = Instantiate(_beePrefab, spawnPos, Quaternion.identity);
                    newBee.GetComponent<Bee>().SpawnIn(-2.35f);
                }
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
            
            yield return new WaitForSeconds(.5f);
        }

        public void Lost()
        {
            StopCoroutine(_timer);
            StopCoroutine(_spawner);
            Managers.MinigamesManager.DeclareCurrentMinigameLost();
            _bgAudioSource.Stop();
            _sfxAudioSource.PlayOneShot(_endMusic,.5f);
            StartCoroutine(LostStop());
            Rigidbody rb = _capybara.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = false;
            isMoving = true;
            rb.AddRelativeTorque(new Vector3(20, 14, -5),ForceMode.Impulse);
            rb.AddExplosionForce(70f,rb.transform.position + Vector3.forward, 10f,1f);
        }

        private IEnumerator LostStop()
        {
            yield return new WaitForSeconds(2.5f);
            Managers.MinigamesManager.EndCurrentMinigame();
        }
    }
}

