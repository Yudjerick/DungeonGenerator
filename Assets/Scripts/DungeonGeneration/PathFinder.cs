using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration
{
    public class Pathfinder
    {
        private bool[,] _solidMap;
        private PathfindingNode[,] _searchMap;
        private List<PathfindingNode> _explored;

        private int _startX;
        private int _startZ;
        private int _endX;
        private int _endZ;

        private PathfindingNode _current;

        public Pathfinder(bool[,] solidMap, int startX, int startZ, int endX, int endZ)
        {
            _solidMap = solidMap;
            _searchMap = new PathfindingNode[_solidMap.GetLength(0), _solidMap.GetLength(1)];
            _explored = new List<PathfindingNode>();
            _startX = startX;
            _startZ = startZ;
            _endX = endX;
            _endZ = endZ;
        }
        public float Heuristic(Vector3 from, Vector3 to)
        {
            Vector3 diff = from - to;
            return Math.Abs(diff.x) + Math.Abs(diff.y) + Math.Abs(diff.z);
        }

        public int Heuristic(int fromX, int fromZ, int toX, int toZ)
        {
            return Math.Abs(fromX - toX) + Math.Abs(fromZ - toZ);
        }


        public List<Vector3> FindPath()
        {

            PathfindingNode startNode = PathfindingNode.StartNode(_startX, _startZ,
                Heuristic(_startX, _startZ, _endX, _endZ));
            _searchMap[_startX, _startZ] = startNode;
            _explored.Add(startNode);
            _current = startNode;
            List<PathfindingNode> potentialWays = new List<PathfindingNode>();
            ResearchAround();
            AddPotentialWaysAround(_current.X, _current.Z, potentialWays);

            int cycle = 0;

            while (true)
            {
                cycle++;
                float minF = float.MaxValue;
                PathfindingNode bestNext = null;
                foreach (var node in potentialWays)
                {
                    if (node.F < minF)
                    {
                        minF = node.F;
                        bestNext = node;
                    }
                }
                if (bestNext != null)
                {
                    potentialWays.Remove(bestNext);
                    _current = bestNext;
                    _current.Explored = true;
                    _explored.Add(_current);
                    ResearchAround();
                    AddPotentialWaysAround(_current.X, _current.Z, potentialWays);
                }
                if (_current.X == _endX && _current.Z == _endZ)
                {
                    List<Vector3> path = new List<Vector3>();
                    while (_current.Previous != null)
                    {
                        path.Add(new Vector3(_current.X, 0, _current.Z));
                        _current = _current.Previous;
                    }
                    path.Add(new Vector3(_current.X, 0, _current.Z));
                    return path;
                }
                if (potentialWays.Count == 0)
                {
                    Debug.Log("No way");
                    return null;
                }
                if (cycle > 1000)
                {
                    break;
                }
            }
            return null;
        }

        private void AddPotentialWaysAround(int x, int z, List<PathfindingNode> ways)
        {
            AddPotentialWayIfPossible(x + 1, z, ways);
            AddPotentialWayIfPossible(x - 1, z, ways);
            AddPotentialWayIfPossible(x, z + 1, ways);
            AddPotentialWayIfPossible(x, z - 1, ways);
        }

        private void AddPotentialWayIfPossible(int x, int z, List<PathfindingNode> ways)
        {
            if (x == _endX && z == _endZ)
            {
                ways.Add(_searchMap[x, z]);
                return;
            }
            if (x < 0 || x >= _solidMap.GetLength(0))
            {
                return;
            }
            if (z < 0 || z >= _solidMap.GetLength(1))
            {
                return;
            }
            if (!_searchMap[x, z].Explored && !_searchMap[x, z].IsSolid && !ways.Contains(_searchMap[x, z]))
            {
                ways.Add(_searchMap[x, z]);
            }
        }

        private void ResearchAround()
        {

            ResearchCoords(_current.X + 1, _current.Z, _current);
            ResearchCoords(_current.X - 1, _current.Z, _current);
            ResearchCoords(_current.X, _current.Z + 1, _current);
            ResearchCoords(_current.X, _current.Z - 1, _current);
        }

        private void ResearchCoords(int x, int z, PathfindingNode current)
        {
            if (x == _endX && z == _endZ)
            {
                _searchMap[x, z] = new PathfindingNode(false, false, x, z, current, 0);
                return;
            }
            if (x < 0 || x >= _solidMap.GetLength(0))
            {
                return;
            }
            if (z < 0 || z >= _solidMap.GetLength(1))
            {
                return;
            }
            if (_searchMap[x, z] == null)
            {
                if (_solidMap[x, z])
                {
                    _searchMap[x, z] = PathfindingNode.Solid(x, z);
                    return;
                }
                _searchMap[x, z] = new PathfindingNode(false, false, x, z, current, Heuristic(x, z, _endX, _endZ));
                return;
            }
            if (_searchMap[x, z].G > _current.G + 1)
            {
                _searchMap[x, z].Previous = _current;
            }
        }
    }
}