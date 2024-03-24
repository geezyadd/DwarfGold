using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.MatrixModule.Core.Scripts.Exceptions;
using UnityEngine;

namespace RSG.Muffin.MatrixModule.Core.Scripts.Services.MatrixPathFinder {
    public class MatrixPathFinder : IMatrixPathFinder
    {
        private const int ITERATION_CYCLE_STOP = 200;
        public List<Vector2Int> PathFinding<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, Vector2Int startPosition, Vector2Int endPosition, Predicate<TMatrixEntity> predicate = null, bool random = false) {
            Node start = new(startPosition.x, startPosition.y);
            Node end = new(endPosition.x, endPosition.y);
            List<Node> openList = new();
            List<Vector2Int> path = new();
            
            openList.Add(start);
            int cycleCount = 0;
            while (openList.Count > 0) {
                if (cycleCount > ITERATION_CYCLE_STOP)
                    throw new InfiniteLoopTypeException();

                Node currentNode = openList.OrderBy(n => n.TotalCost)
                    .First();
                
                openList.Remove(currentNode);
                
                if (currentNode.Equals(end)) {
                    while (currentNode != null) {
                        path.Add(new Vector2Int(currentNode.X, currentNode.Y));
                        currentNode = currentNode.Parent;
                    }

                    path.Reverse();
                    return path;
                }

                if (!NeighborValidator(matrix, currentNode, predicate, openList, end, random))
                    return new List<Vector2Int>();

                cycleCount++;
            }

            return path;
        }

        private bool NeighborValidator<TMatrixEntity>(IMatrix<TMatrixEntity> matrix, Node currentNode, Predicate<TMatrixEntity> predicate, List<Node> openList, Node end, bool random) {
            List<Vector2Int> neighborsVectorPosition = new();
            if (random)
            {
                neighborsVectorPosition = matrix.GetNeighborsRandomly(new Vector2Int(currentNode.X, currentNode.Y), predicate);
            }
            else
            {
                neighborsVectorPosition = matrix.GetNeighbors(new Vector2Int(currentNode.X, currentNode.Y), predicate);
            }
            List<Node> neighbors = new();
            foreach (Vector2Int position in neighborsVectorPosition)
                neighbors.Add(new Node(position.x, position.y));
        
            bool isCantFindPath = false;
            foreach (Node neighbor in neighbors) {
                if (openList.Any(n => n.Equals(neighbor)) && currentNode.CostSoFar >= neighbor.CostSoFar) {
                    isCantFindPath = false;
                    continue;
                }
        
                neighbor.CostSoFar = currentNode.CostSoFar;
                neighbor.HeuristicCost = Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);
                neighbor.TotalCost = neighbor.CostSoFar + neighbor.HeuristicCost;
                neighbor.Parent = currentNode;
        
                if (!openList.Any(n => n.Equals(neighbor)))
                    openList.Add(neighbor);
        
                isCantFindPath = true;
            }
        
            return isCantFindPath;
        }
    }
}
