using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TNDStudios { 
    public class GameManager : MonoBehaviour
    {
        public GameObject nodePrefab;
        public GameObject linePrefab;
        public GameObject player;
        public GameObject wizardPrefab; 
        public List<GameObject> destroyAllObjects = new List<GameObject>();
        public TrolleyManager.Node headNode;
        public TrolleyManager.Node currentNode;
        public int selectedPath = -1; 
        public List<TrolleyManager.Node>[] nodes = new List<TrolleyManager.Node>[3] { new List<TrolleyManager.Node>(), new List<TrolleyManager.Node>(), new List<TrolleyManager.Node>() };
        public float horizontalLength = -1.5f;
        public Vector3 startPos = new Vector3(-6.5f, 0.8f, -3.5f);
        public List<TrolleyManager.Node> currentChildren = new List<TrolleyManager.Node>();
        public int onDecision = 0;
        public int wizardsKilled = 0; 
        
        public AudioClip backgroundClip;
        public AudioSource backgroundSource = null;

        public AudioClip whistleClip;
        public int bestScore = 0;

        private TrolleyManager tm;
        // Start is called before the first frame update
        void Start()
        {
            tm = new TrolleyManager();
            tm.GenerateGraph(3);
            tm.AssignYValues();
            tm.AssignPositions();
            headNode = tm.headNode;
            bestScore = tm.headNode.BestScoreFromHere();
            Debug.Log("Best score: " + bestScore);
            headNode.numHostages = 0;

            try
            {

                backgroundSource = Managers.AudioManager.CreateAudioSource();
                if (backgroundSource != null)
                {
                    backgroundSource.clip = backgroundClip;
                    backgroundSource.Play();
                }

                AudioSource whistleSource = Managers.AudioManager.CreateAudioSource();
                if (whistleSource != null)
                {
                    whistleSource.clip = whistleClip;
                    whistleSource.Play();
                }
            }
            catch { }
            

            foreach (TrolleyManager.Node n in tm.allNodes)
            {
                Vector3 wizardPlacementStartingPos = n.m_position + new Vector3(horizontalLength * 0.8f, 0, -3.5f);

                foreach (KeyValuePair<int, TrolleyManager.Node> c in n.m_children)
                {
                    TrolleyManager.Node child = c.Value;
                    // diagonal transition
                    GameObject l = Instantiate(linePrefab, new Vector3(0,0,0), Quaternion.identity);
                    LineRenderer lr = l.GetComponent<LineRenderer>();
                    Vector3[] positions = new Vector3[2] { n.m_position, child.m_position + new Vector3(horizontalLength, 0, 0) };
                    lr.SetPositions(positions);
                    n.m_lineRenderers.Add(lr);
                    destroyAllObjects.Add(l);
                }

                // horizontal bit before
                GameObject l2 = Instantiate(linePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                LineRenderer lr2 = l2.GetComponent<LineRenderer>();
                Vector3[] positions2 = new Vector3[2] { n.m_position, n.m_position + new Vector3(horizontalLength, 0, 0) };
                lr2.SetPositions(positions2);
                n.horizontalLR = lr2;
                destroyAllObjects.Add(l2);

                for (int i = 0; i < n.numHostages; i++)
                {
                    GameObject wiz = Instantiate(wizardPrefab, wizardPlacementStartingPos, Quaternion.identity);
                    wizardPlacementStartingPos -= new Vector3(horizontalLength * 0.2f, 0, 0);
                    Wizard w = wiz.GetComponent<Wizard>();
                    w.gameManager = this;
                    destroyAllObjects.Add(wiz);
                }

                if (n.m_layer > 0) nodes[n.m_layer - 1].Add(n);
            }

            player.transform.position = startPos;
            currentNode = headNode;
            selectedPath = headNode.m_children.Count - 1;
        }

        // Update is called once per frame
        void Update()
        {
            player.transform.position += new Vector3(1.25f, 0, 0) * Time.deltaTime;

            if (onDecision == 3) {
                if (player.transform.position.x > 6)
                {
                    if (wizardsKilled == bestScore)
                    {
                        Managers.MinigamesManager.DeclareCurrentMinigameWon();
                    }
                    else
                    {
                        Managers.MinigamesManager.DeclareCurrentMinigameLost();
                    }
                    Managers.MinigamesManager.EndCurrentMinigame();
                }
                
                return;
            }

            if (
                (onDecision == 0 && player.transform.position.x > -4.5f) || 
                (onDecision == 1 && player.transform.position.x > -1) ||
                (onDecision == 2 && player.transform.position.x > 2.5f)
                )
            {
                //Debug.Log(player.transform.position.x);
                //Debug.Log(onDecision);
                player.transform.position += new Vector3(0, (currentNode.m_children[selectedPath].Value.m_position.y - currentNode.m_position.y) * 0.65f, 0) * Time.deltaTime;

                if (player.transform.position.x > 3.5f * onDecision - 2.5f)
                {
                    if (onDecision == 2)
                    {
                        Debug.Log("ondecision is 3");
                        onDecision = 3;
                        return;
                    }
                    Debug.Log("MOVING TO NEXT NODE");
                    currentNode.m_lineRenderers[selectedPath].startColor = Color.gray;
                    currentNode.m_lineRenderers[selectedPath].endColor = Color.gray;
                    currentNode = currentNode.m_children[selectedPath].Value;
                    currentChildren.Clear();
                    foreach (KeyValuePair<int, TrolleyManager.Node> n in currentNode.m_children)
                    {
                        currentChildren.Add(n.Value);
                    }
                    selectedPath = 0;
                    onDecision++;
                }
            
            } else
            {
                currentNode.m_lineRenderers[selectedPath].startColor = Color.green;
                currentNode.m_lineRenderers[selectedPath].endColor = Color.green;
                int previousPath = selectedPath;
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    previousPath = selectedPath;
                    selectedPath--;
                }
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    previousPath = selectedPath;
                    selectedPath++;
                }
                player.transform.position = new Vector3(player.transform.position.x, currentNode.m_position.y + 0.375f, player.transform.position.z);
                if (selectedPath < 0) selectedPath = currentNode.m_children.Count - 1;
                selectedPath %= currentNode.m_children.Count;
                if (previousPath != selectedPath)
                {
                    currentNode.m_lineRenderers[previousPath].startColor = Color.white;
                    currentNode.m_lineRenderers[previousPath].endColor = Color.white;
                    //Debug.Log("selectedPath set to " + selectedPath);
                }
            }
        }


        private void OnDestroy()
        {
            Debug.Log("DESTORYED");
            foreach(GameObject g in destroyAllObjects)
            {
                Destroy(g);
            }
            
            tm = new TrolleyManager();
            

            
           if (backgroundSource != null)
            {
                backgroundSource.Stop();
            }
        }
    }

}
