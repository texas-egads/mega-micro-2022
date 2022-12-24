using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace dashi
{
    public class Bee : MonoBehaviour
    {
        [SerializeField] private float _beeSpeed;
        [SerializeField] private float _spawnInTime;
        [SerializeField] private AnimationCurve _spawnInCurve;
        [SerializeField] private Rigidbody _rb;
        void Start()
        {
            _rb.velocity = transform.forward * -_beeSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void SpawnIn(float targetHeight)
        {
            transform.DOMoveY(targetHeight, _spawnInTime).SetEase(_spawnInCurve);
        }
    }
}
