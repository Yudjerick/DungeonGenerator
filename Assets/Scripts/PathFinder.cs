using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private bool[,,] _solidMap;
    private PathfindingNode[,,] _searchMap;
    private List<PathfindingNode> _explored;

    private int _startX;
    private int _startY;
    private int _startZ;
    private int _endX;
    private int _endY;
    private int _endZ;

    private PathfindingNode _current;

    public Pathfinder(bool[,,] solidMap, int startX, int startY, int startZ, int endX, int endY, int endZ){
        _solidMap = solidMap;
        _searchMap = new PathfindingNode[_solidMap.GetLength(0), _solidMap.GetLength(1), _solidMap.GetLength(2)];
        _explored = new List<PathfindingNode>();
        _startX = startX;
        _startY = startY;
        _startZ = startZ;
        _endX = endX;
        _endY = endY;
        _endZ = endZ;
    }
    public float Heuristic(Vector3 from, Vector3 to){
        Vector3 diff = from - to;
        return Math.Abs(diff.x) + Math.Abs(diff.y) + Math.Abs(diff.z);
    }

    public int Heuristic(int fromX, int fromY, int fromZ, int toX, int toY, int toZ){
        return Math.Abs(fromX - toX) + Math.Abs(fromY - toY) + Math.Abs(fromZ - toZ);
    }


    public List<Vector3> FindPath(){
        
        PathfindingNode startNode = PathfindingNode.StartNode(_startX, _startY, _startZ, 
            Heuristic(_startX, _startY, _startZ, _endX, _endY, _endZ));
        _searchMap[_startX, _startY, _startZ] = startNode;
        _explored.Add(startNode);
        _current = startNode;
        List<PathfindingNode> potentialWays = new List<PathfindingNode>();
        ResearchAround();
        AddPotentialWaysAround(_current.X, _current.Y, _current.Z, potentialWays);
        
        int cycle = 0;
        
        while(true){
            cycle++;
            float minH = float.MaxValue;
            PathfindingNode bestNext = null;
            foreach (var node in potentialWays)
            {
                if(node.H < minH){
                    minH = node.H;
                    bestNext = node;
                }
            }
            if(bestNext != null)
            {
                potentialWays.Remove(bestNext);
                _current = bestNext;
                _current.Explored = true;
                _explored.Add(_current);
                ResearchAround();
                AddPotentialWaysAround(_current.X, _current.Y, _current.Z, potentialWays);
            }
            if(_current.X == _endX && _current.Y == _endY && _current.Z == _endZ)
            {
                List<Vector3> path = new List<Vector3>();
                while(_current.Previous != null)
                {
                    path.Add(new Vector3(_current.X, _current.Y, _current.Z));
                    _current = _current.Previous;
                }
                return path;
            }
            if(potentialWays.Count == 0){
                Debug.Log("No way");
                return null;
            }
            if(cycle > 1000){
                break;
            }
        }
        return null;
    }

    private void AddPotentialWaysAround(int x, int y, int z, List<PathfindingNode> ways)
    {
        AddPotentialWayIfPossible(x+1,y, z, ways);
        AddPotentialWayIfPossible(x-1,y, z, ways);
        AddPotentialWayIfPossible(x,y, z+1, ways);
        AddPotentialWayIfPossible(x,y, z-1, ways);
    }

    private void AddPotentialWayIfPossible(int x, int y, int z, List<PathfindingNode> ways){
        if(x == _endX && y == _endY && z == _endZ){
            ways.Add(_searchMap[x,y,z]);
            return;
        }
        if(x < 0 || x >= _solidMap.GetLength(0)){
            return;
        }
        if(y < 0 || y >= _solidMap.GetLength(1)){
            return;
        }
        if(z < 0 || z >= _solidMap.GetLength(2)){
            return;
        }
        if(!_searchMap[x,y,z].Explored && !_searchMap[x,y,z].IsSolid && !ways.Contains(_searchMap[x,y,z])){
            ways.Add(_searchMap[x,y,z]);
        }
    }

    private void ResearchAround(){
        
        ResearchCoords(_current.X + 1, _current.Y, _current.Z, _current);
        ResearchCoords(_current.X - 1, _current.Y, _current.Z, _current);
        ResearchCoords(_current.X, _current.Y, _current.Z + 1, _current);
        ResearchCoords(_current.X, _current.Y, _current.Z - 1, _current);
    }

    private void ResearchCoords(int x, int y, int z, PathfindingNode current){
        if(x == _endX && y == _endY && z == _endZ){
            _searchMap[x,y,z] = new PathfindingNode(false, false, x, y, z, current, 0);
            return;
        }
        if(x < 0 || x >= _solidMap.GetLength(0)){
            return;
        }
        if(y < 0 || y >= _solidMap.GetLength(1)){
            return;
        }
        if(z < 0 || z >= _solidMap.GetLength(2)){
            return;
        }
        if(_searchMap[x, y, z] == null)
        {
            if(_solidMap[x,y,z]){
                _searchMap[x,y,z] = PathfindingNode.Solid(x,y,z);
                return;
            }
            _searchMap[x,y,z] = new PathfindingNode(false, false, x, y, z, current, Heuristic(x,y,z,_endX, _endY, _endZ));
            return;
        }
        if(_searchMap[x,y,z].G > _current.G + 1){
            _searchMap[x,y,z].Previous = _current;
        }
    }
}
