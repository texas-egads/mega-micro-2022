using System.Collections.Generic;
using UnityEngine;

namespace TNDStudios
{
    public class TrolleyManager
    {
        const int MAX_LAYERS = 3;
        const int MAX_CHILDREN = 3;
        const int MIN_HOSTAGES = 1;
        const int MAX_HOSTAGES = 10;
        public int currentId = 0;
        public List<Node> allNodes = new List<Node>();
        public class Node
        {
            public int m_id;
            public int m_layer;
            public int m_y;
            public List<KeyValuePair<int, Node>> m_children;
            private TrolleyManager trolleyManager;

            public Node(int layer, int id, TrolleyManager tm)
            {
                m_layer = layer;
                m_id = id;
                m_y = 0;
                m_children = new List<KeyValuePair<int, Node>>();
                trolleyManager = tm;
            }

            public bool AddChild(int weight)
            {
                if(m_layer == MAX_LAYERS) return false;
                if(m_children.Count >= MAX_CHILDREN) return false;
                Node child = new Node(m_layer + 1, trolleyManager.currentId++, trolleyManager);
                trolleyManager.allNodes.Add(child);
                m_children.Add(new KeyValuePair<int, Node>(weight, child));
                return true;
            }

            public bool AddChild(int weight, Node child)
            {
                if(m_layer == MAX_LAYERS) return false;
                if(m_children.Count >= MAX_CHILDREN) return false;
                child.m_layer = m_layer + 1;
                m_children.Add(new KeyValuePair<int, Node>(weight, child));
                return true;
            }

            public bool IsEndNode()
            {
                return m_children.Count == 0;
            }

            public int BestScoreFromHere()
            {
                if(IsEndNode()) return 0;
                int score = -1;
                foreach(KeyValuePair<int, Node> child in m_children)
                {
                    int candidate = child.Key + child.Value.BestScoreFromHere();
                    score = (score == -1 || score > candidate) ? candidate : score;
                }
                return score;
            }
        }

        public Node headNode;

        public TrolleyManager()
        {
            headNode = new Node(0, currentId++, this);
            allNodes.Add(headNode);
        }

        public void GenerateGraph(int numLayers)
        {
            Queue<Node> generationQueue = new Queue<Node>();
            generationQueue.Enqueue(headNode);
            
            while (generationQueue.Count > 0 && generationQueue.Peek().m_layer != numLayers)
            {
                Node parent = generationQueue.Dequeue();
                int index = Random.Range(0, 3);
                int[] numChildren = {2, 2, 3};
                for(int i = 0; i < numChildren[index]; i++)
                {
                    parent.AddChild(Random.Range(MIN_HOSTAGES, MAX_HOSTAGES + 1));
                }
                foreach(KeyValuePair<int, Node> child in parent.m_children)
                {
                    generationQueue.Enqueue(child.Value);
                }
            }
        }

        public void PrintStringRepresentation()
        {
            Queue<Node> generationQueue = new Queue<Node>();
            generationQueue.Enqueue(headNode);
            while (generationQueue.Count > 0)
            {
                Node node = generationQueue.Dequeue();
                Debug.Log("Node " + node.m_id + " at y-level = " + node.m_y);
                Debug.Log("Children: ");
                foreach(KeyValuePair<int, Node> child in node.m_children)
                {
                    generationQueue.Enqueue(child.Value);
                    Debug.Log(child.Value.m_id + " with weight " + child.Key);
                }
            }
        }
        public void AssignYValues()
        {
            Queue<Node> generationQueue = new Queue<Node>();
            generationQueue.Enqueue(headNode);
            
            while (generationQueue.Count > 0)
            {
                Node parent = generationQueue.Dequeue();
                if(parent.m_children.Count == MAX_CHILDREN)
                {
                    parent.m_children[0].Value.m_y = (int)(parent.m_y + Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                    parent.m_children[1].Value.m_y = parent.m_y;
                    parent.m_children[2].Value.m_y = (int)(parent.m_y - Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                }
                if(parent.m_children.Count == MAX_CHILDREN - 1)
                {
                    int index = Random.Range(0, 3);
                    if(index == 0)
                    {
                        parent.m_children[0].Value.m_y = (int)(parent.m_y + Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                        parent.m_children[1].Value.m_y = parent.m_y;
                    }
                    else if(index == 1)
                    {
                        parent.m_children[0].Value.m_y = (int)(parent.m_y + Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                        parent.m_children[1].Value.m_y = (int)(parent.m_y - Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                    }
                    else if(index == 2)
                    {
                        parent.m_children[0].Value.m_y = parent.m_y;
                        parent.m_children[1].Value.m_y = (int)(parent.m_y - Mathf.Pow(MAX_CHILDREN, MAX_LAYERS - parent.m_layer));
                    }
                }
                foreach(KeyValuePair<int, Node> child in parent.m_children)
                {
                    generationQueue.Enqueue(child.Value);
                }
            }
        }

    }
}