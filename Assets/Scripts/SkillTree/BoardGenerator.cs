using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardGenerator
{

    public static Board GenerateBoard()
    {

        List<NodeConnection> nodeConnections = new List<NodeConnection>();
        List<Node> nodes = new List<Node>();
        Board board = new Board();
        List<Node> boardNodes = new List<Node>();
        List<NodeConnection> allConnections = new List<NodeConnection>();
        List<Node> baseNodes = new List<Node>();
        int nodeCount = UnityEngine.Random.Range(2, 5);//5
        Debug.Log(nodeCount);
        for (int i = 0; i < nodeCount; i++)
        {
            Node newNode = new Node() { Name = "Test", ConnectionNode = true, ConnectionNodeCount = UnityEngine.Random.Range(1, 5) };
            newNode.Cord = FindConnectionNodeXY(i, nodeCount);
            bool corner = UnityEngine.Random.Range(0, 2) == 1;
            boardNodes.Add(newNode);
            baseNodes.Add(newNode);
            if (corner)
            {
                Node newNodeCorner = new Node() { Name = "Test", ConnectionNode = false };
                newNodeCorner.Cord = FindCornerNodeXY(i, nodeCount);
                baseNodes.Add(newNodeCorner);
                boardNodes.Add(newNodeCorner);
            }

        }
        foreach (Node n in baseNodes)
            n.values.Add(new NodeValues() { effect = NodeEffects.gw, value = 10, text = "10gW" });
        for (int i = 0; i < baseNodes.Count; i++)
        {
            List<Node> treenodes = new List<Node>();
            bool travel = UnityEngine.Random.Range(0, 2) == 1;
            if (nodeCount == 2)
                travel = false;
            float selection = UnityEngine.Random.Range(0, 7);
            Node startConn = new Node();
            Node endConn = new Node();
            if (travel)
            {
                treenodes = TravelNode(allConnections, out Node start, out Node end);
                startConn = start;
                endConn = end;
            }
            else
            {
                if (selection == 0)
                {
                    treenodes = ThreeNode(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 1)
                {
                    treenodes = FourDiamond(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 2)
                {
                    treenodes = FourDiamondSingleConnection(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 3)
                {
                    treenodes = FourDiamondDoubleConnection(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 4)
                {
                    treenodes = FourDiamondTripleConnection(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 5)
                {
                    treenodes = DoubleThree(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
                if (selection == 6)
                {
                    treenodes = FiveNodeSingleConnection(allConnections, out Node start, out Node end);
                    startConn = start;
                    endConn = end;
                }
            }
            if (i != baseNodes.Count - 1)
                treenodes = ModifyNodes(treenodes, baseNodes[i + 1].Cord - baseNodes[i].Cord, baseNodes[i].Cord, (baseNodes.Count == 3 && i == 2));
            else
                treenodes = ModifyNodes(treenodes, baseNodes[0].Cord - baseNodes[i].Cord, baseNodes[i].Cord, (baseNodes.Count == 3 && i == 2));
            allConnections.Add(new NodeConnection() { n1 = baseNodes[i], n2 = startConn });

            if (i != baseNodes.Count - 1)
                allConnections.Add(new NodeConnection() { n1 = baseNodes[i + 1], n2 = endConn });
            else
                allConnections.Add(new NodeConnection() { n1 = baseNodes[0], n2 = endConn });
            nodes.AddRange(treenodes);
            if (baseNodes.Count == 2)
                break;
        }
        if (nodeCount == 4)
        {
            //middle connection
            Node middleNode = new Node() { Cord = new Vector2(0, 0) };
            middleNode.values.Add(new NodeValues() { effect = NodeEffects.gw, value = 10, text = "10gW" });
            nodes.Add(middleNode);
            int getRanNum1 = UnityEngine.Random.Range(0, 4);
            int getRanNum2 = UnityEngine.Random.Range(0, 4);
            int getRanNum3 = UnityEngine.Random.Range(0, 4);
            while (getRanNum2 == getRanNum1)
                getRanNum2 = UnityEngine.Random.Range(0, 4);
            while (getRanNum3 == getRanNum1 || getRanNum3 == getRanNum2)
                getRanNum3 = UnityEngine.Random.Range(0, 4);
            List<Node> connectionNodes = baseNodes.Where(p => p.ConnectionNode == true).ToList();
            allConnections.Add(new NodeConnection() { n1 = connectionNodes[getRanNum1], n2 = middleNode });
            allConnections.Add(new NodeConnection() { n1 = connectionNodes[getRanNum2], n2 = middleNode });
            allConnections.Add(new NodeConnection() { n1 = connectionNodes[getRanNum3], n2 = middleNode });
        }
        nodes.AddRange(baseNodes);
        nodeConnections.AddRange(allConnections);
        board.boardNodes = nodes;
        board.allConnections = nodeConnections;
        return board;
    }
    public static Board GenerateStarterBoard()
    {
        List<NodeConnection> nodeConnections = new List<NodeConnection>();
        List<Node> nodes = new List<Node>();
        Board board = new Board();
        Node n1 = new Node() { ConnectionNode = true, ConnectionNodeCount = 1, Cord = new Vector2(0, 1) };
        Node n2 = new Node() { ConnectionNode = true, ConnectionNodeCount = 2, Cord = new Vector2(1, 0) };
        Node n3 = new Node() { ConnectionNode = true, ConnectionNodeCount = 3, Cord = new Vector2(0, -1) };
        Node n4 = new Node() { ConnectionNode = true, ConnectionNodeCount = 4, Cord = new Vector2(-1, 0) };
        Node n5 = new Node() { ConnectionNode = false, Cord = new Vector2(0, 0) };
        nodes.AddRange(new List<Node>() { n1, n2, n3, n4, n5 });
        NodeConnection nc1 = new NodeConnection() { n1 = n1, n2 = n2 };
        NodeConnection nc2 = new NodeConnection() { n1 = n1, n2 = n5 };

        NodeConnection nc21 = new NodeConnection() { n1 = n2, n2 = n3 };
        NodeConnection nc22 = new NodeConnection() { n1 = n2, n2 = n5 };

        NodeConnection nc31 = new NodeConnection() { n1 = n3, n2 = n4 };
        NodeConnection nc32 = new NodeConnection() { n1 = n3, n2 = n5 };

        NodeConnection nc41 = new NodeConnection() { n1 = n4, n2 = n1 };
        NodeConnection nc42 = new NodeConnection() { n1 = n4, n2 = n5 };
        nodeConnections.AddRange(new List<NodeConnection>() { nc1, nc2, nc21, nc22, nc31, nc32, nc41, nc42 });
        board.boardNodes = nodes;
        board.allConnections = nodeConnections;
        foreach (Node n in board.boardNodes)
            n.values.Add(new NodeValues() { effect = NodeEffects.gw, value = 10, text = "10gW" });
        return board;
    }
    public static Vector2 FindConnectionNodeXY(int i, int nodeCount)
    {
        if (i == 0)
            return new Vector2() { x = 0, y = 1 };
        if (i == 1 && nodeCount == 2)
            return new Vector2() { x = 0, y = -1 };
        if (i == 1)
            return new Vector2() { x = 1, y = 0 };
        if (i == 2)
            return new Vector2() { x = 0, y = -1 };
        else
            return new Vector2() { x = -1, y = 0 };

    }
    public static Vector2 FindCornerNodeXY(int i, int nodeCount)
    {
        if (i == 0)
            return new Vector2() { x = 1, y = 1 };
        if (i == 1 && nodeCount == 2)
            return new Vector2() { x = -1, y = -1 };
        if (i == 1)
            return new Vector2() { x = 1, y = -1 };
        if (i == 2)
            return new Vector2() { x = -1, y = -1 };
        else
            return new Vector2() { x = -1, y = 1 };
    }
    public static List<Node> ModifyNodes(List<Node> nodesToModify, Vector2 newVector, Vector2 start, bool threeway)
    {
        foreach (Node node in nodesToModify)
        {
            float magDiff = newVector.magnitude;
            float angle = Vector2.Angle(Vector2.up, newVector);
            var cross = Vector3.Cross(Vector2.up, newVector).z;
            if (cross == 0)
                cross = 1;
            Vector2 v2 = Quaternion.AngleAxis(cross * angle, Vector3.forward) * node.Cord * magDiff;

            node.Cord = start + v2;
            if (threeway)
                node.Cord += new Vector2(-0.5f, 0);
        }
        return nodesToModify;
    }
    public static List<Node> TravelNode(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.5f } };
        nodes.Add(n1);
        startConn = n1;
        endConn = n1;
        return nodes;
    }
    public static List<Node> ThreeNode(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0.2f, y = 0.33f } };
        Node n2 = new Node() { Cord = new Vector2() { x = -0.3f, y = 0.5f }, notable = true };
        Node n3 = new Node() { Cord = new Vector2() { x = 0.2f, y = 0.66f } };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n2 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n3 });
        startConn = n1;
        endConn = n3;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
    public static List<Node> FourDiamond(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.33f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f } };
        Node n3 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f } };
        Node n4 = new Node() { Cord = new Vector2() { x = 0f, y = 0.66f } };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n2 });
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n4 });
        allConnections.Add(new NodeConnection() { n1 = n3, n2 = n4 });
        startConn = n1;
        endConn = n4;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
    public static List<Node> FourDiamondSingleConnection(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.33f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f } };
        Node n3 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f } };
        Node n4 = new Node() { Cord = new Vector2() { x = 0f, y = 0.66f }, notable = true };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n1 });
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n3, n2 = n4 });
        startConn = n2;
        endConn = n2;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
    public static List<Node> FourDiamondDoubleConnection(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.33f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f } };
        Node n3 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f } };
        Node n4 = new Node() { Cord = new Vector2() { x = 0f, y = 0.66f }, notable = true };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n2 });
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n4 });
        startConn = n2;
        endConn = n4;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
    public static List<Node> FourDiamondTripleConnection(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.33f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f } };
        Node n3 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f } };
        Node n4 = new Node() { Cord = new Vector2() { x = 0f, y = 0.66f }, notable = true };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n1 });
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n3, n2 = n4 });
        startConn = n1;
        endConn = n4;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
    public static List<Node> DoubleThree(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = 0, y = 0.2f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.35f } };
        Node n3 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f }, notable = true };
        Node n4 = new Node() { Cord = new Vector2() { x = .175f, y = 0.65f } };

        Node n5 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.35f } };
        Node n6 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f }, notable = true };
        Node n7 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.65f } };

        Node n8 = new Node() { Cord = new Vector2() { x = 0f, y = 0.8f } };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        nodes.Add(n5);
        nodes.Add(n6);
        nodes.Add(n7);
        nodes.Add(n8);

        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n2 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n3, n2 = n4 });
        allConnections.Add(new NodeConnection() { n1 = n4, n2 = n8 });

        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n5 });
        allConnections.Add(new NodeConnection() { n1 = n5, n2 = n6 });
        allConnections.Add(new NodeConnection() { n1 = n6, n2 = n7 });
        allConnections.Add(new NodeConnection() { n1 = n7, n2 = n8 });
        startConn = n1;
        endConn = n8;
        return nodes;
    }
    public static List<Node> FiveNodeSingleConnection(List<NodeConnection> allConnections, out Node startConn, out Node endConn)
    {
        List<Node> nodes = new List<Node>();
        Node n1 = new Node() { Cord = new Vector2() { x = .3f, y = 0.5f } };
        Node n2 = new Node() { Cord = new Vector2() { x = .175f, y = 0.5f } };
        Node n3 = new Node() { Cord = new Vector2() { x = 0f, y = 0.67f } };
        Node n4 = new Node() { Cord = new Vector2() { x = 0.085f, y = 0.33f } };
        Node n5 = new Node() { Cord = new Vector2() { x = -0.085f, y = 0.33f } };
        Node n6 = new Node() { Cord = new Vector2() { x = -.175f, y = 0.5f }, notable = true };
        nodes.Add(n1);
        nodes.Add(n2);
        nodes.Add(n3);
        nodes.Add(n4);
        nodes.Add(n5);
        nodes.Add(n6);
        allConnections.Add(new NodeConnection() { n1 = n1, n2 = n2 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n3 });
        allConnections.Add(new NodeConnection() { n1 = n3, n2 = n6 });
        allConnections.Add(new NodeConnection() { n1 = n2, n2 = n4 });
        allConnections.Add(new NodeConnection() { n1 = n4, n2 = n5 });
        allConnections.Add(new NodeConnection() { n1 = n5, n2 = n6 });
        startConn = n1;
        endConn = n1;
        ClusterGenerator.GenerateCluster(0, nodes);
        return nodes;
    }
}
public class Node
{
    public string Name;
    public bool ConnectionNode = false;
    public int ConnectionNodeCount = 0;
    public Vector2 Cord;
    public bool notable = false;
    public List<NodeValues> values = new List<NodeValues>();
}
public class NodeValues
{
    public NodeEffects effect;
    public decimal value;
    public string text;
    public decimal minValue;//for rolling value
    public decimal maxValue;//for rolling value

}

public class NodeConnection
{
    public Node n1;
    public Node n2;
}
public class Board
{
    public List<Node> boardNodes = new List<Node>();
    public List<NodeConnection> allConnections = new List<NodeConnection>();
}

