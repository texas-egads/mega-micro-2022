using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TNDStudios { 
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TrolleyManager tm = new TrolleyManager();
            tm.GenerateGraph(3);
            tm.PrintStringRepresentation();
            Debug.Log("Best score: " + tm.headNode.BestScoreFromHere());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
