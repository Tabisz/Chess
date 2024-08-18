using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.Units;
using _Scripts.Utils;
using UnityEngine;
using Random = System.Random;

public class GridManager : MonoBehaviour, ICustomInitializer
{
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Vector2 buildingOffset;
    [SerializeField] private float tileSize;
    
    private Dictionary<(int,int), Tile> _tiles;

    public void Init()
    {
        GenerateGrid();
        GameController.Instance.GameplayRefsHolder.Observer.OnUnitSelected += OnPlayerUnitSelected;
        
        GameController.Instance.GameplayRefsHolder.Observer.OnEmptyTileSelected += OnEmptyTileSelected;
        GameController.Instance.GameplayRefsHolder.Observer.OnEmptyTileSecondarySelected += OnEmptyTileSelected;

        

    }
 
     private void GenerateGrid() {
        _tiles = new Dictionary<(int, int), Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x + buildingOffset.x, y+ buildingOffset.y), Quaternion.identity, transform);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset,x,y, this);
                
                _tiles[(x, y)] = spawnedTile;
            }
        }
    }

     private void OnPlayerUnitSelected(Unit unit)
     {
        ShowRangesForUnit(unit);
     }

     public void ShowRangesForUnit(Unit unit)
     {
         RefreshGrid();
         if (unit.Fraction == Fraction.PLAYER)
         {
             if (unit.CanPerformMove())
             {
                 var tiles = GetSurroundingTiles(unit.CurrentTile, unit.Statistics.MoveRange);
                 for (var i = 0; i < tiles.Length; i++)
                 {
                     var tile = tiles[i];
                     tile.Highlight(false);
                 }                 
             }
             else
                 unit.CurrentTile.Highlight(false);

             if (unit.CanPerformAttack())
             {
                 var tiles = GetSurroundingTiles(unit.CurrentTile, unit.Statistics.AttackRange);
                 for (var i = 0; i < tiles.Length; i++)
                 {
                     var tile = tiles[i];
                     tile.DoubleHighlight(false);
                 }       
             }

         }
         else
         {
             var tiles = GetSurroundingTiles(unit.CurrentTile, unit.Statistics.MoveRange);
             for (var i = 0; i < tiles.Length; i++)
             {
                 var tile = tiles[i];
                 tile.Highlight(true);
             }  
             
             tiles = GetSurroundingTiles(unit.CurrentTile, unit.Statistics.AttackRange);
             for (var i = 0; i < tiles.Length; i++)
             {
                 var tile = tiles[i];
                 tile.DoubleHighlight(true);
             }    
         }
     }

     private void OnEmptyTileSelected(Tile tile)
     {
         RefreshGrid();
     }

     #region Utils

     public int GetDistance(Tile sourceTile, Tile destinationTile)
     {
         int deltaX = Mathf.Abs(sourceTile.XCoordinate - destinationTile.XCoordinate);
         int deltaY = Mathf.Abs(sourceTile.YCoordinate - destinationTile.YCoordinate);
         return deltaX + deltaY / 2;
     }
     public Tile GetClosestFreeTileTowardsDestination(Tile sourceTile, Tile destinationTile, int range)
     {
         return GetClosestFreeTileTowardsDestination(sourceTile.XCoordinate, sourceTile.YCoordinate,
             destinationTile.XCoordinate, destinationTile.YCoordinate, range);
     }
     
     public Tile GetClosestFreeTileTowardsDestination(int sX,int sY, int eX,int  eY, int range)
     {
         int newX = sX, newY = sY;
         List<(int, int)> path = new List<(int, int)>();
         path.Add((sX,sY));
         do
         {
             if (eX > newX && eY > newY) //go right up diagonal
             {
                 newX++;
                 newY++;
             }
             else if (eX > newX && eY < newY) // go right down diagonal
             {
                 newX++;
                 newY--;
             }
             else if (eX < newX && eY > newY) //go left up diagonal
             {
                 newX--;
                 newY++;
             }
             else if (eX < newX && eY < newY) // go left down diagonal
             {
                 newX--;
                 newY--;
             }
             else if (eX == newX && eY > newY) //go up
             {
                 newY++;
             }
             else if (eX == newX && eY < newY) // go down
             {
                 newY--;
             }
             else if (eX < newX && eY == newY) //go left
             {
                 newX--;
             }
             else if (eX < newX && eY == newY) // go right 
             {
                 newX++;
             }
            path.Add((newX,newY));
            range--;
         } while (range > 0);

         for (int i = path.Capacity-1; i > 0; i--)//step back if you get obstructed tile 
         {
             var tile = GetTileAtPosition(path[i].Item1, path[i].Item2);
             if(tile!= null && tile.CurrentTileOccipier == null)
                 return tile;
         }

         return GetTileAtPosition(path[0].Item1, path[0].Item2);// if you dont get any space just return current tile where you stand


     }
     
     public bool IsTileInRange(Tile sourceTile, Tile tileToCheck, int range)
     {
         (int xmin, int xmax, int ymin, int ymax) = CalculateCoordsInRange(sourceTile.XCoordinate,sourceTile.YCoordinate,range);

         return tileToCheck.XCoordinate >= xmin && tileToCheck.XCoordinate <= xmax &&
                tileToCheck.YCoordinate >= ymin && tileToCheck.YCoordinate <= ymax;
     }

     public Tile GetRandomFreeTile()
     {
         Random random = new Random();
         int xRand = random.Next(0,_width);
         int yRand = random.Next(0, _height);

         var tile = GetTileAtPosition(xRand, yRand);
         while (tile.CurrentTileOccipier != null)
             tile = GetRandomFreeTile();
         return tile;
     }
     
     public Vector2 GetPositionOfTile(Tile tile)
     {
         return GetPositionOfTile(tile.XCoordinate, tile.YCoordinate);
     }
     public Vector2 GetPositionOfTile(int x, int y)
     {
         Vector2 ret = buildingOffset;
         ret += new Vector2(x * tileSize, y * tileSize);
         return ret;
     }
     
     public Tile GetTileAtPosition(int x, int y)
     {
         if (_tiles.TryGetValue((x,y), out var tile)) return tile;
         return null;
     }
     public Tile GetTileAtPosition(Vector2 position)
     {
         (int x, int y) = GetNormalizedCoordinates(position);

        return GetTileAtPosition(x, y);
    }
     
     public Tile[] GetSurroundingTiles(Tile tile,int radius)
      {
          return GetSurroundingTiles(tile.XCoordinate, tile.YCoordinate, radius);
      }
     
     public Tile[] GetSurroundingTiles(Vector2 position,int radius)
     {
            (int x, int y) = GetNormalizedCoordinates(position);
         return GetSurroundingTiles(x,y, radius);
     }
     
     public Tile[] GetSurroundingTiles(int x, int y,int radius)
     {
         (int xmin, int xmax, int ymin, int ymax) = CalculateCoordsInRange(x,y,radius);

         int wantedCount = (xmax - xmin+1) * (ymax - ymin+1);
         Tile[] tiles = new Tile[wantedCount];
         int k = 0;
         for (int i = xmin; i <= xmax; i++)
         {
             for (int j = ymin; j <= ymax; j++)
             {
                 if (_tiles.TryGetValue((i, j), out var d))
                 {
                     tiles[k] = d;
                     k++;
                 }
             }
         }

         return tiles;
     }

     private (int, int, int ,int) CalculateCoordsInRange(int x, int y,int radius)
     {
         int BottomClamp(int v) =>  v < 0 ? 0 : v;
         int UpperClamp(int v, int max) =>v >= max ? max-1 : v;
         
         var xmin = x - radius;
         var ymin = y - radius;
         xmin = BottomClamp(xmin);
         ymin = BottomClamp(ymin);
         var xmax = x + radius;
         var ymax = y + radius;
         xmax = UpperClamp(xmax, _width);
         ymax = UpperClamp(ymax,_height);

         return (xmin, xmax, ymin, ymax);
     }
     
     private (int, int) GetNormalizedCoordinates(Vector2 position)
     {
         int x = (int)((position.x - buildingOffset.x)/tileSize);
         int y = (int)((position.y - buildingOffset.y)/tileSize);
         
         return (x, y);
     }


     public void RefreshGrid()
     {
         for (int x = 0; x < _width; x++) {
             for (int y = 0; y < _height; y++)
             {
                 _tiles.TryGetValue((x, y), out var tile);
                 if (tile)
                     tile.Unhighlight();
             }
         }
     }
     
     #endregion

    public void Deinit()
    {
        
    }
}