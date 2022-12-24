using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace dashi
{
    public class GameManager : MonoBehaviour
    {
        private MinigamesManager _minigameManager;
        private bool _gameWon = false;
        // Start is called before the first frame update
        void Start()
        {
            _minigameManager = Managers.__instance.minigamesManager;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

