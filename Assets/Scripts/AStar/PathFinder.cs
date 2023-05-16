using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static PathFinder;

public class PathNodeBase
{
    public PathNodeBase prevConnection { get; private set; }
    public float G { get; private set; } //cost from start point
    public float H { get; private set; } //estimate cheapest cost to the end
    public float F => G + H;
    public Vector3Int gridPosition { get; private set; }
    public PathNodeBase(PathNodeBase prevNode, Vector3Int gridPos)
    {
        prevConnection = prevNode;
        gridPosition = gridPos;
    }
    public void SetPrevConnection(PathNodeBase node) => prevConnection = node;
    public void SetGValue(float g) => G = g;
    public void SetHValue(float h) => H = h;
    public void SetGridPosition(Vector3Int pos) => gridPosition = pos;
}

public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    //0-up, 1-up right corner, 2-right, 3-down right corner,
    //4-down, 5-down left corner, 6-left, 7-left up corner
    private readonly Vector3Int[] neighborOffset = new Vector3Int[] {
        new Vector3Int(0,1,0), new Vector3Int(1,1,0), new Vector3Int(1,0,0), new Vector3Int(1,-1,0),
        new Vector3Int(0,-1,0), new Vector3Int(-1,-1,0), new Vector3Int(-1,0,0), new Vector3Int(-1,1,0)
    };
    [SerializeField] private Camera cam;
    [SerializeField] private Grid grid;
    //[SerializeField] private Camera cam;
    [SerializeField] private List<Tilemap> walkableTileLayers = new List<Tilemap>();
    [SerializeField] private List<Tilemap> unwalkableTileLayers = new List<Tilemap>();
    private bool tempCheck;
    private List<PathNodeBase> tempPathCheck = new List<PathNodeBase>();

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDrawGizmos()
    {
        //if(tempPathCheck != null)
        //{
        //    if (tempPathCheck.Count > 0)

        //    {
        //        Vector3 prev = transform.position;
        //        Debug.LogWarning(tempPathCheck.Count);
        //        foreach (PathNodeBase node in tempPathCheck)
        //        {
        //            Vector3 cur = grid.GetCellCenterWorld(node.gridPosition);
        //            Debug.LogWarning(cur);
        //            Gizmos.color = Color.yellow;
        //            Gizmos.DrawLine(prev, cur);
        //            prev = cur;
        //        }
        //    }
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        ////debug
        //if (Input.GetMouseButtonDown(0))
        //{
        //    tempCheck = true;
        //    return;
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    tempCheck = false;
        //    return;
        //}

        //if (tempCheck)
        //{
        //    Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3Int gridpos = grid.WorldToCell(pos);
        //    tempPathCheck = FindPath(WorldToGridPos(transform.position), gridpos);
        //}
    }

    public Vector3Int WorldToGridPos(Vector3 pos)
    {
        return grid.WorldToCell(pos);
    }

    public Vector3 GridToWorldPos(Vector3Int pos)
    {
        return grid.GetCellCenterWorld(pos);
    }

    public bool CheckGridCellWalkable(Vector3Int gridPosition)
    {
        bool result = false;
        //check if thats at least a walkable area rather than a void area
        foreach (Tilemap tilemap in walkableTileLayers)
        {
            if (tilemap.GetTile(gridPosition) != null)
            {
                result = true;
                break;
            }
        }
        //If void area then just return false
        if (result == false)
        {
            return false;
        }
        //If within walkable area, start checking walls and obstacles
        else
        {
            foreach (Tilemap blockedTilemap in unwalkableTileLayers)
            {
                //when there is a obstacle in the grid then return false
                if (blockedTilemap.GetTile(gridPosition) != null)
                {
                    return false;
                }
            }
            //If there is no walls or obstacles at all layers then return true
            return true;
        }


    }

    public List<PathNodeBase> FindPath(Vector3Int start, Vector3Int end)
    {
        //Find Path from start grid point to end grid point with A* algorithm
        
        //Check if the end point is walkable
        if(!CheckGridCellWalkable(end))
        {
            Debug.LogWarning("End point is not walkable");
            return null;
        }

        //Initialize the lists
        List<PathNodeBase> openList = new List<PathNodeBase>();
        List<PathNodeBase> closedList = new List<PathNodeBase>();
        //Initialize the start point
        PathNodeBase startPoint = new PathNodeBase(null, start);
        startPoint.SetHValue(Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y));//calculate Mahattan Distance as H value
        startPoint.SetGValue(0);
        openList.Add(startPoint);
        //set current node as the start node
        PathNodeBase currentNode = openList[0];

        while (openList.Count > 0)
        {
            //Get the node with lowest F value as current Node
            currentNode = openList[0];
            //Check if this node is the end point
            if(currentNode.gridPosition == end)
            {
                break;
            }
            //Move this node from open list to closed List
            closedList.Add(currentNode);
            openList.Remove(currentNode);
            //Add the available neighbors of the current node to the open list
            //Preprocess with the neighbors
            Vector3Int centerPos = currentNode.gridPosition;
            bool[] reachableFlag = new bool[8];
            {

                //0-up, 2-right, 4-down, 6-left
                //1-up right corner, 3-down right corner, 5-down left corner, 7-left up corner

                for(int i=0;i<8;i++)
                {
                    Vector3Int t = centerPos + neighborOffset[i];
                    if (CheckGridCellWalkable(t))
                    {
                        //process direct neibour
                        if(i % 2 == 0)
                        {
                            //Reachable then add this node to the queue waiting for process
                            reachableFlag[i] = true;
                            AddNodeToOpenList(t, currentNode.G + 10, currentNode, ref openList, closedList, end);
                        }
                        //process diagonal neibours
                        else
                        {
                            //Diagonal cells need double check later
                            reachableFlag[i] = true;
                        }
                    }
                    else { reachableFlag[i] = false; }
                }

                //diagonal process
                for (int i = 0; i < 8; i++)
                {
                    Vector3Int t = centerPos + neighborOffset[i];
                    //only process diagonal ones
                    if(i % 2 != 0)
                    {
                        if (reachableFlag[i])
                        {
                            int n1 = (i - 1) % 8;
                            int n2 = (i + 1) % 8;
                            //diagonal grid cell will be blocked if two other nearby grid are unreachable from current cell
                            if (reachableFlag[n1] == false && reachableFlag[n2] == false) { reachableFlag[i] = false; }
                            //and will be special processed when one neighbor is blocked and other one not (make it harder to reach than going around the corner) 
                            else if (reachableFlag[n1] == false || reachableFlag[n2] == false)
                            {
                                AddNodeToOpenList(t, currentNode.G + 24, currentNode, ref openList, closedList, end);
                            }
                            //Normal process for diagonal cells
                            else
                            {
                                AddNodeToOpenList(t, currentNode.G + 14, currentNode, ref openList, closedList, end);
                            }
                        }
                    }
                }
            }
            //sort the open list by F value and H cost in ascending order
            openList.Sort((a, b) =>
            {
                if (a.F.CompareTo(b.F) != 0) return a.F.CompareTo(b.F);
                else
                    return a.H.CompareTo(b.H);
            });
        }

        if (currentNode.gridPosition != end)
        {
            //no available path to the point
            Debug.LogWarning("No reachable Path!!!!");
            return null;
        }
        else
        {
            //trace back the path
            List<PathNodeBase> path = new List<PathNodeBase>();
            TraceBackThePath(path, currentNode);
            return path;
        }
        
    }

    public void TraceBackThePath(List<PathNodeBase> pathNodes, PathNodeBase curNode)
    {
        //if not the start node, then just trace back the previous node
        if (curNode.prevConnection != null)
        {
            TraceBackThePath(pathNodes, curNode.prevConnection);
        }
        //if reach the end then stop and add nodes one by one
        pathNodes.Add(curNode);

    }

    public void AddNodeToOpenList(Vector3Int gridPos, float newG, PathNodeBase cameFrom, ref List<PathNodeBase> openList, List<PathNodeBase> closedList, Vector3Int endPos)
    {
        //Initialize this new node
        PathNodeBase procNode = new PathNodeBase(cameFrom, gridPos);
        procNode.SetGValue(newG);
        procNode.SetHValue(Mathf.Abs(endPos.x - gridPos.x) + Mathf.Abs(endPos.y - gridPos.y));//calculate Mahattan Distance as H value

        //Check if this node should be added to the list
        //Firstly, check if this node has already been processed
        foreach (PathNodeBase node in closedList)
        {
            if (node.gridPosition == procNode.gridPosition)
            {
                return;
            }
        }
        //Then check if this one has already been added to the open list
        foreach (PathNodeBase node in openList)
        {
            if (node.gridPosition == procNode.gridPosition)
            {
                //if this node already in the queue waiting for process, check if the G value decreased
                if (procNode.G < node.G)
                {
                    //if G value decreased, replace the node in the queue with this new node
                    node.SetGValue(procNode.G);
                    node.SetPrevConnection(procNode.prevConnection);
                    return;
                }
                else
                {   //if G value increased then it is useless
                    return;
                }
            }
        }
        //Here the node is not in either lists, so we add it to the open list waiting for process
        openList.Add(procNode);

    }

    public struct SearchNode
    {
        public Vector3Int gridPos;
        public float distanceFromStart;
    }

    public void AddSearchNodetoOpenList(Vector3Int pos, float dis, ref List<SearchNode> openList, List<SearchNode> closedList)
    {
        SearchNode t = new SearchNode();
        t.gridPos = pos;
        t.distanceFromStart = dis;
        //Check if this node should be added to the list
        //Firstly, check if this node has already been processed
        foreach (SearchNode node in closedList)
        {
            if (node.gridPos == t.gridPos)
            {
                return;
            }
        }
        //Then check if this one has already been added to the open list
        foreach (SearchNode node in openList)
        {
            if (node.gridPos == t.gridPos)
            {
                //if this node already in the queue waiting for process, check if the G value decreased
                if (t.distanceFromStart < node.distanceFromStart)
                {
                    //if distance decreased, replace the node in the queue with this new node
                    openList.Remove(node);
                    openList.Add(t);
                    return;
                }
                else
                {   
                    return;
                }
            }
        }
        //Here the node is not in either lists, so we add it to the open list waiting for process
        openList.Add(t);
    }

    public Vector2 FindFleePoint(Vector2 curPos, Vector2 fleeFromPoint, float fleeRange)
    {
        //Initialize start point to search
        SearchNode startNode = new SearchNode();
        startNode.gridPos = WorldToGridPos(fleeFromPoint + (curPos - fleeFromPoint).normalized * fleeRange);
        startNode.distanceFromStart = 0;

        //Initialize the lists
        List<SearchNode> openList = new List<SearchNode>();
        List<SearchNode> closedList = new List<SearchNode>();
        openList.Add(startNode);
        //set current node as the start node
        SearchNode currentNode;

        while(openList.Count > 0) 
        {
            //Set the currentNode as the closest 
            currentNode = openList[0];
            //Check if current node is valid for flee
            float dis = Vector2.Distance(GridToWorldPos(currentNode.gridPos), fleeFromPoint);
            //when the node is walkable and beyond the flee range, the node is a valid fleepoint
            if(CheckGridCellWalkable(currentNode.gridPos) && dis > fleeRange)
            {
                //fleepoint is valid return its position
                return GridToWorldPos(currentNode.gridPos);
            }
            else
            {
                //Move this node from open list to closed List
                closedList.Add(currentNode);
                openList.Remove(currentNode);
                //Not a valid fleepoint, expand the searchlist with the neighbours
                Vector3Int centerPos = currentNode.gridPos;
                for (int i = 0; i < 8; i++)
                {
                    Vector3Int t = centerPos + neighborOffset[i];
                    if (t != WorldToGridPos(fleeFromPoint))
                    {
                        //when not the grid player is in then add this node to the queue waiting for process
                        //process direct neibour
                        if (i % 2 == 0)
                        {
                            AddSearchNodetoOpenList(t, currentNode.distanceFromStart + 10, ref openList, closedList);
                        }
                        //process diagonal neibours
                        else
                        {
                            AddSearchNodetoOpenList(t, currentNode.distanceFromStart + 14, ref openList, closedList);
                        }
                    }
                }
                openList.Sort((a, b) => a.distanceFromStart.CompareTo(b.distanceFromStart));
                
            }
        }
        //no valid fleepoint
        Debug.Log("No valid flee point, stay position");
        return curPos;

    }

}
