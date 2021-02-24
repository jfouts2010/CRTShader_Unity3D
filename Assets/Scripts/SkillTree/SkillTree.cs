using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTree
{
    public Dictionary<Board, Vector2> BoardPositions = new Dictionary<Board, Vector2>();
    public List<Board> Boards { get { return BoardPositions.Keys.ToList(); } }
    public List<BoardConnection> boardsConnections = new List<BoardConnection>();
    public List<Node> skillTreeNodes { get { return Boards.SelectMany(p => p.boardNodes).ToList(); } }
    public List<NodeConnection> skillTreeConnections { get { return (Boards.SelectMany(p => p.allConnections).ToList().Concat(boardsConnections.Select(p2 => new NodeConnection() { n1 = p2.anchorNode, n2 = p2.newNode }).ToList())).ToList(); } }
    public List<Node> takenNodes = new List<Node>();
    public List<Node> takeableNodes { get { return skillTreeConnections.Where(p => takenNodes.Contains(p.n1)).Select(p => p.n2).ToList().Concat(skillTreeConnections.Where(p => takenNodes.Contains(p.n2)).Select(p => p.n1).ToList()).Where(p =>!takenNodes.Contains(p)).ToList() ; } }
    public int skillPoints;
}
public class BoardConnection
{
    public Board anchorBoard;
    public Board newBoard;
    public Node anchorNode;
    public Node newNode;
}
