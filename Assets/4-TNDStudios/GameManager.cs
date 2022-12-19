using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TNDStudios { 
    public class GameManager : MonoBehaviour
    {
        public GameObject nodePrefab;
        public List<GameObject> myNodes = new List<GameObject>();
        public List<PositionedNode> myPositionedNodes = new List<PositionedNode>();
        // Start is called before the first frame update
        void Start()
        {
            TrolleyManager tm = new TrolleyManager();
            tm.GenerateGraph(3);
            tm.AssignYValues();
            tm.PrintStringRepresentation();
            Debug.Log("Best score: " + tm.headNode.BestScoreFromHere());
            foreach (TrolleyManager.Node n in tm.allNodes)
            {
                Vector3 pos = new Vector3(n.m_layer * 4 - 6f, n.m_y / 3.0f * 7 / 27 + 1, 0);
                myPositionedNodes.Add(new PositionedNode(n, pos));
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
    public struct PositionedNode
    {
        public Vector3 position;
        public TrolleyManager.Node node;

        public PositionedNode(TrolleyManager.Node n, Vector3 p)
        {
            node = n;
            position = p;
        }
    }
}
