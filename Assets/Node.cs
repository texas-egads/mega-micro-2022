namespace TNDStudios
{
    using System.Collection.Generic;
    public class TrolleyManager
    {
        const int MAX_LAYERS = 5;
        const int MAX_CHILDREN = 3;
        const int MIN_HOSTAGES = 1;
        const int MAX_HOSTAGES = 10;
        Random random = new Random();
        public class Node
        {
            public string m_id;
            public int m_layer;
            public List<KeyValuePair<int, Node>> m_children;

            public Node(int layer, int id)
            {
                m_layer = layer;
            }

            public bool addChild(int weight)
            {
                if(m_layer == MAX_LAYERS) return false;
                if(m_children.Count >= MAX_CHILDREN) return false;
                Node child = new Node(m_layer + 1, m_id + 1);
                m_children.Add(new KeyValuePair<int, Node>(weight, child));
                return true;
            }

            public bool addChild(int weight, Node child)
            {
                if(m_layer == MAX_LAYERS) return false;
                if(m_children.Count >= MAX_CHILDREN) return false;
                child.m_layer = m_layer + 1;
                m_children.Add(new KeyValuePair<int, Node>(weight, child));
                return true;
            }

            public bool isEndNode()
            {
                return m_children.Count == 0;
            }

            public int bestScoreFromHere()
            {
                if(isEndNode()) return 0;
                int score = -1;
                foreach(KeyValuePair<int, Node> child in m_children)
                {
                    int candidate = child.Key + child.Value.bestScoreFromHere();
                    score = (score == -1 || score > candidate) ? candidate : score;
                }
                return score;
            }
        }

        public Node headNode = new Node(0, 0);
        
        public void generateGraph(int numLayers)
        {
            Queue<Node> generationQueue = {headNode};
            while(generationQueue.Count > 0 && generationQueue.Peek().m_layer != numLayers)
            {
                Node parent = generationQueue.Dequeue();
                int index = random.Next(3);
                int[] numChildren = {0, 2, 3};
                for(int i = 0; i < numChildren[index]; i++)
                {
                    parent.addChild(random.Next(MIN_HOSTAGES, MAX_HOSTAGES));
                }
                foreach(KeyValuePair<int, Node> child in parent.m_children)
                {
                    generationQueue.enqueue(child.Value);
                }
            }
        }

        public void toString()
        {
            Queue<Node> generationQueue = {headNode};
            while(generationQueue.Count > 0)
            {
                Node node = generationQueue.Dequeue();
                Debug.Log("Node " + node.m_id);
                Debug.Log("Children: ");
                foreach(KeyValuePair<int, Node> child in node.m_children)
                {
                    generationQueue.enqueue(child.Value);
                    Debug.Log(child.Value.m_id + " with weight " + child.Key);
                }

            }
        }
    }
}