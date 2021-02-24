using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SkillTreeUI : MonoBehaviour
{
    PlayerManager pm;
    GameObject InventoryGO;
    GameObject TreeGO;
    GameObject InventorySelectionGO;
    public GameObject BoardBackground;
    public GameObject boardNode;
    public Material LineMaterial;
    public int boardMulti = 50;
    Dictionary<Board, GameObject> boardInventoryDict = new Dictionary<Board, GameObject>();
    Dictionary<Board, GameObject> boardTreeDict = new Dictionary<Board, GameObject>();
    Dictionary<Node, GameObject> nodeInventoryDict = new Dictionary<Node, GameObject>();
    Dictionary<Node, GameObject> nodeTreeDict = new Dictionary<Node, GameObject>();
    Dictionary<Node, GameObject> allNodeDict { get { return nodeTreeDict.Concat(nodeInventoryDict)
    .ToLookup(x => x.Key, x => x.Value)
    .ToDictionary(x => x.Key, g => g.First());} }
    public Dictionary<GameObject, Node> nodeGOAllDict { get { return allNodeDict.ToDictionary(p => p.Value, p => p.Key); } }
    Node SelectedNode;
    Board SelectedBoard;
    bool InventorySelection = true;
    bool dragging = false;
    Vector3 oldMousePosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        InventoryGO = transform.Find("Inventory").gameObject;
        InventorySelectionGO = InventoryGO.transform.Find("Selection").gameObject;
        TreeGO = transform.Find("Tree").gameObject;
        Test();

        pm.skillTree.skillPoints = 10;
        Board starterBoard = BoardGenerator.GenerateStarterBoard();
        pm.skillTree.takenNodes.Add(starterBoard.boardNodes.First(p => p.Cord == new Vector2(0, 0)));
        pm.skillTree.BoardPositions.Add(starterBoard, new Vector2(0, 0));
        GenerateInventoryUI();
        GenerateSkillTreeUI();
    }
    public void Update()
    {
        RectTransform rt = TreeGO.GetComponent<RectTransform>();
        if ((rt.localScale.x > 1 && Input.mouseScrollDelta.y < 0) || (rt.localScale.x < 10 && Input.mouseScrollDelta.y > 0))
            rt.localScale += new Vector3(Input.mouseScrollDelta.y, Input.mouseScrollDelta.y, 0);
        if (dragging)
        {
            if (oldMousePosition != Vector3.zero)
            {
                TreeGO.transform.position += new Vector3(Input.mousePosition.x - oldMousePosition.x, Input.mousePosition.y - oldMousePosition.y,0) * Time.deltaTime * 30f;
            }
            oldMousePosition = Input.mousePosition;
        }
        if(Input.GetKey(KeyCode.Mouse2))
        {
            dragging = true;
        }
        if(Input.GetKeyUp(KeyCode.Mouse2))
        {
            dragging = false;
            oldMousePosition = Vector3.zero;
        }
    }
    public void GenerateInventoryUI()
    {
        List<GameObject> destroyGO = new List<GameObject>();
        for (int i = 0; i < InventoryGO.transform.childCount; i++)
        {
            if (InventoryGO.transform.GetChild(i).transform.name != "Selection")
                destroyGO.Add(InventoryGO.transform.GetChild(i).gameObject);
        }
        foreach (GameObject go in destroyGO)
            Destroy(go);
        boardInventoryDict.Clear();
        nodeInventoryDict.Clear();
        //load up inventory
        for (int i = 0; i < pm.BoardsInventory.Count; i++)
        {
            Board b = pm.BoardsInventory[i];
            GameObject boardGO = GenerateBoardUI(b, true);
            boardInventoryDict.Add(b, boardGO);
            boardGO.transform.localPosition = new Vector3((i % 2 == 0 ? -60 : 60), (i * -60) + (i % 2 == 0 ? 0 : 60), 0);
        }
    }
    public void GenerateSkillTreeUI()
    {
        List<GameObject> destroyGO = new List<GameObject>();
        for (int i = 0; i < TreeGO.transform.childCount; i++)
        {
            if (TreeGO.transform.GetChild(i).transform.name != "Selection")
                destroyGO.Add(TreeGO.transform.GetChild(i).gameObject);
        }
        foreach (GameObject go in destroyGO)
            Destroy(go);
        boardTreeDict.Clear();
        nodeTreeDict.Clear();
        foreach (var pair in pm.skillTree.BoardPositions)
        {
            Board b = pair.Key;
            GameObject boardGO = GenerateBoardUI(b, false);
            //if not center, rotate it towards first connection
            if (pair.Value != new Vector2(0, 0))
            {
                BoardConnection bc = pm.skillTree.boardsConnections.First(p => p.newBoard == b);
                Vector2 newVector = pm.skillTree.BoardPositions[bc.anchorBoard] - pm.skillTree.BoardPositions[bc.newBoard];
                float angle = Vector2.Angle(bc.newNode.Cord.normalized, newVector.normalized);
                var cross = Vector3.Cross(bc.newNode.Cord.normalized, newVector.normalized).z;
                if (cross == 0)
                    cross = 1;
                boardGO.transform.eulerAngles = new Vector3(0, 0, angle * cross);
            }
            boardGO.transform.localPosition = pair.Value;
            boardTreeDict.Add(b, boardGO);
        }
        /* foreach(var connection in pm.skillTree.boardsConnections)
         {
             GameObject anchorNodeGO = nodeTreeDict[connection.anchorNode];
             GameObject newNodeGO = nodeTreeDict[connection.newNode];
             GameObject go1 = Instantiate(new GameObject(), TreeGO.transform);
             UILineRenderer lr1 = go1.AddComponent<UILineRenderer>();
             lr1.Points = new Vector2[] { anchorNodeGO.transform.position, newNodeGO.transform.position};
             lr1.material = LineMaterial;
         }*/
    }
    public GameObject GenerateBoardUI(Board b, bool inventory)
    {
        GameObject boardGO = Instantiate(new GameObject(), inventory ? InventoryGO.transform : TreeGO.transform);
        Instantiate(BoardBackground, boardGO.transform);
        foreach (NodeConnection n in b.allConnections)
        {
            GameObject go1 = Instantiate(new GameObject(), boardGO.transform);
            UILineRenderer lr1 = go1.AddComponent<UILineRenderer>();
            lr1.Points = new Vector2[] { new Vector2(boardMulti, boardMulti) + n.n1.Cord * (boardMulti * .8f), new Vector2(boardMulti, boardMulti) + n.n2.Cord * (boardMulti * .8f) };
            lr1.material = LineMaterial;
        }
        if (!inventory)
        {
            foreach (var connection in pm.skillTree.boardsConnections)
            {
                if (b.boardNodes.Contains(connection.anchorNode))
                {
                    GameObject go1 = Instantiate(new GameObject(), boardGO.transform);
                    UILineRenderer lr1 = go1.AddComponent<UILineRenderer>();
                    lr1.Points = new Vector2[] { new Vector2(boardMulti, boardMulti) + connection.anchorNode.Cord * (boardMulti * .8f), new Vector2(boardMulti, boardMulti) + connection.anchorNode.Cord * (boardMulti * 1.2f) };
                    lr1.material = LineMaterial;
                }
                if (b.boardNodes.Contains(connection.newNode))
                {
                    GameObject go1 = Instantiate(new GameObject(), boardGO.transform);
                    UILineRenderer lr1 = go1.AddComponent<UILineRenderer>();
                    lr1.Points = new Vector2[] { new Vector2(boardMulti, boardMulti) + connection.newNode.Cord * (boardMulti * .8f), new Vector2(boardMulti, boardMulti) + connection.newNode.Cord * (boardMulti * 1.2f) };
                    lr1.material = LineMaterial;
                }
            }

        }
        foreach (Node n in b.boardNodes)
        {
            if (nodeInventoryDict.ContainsKey(n))
                nodeInventoryDict.Remove(n);
            if (nodeTreeDict.ContainsKey(n))
                nodeTreeDict.Remove(n);
            GameObject nodeGO = Instantiate(boardNode, boardGO.transform);
            Button button = nodeGO.GetComponent<Button>();
            if (n.ConnectionNode)
            {
                //connection count
                GameObject text = Instantiate(new GameObject(), nodeGO.transform);
                text.transform.localPosition = new Vector3(2, 0);

                TextMeshProUGUI textObj = text.AddComponent<TextMeshProUGUI>();
                textObj.text = n.ConnectionNodeCount.ToString();
                text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10);
                text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10);
                text.transform.localPosition = new Vector3(8, 4, 0);
                textObj.alignment = TextAlignmentOptions.Center;
                textObj.color = Color.black;
                textObj.fontSize = 10;
            }
            ColorBlock cb = button.colors;
            cb.normalColor = Color.blue;
            button.colors = cb;
            if (inventory)
            {
                if (InventorySelection)
                {
                    if (n.ConnectionNode)
                        button.enabled = true;
                    button.onClick.AddListener(() => SelectInventoryNode(b, n));
                    nodeInventoryDict.Add(n, nodeGO);
                }
                else // in node selection
                {
                    button.enabled = false;
                }
            }
            else //in tree
            {
                if (!InventorySelection) //in node selection mode
                {
                    if (pm.skillTree.takenNodes.Contains(n))
                    {
                        ColorBlock cb2 = button.colors;
                        cb2.normalColor = Color.blue;
                        button.colors = cb2;
                    }
                    else
                    {
                        //check to see if you can take this node
                        if (pm.skillTree.takeableNodes.Contains(n))
                        {
                            ColorBlock cb2 = button.colors;
                            cb2.normalColor = Color.green;
                            button.colors = cb2;
                        }
                        else
                        {
                            ColorBlock cb2 = button.colors;
                            cb2.normalColor = Color.red;
                            button.colors = cb2;
                        }
                    }
                    button.enabled = true;
                }
                else
                {
                    button.enabled = false;
                }
                button.onClick.AddListener(() => SelectTreeNode(b, n));
                nodeTreeDict.Add(n, nodeGO);
            }
            nodeGO.transform.localPosition = n.Cord * (boardMulti * .8f);
        }
        return boardGO;
    }
    public void Test()
    {
        Debug.Log("test");
        //generate 5 boards
        for (int i = 0; i < 5; i++)
        {
            Board b = BoardGenerator.GenerateBoard();
            pm.BoardsInventory.Add(b);
        }

    }
    
    public void SelectTreeNode(Board b, Node n)
    {
        if (InventorySelection)
        {
            if (SelectedNode == null || SelectedBoard == null)
            {

            }
            else
            {
                pm.skillTree.boardsConnections.Add(new BoardConnection() { anchorBoard = b, newBoard = SelectedBoard, anchorNode = n, newNode = SelectedNode });
                pm.BoardsInventory.Remove(SelectedBoard);
                GameObject boardGO = boardTreeDict[b];
                //find the new board position
                var rotatedNode = RotateVector(n.Cord, boardGO.transform.eulerAngles.z);
                if (RotateVector(n.Cord, boardGO.transform.eulerAngles.z) == new Vector2(0, 1))
                {
                    pm.skillTree.BoardPositions.Add(SelectedBoard, pm.skillTree.BoardPositions[b] + new Vector2(0, boardMulti * 2));
                }
                if (RotateVector(n.Cord, boardGO.transform.eulerAngles.z) == new Vector2(0, -1))
                {
                    pm.skillTree.BoardPositions.Add(SelectedBoard, pm.skillTree.BoardPositions[b] + new Vector2(0, -boardMulti * 2));
                }
                if (RotateVector(n.Cord, boardGO.transform.eulerAngles.z) == new Vector2(1, 0))
                {
                    pm.skillTree.BoardPositions.Add(SelectedBoard, pm.skillTree.BoardPositions[b] + new Vector2(boardMulti * 2, 0));
                }
                if (RotateVector(n.Cord, boardGO.transform.eulerAngles.z) == new Vector2(-1, 0))
                {
                    pm.skillTree.BoardPositions.Add(SelectedBoard, pm.skillTree.BoardPositions[b] + new Vector2(-boardMulti * 2, 0));
                }
                SelectedBoard = null;
                SelectedNode = null;
                foreach (var pairInv in nodeInventoryDict)
                {
                    if (pairInv.Key.ConnectionNode)
                        pairInv.Value.GetComponent<Button>().enabled = true;
                }
                foreach (var pairInv in nodeTreeDict)
                {
                    pairInv.Value.GetComponent<Button>().enabled = false;
                }
                GenerateInventoryUI();
                GenerateSkillTreeUI();
            }
        }
        else
        {
            if (pm.skillTree.skillPoints > 0)
                if (pm.skillTree.takeableNodes.Contains(n))
                {
                    pm.skillTree.takenNodes.Add(n);
                    pm.skillTree.skillPoints -= 1;
                    transform.Find("SkillPointsLeft").gameObject.GetComponent<TextMeshProUGUI>().text = "Points: " + pm.skillTree.skillPoints;
                    foreach(var character in transform.Find("SkillPointsLeft").gameObject.GetComponent<TextMeshProUGUI>().textInfo.characterInfo)
                    {
                        Debug.Log(character.character + " " + character.topLeft);
                    }
                    GenerateSkillTreeUI();
                }
        }
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
    public void SelectInventoryNode(Board b, Node n)
    {
        if (InventorySelection)
        {
            if (pm.skillTree.Boards.Count == 0)
            {
                //first board
                pm.skillTree.BoardPositions.Add(b, new Vector2(0, 0));
                pm.BoardsInventory.Remove(b);
                GenerateInventoryUI();
                GenerateSkillTreeUI();
            }
            else
            {
                SelectedNode = n;
                SelectedBoard = b;
                //highlight nodes of same type on tree
                foreach (var pair in nodeTreeDict)
                {
                    Node treeNode = pair.Key;
                    if (treeNode.ConnectionNode && treeNode.ConnectionNodeCount == n.ConnectionNodeCount)
                    {
                        Button button = pair.Value.GetComponent<Button>();
                        button.enabled = true;
                        ColorBlock cb = button.colors;
                        cb.normalColor = Color.blue;
                        button.colors = cb;
                    }
                }
                //uncolor every inventory node
                foreach (var pairInv in nodeInventoryDict)
                {
                    pairInv.Value.GetComponent<Button>().enabled = false;
                }
            }
        }
        else
        {
            //this should never happen, buttons should not be pressed
            Debug.LogError("Clicked an Inventory button while in node selection mode");
        }
    }
    public void SwitchSkillTreeType()
    {
        InventorySelection = !InventorySelection;
        if (InventorySelection)
        {
            RectTransform rt = InventorySelectionGO.transform.Find("Image").GetComponent<RectTransform>();
            rt.eulerAngles = new Vector3(0, 0, 90);
            InventoryGO.transform.position = InventoryGO.transform.position + new Vector3(-245, 0, 0);
            //turn off tree nodes
            /* foreach (var pair in nodeTreeDict)
             {
                 pair.Value.GetComponent<Button>().enabled = false;
             }
             foreach (var pair in nodeInventoryDict)
             {
                 Node node = pair.Key;
                 if (node.ConnectionNode)
                     pair.Value.GetComponent<Button>().enabled = true;
                 else
                     pair.Value.GetComponent<Button>().enabled = false;
             }*/
            GenerateInventoryUI();
            GenerateSkillTreeUI();
        }
        else
        {
            //rotate image
            RectTransform rt = InventorySelectionGO.transform.Find("Image").GetComponent<RectTransform>();
            rt.eulerAngles = new Vector3(0, 0, -90);
            InventoryGO.transform.position = InventoryGO.transform.position + new Vector3(245, 0, 0);

            /*foreach (var pair in nodeTreeDict)
            {
                Button button = pair.Value.GetComponent<Button>();
                button.enabled = true;
                if(pm.skillTree.takenNodes.Contains(pair.Key))
                {
                    ColorBlock cb = button.colors;
                    cb.normalColor = Color.blue;
                    button.colors = cb;
                }
                else
                {
                    ColorBlock cb = button.colors;
                    cb.normalColor = Color.red;
                    button.colors = cb;
                }
            }*/
            /*foreach (var pair in nodeInventoryDict)
            {
                pair.Value.GetComponent<Button>().enabled = false;
            }*/
            GenerateInventoryUI();
            GenerateSkillTreeUI();
        }
    }
}
