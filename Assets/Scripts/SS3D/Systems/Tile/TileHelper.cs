﻿using SS3D.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using SS3D.Utils;

namespace SS3D.Systems.Tile
{
    /// <summary>
    /// Helper class for the tilemap to deal with layers and rotations
    /// </summary>
    public static class TileHelper
    {
        private static TileLayer[] TileLayers;


        /// <summary>
        /// Get a direction 90 degree clockwise from the one passed in parameter.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Direction GetNextCardinalDir(Direction dir)
        {
            switch (dir)
            {
                default:
                case Direction.South: return Direction.West;
                case Direction.SouthWest: return Direction.NorthWest;
                case Direction.West: return Direction.North;
                case Direction.NorthWest: return Direction.NorthEast;
                case Direction.North: return Direction.East;
                case Direction.NorthEast: return Direction.SouthEast;
                case Direction.East: return Direction.South;
                case Direction.SouthEast: return Direction.SouthWest;
            }
        }

        /// <summary>
        /// Get the rotation angle of a particular dir.
        /// E.g. assuming north is the initial position (should be), north return 0, north-east 45 ...
        /// </summary>
        public static int GetRotationAngle(Direction dir)
        {
            return (int)dir * 45;
        }

        /// <summary>
        /// Get all different kind of tile layers.
        /// </summary>
        public static TileLayer[] GetTileLayers()
        {
            if (TileLayers == null)
            {
                TileLayers = (TileLayer[])Enum.GetValues(typeof(TileLayer));
            }
            return TileLayers;
        }

        /// <summary>
        /// Get the two adjacent directions from dir passed in parameter.
        /// e.g if dir is North, return NorthWest and NorthEast.
        /// </summary>

        public static Tuple<Direction,Direction> GetAdjacentDirections(Direction dir)
        {
            return new Tuple<Direction, Direction>( (Direction) MathUtility.mod((int) dir + 1, 8),
                (Direction)MathUtility.mod((int)dir - 1, 8) );
        }

        /// <summary>
        /// Get the offset in coordinates in a given direction.
        /// </summary>
        public static Tuple<int, int> ToCardinalVector(Direction direction)
        {
            return new Tuple<int, int>(
                (direction > Direction.North && direction < Direction.South) ? 1 : (direction > Direction.South) ? -1 : 0,
                (direction > Direction.East && direction < Direction.West) ? -1 : (direction == Direction.East || direction == Direction.West) ? 0 : 1
            );
        }

        /// <summary>
        /// Get the closest round number world position on the plane where y = 0.
        /// </summary>
        public static Vector3 GetClosestPosition(Vector3 worldPosition)
        {
            return new Vector3(Mathf.Round(worldPosition.x), 0, Mathf.Round(worldPosition.z));
        }

        /// <summary>
        /// Get the relative direction between two direction. 
        /// </summary>
        public static Direction GetRelativeDirection(Direction from, Direction to)
        {
            return (Direction)((((int)from - (int)to) + 8) % 8);
        }

        /// <summary>
        /// Return a list of the cardinal directions.
        /// </summary>
        public static List<Direction> CardinalDirections()
        {
            return new List<Direction> { Direction.North, Direction.East, Direction.South, Direction.West };
        }

        /// <summary>
        /// Return a list of all existing directions.
        /// </summary>
        public static List<Direction> AllDirections()
        {
            return new List<Direction> { Direction.North, Direction.NorthEast, Direction.East, Direction.SouthEast,
                Direction.South, Direction.SouthWest, Direction.West, Direction.NorthWest };
        }

        /// <summary>
        /// Return a list of the diagonal directions.
        /// </summary>
        public static List<Direction> DiagonalDirections()
        {
            return new List<Direction> { Direction.NorthEast, Direction.SouthEast, Direction.SouthWest, Direction.NorthWest };
        }

        /// <summary>
        /// Return the diagonal direction between two cardinal directions.
        /// </summary>
        public static Direction GetDiagonalBetweenTwoCardinals(Direction cardinal1, Direction cardinal2)
        {
            List<Direction> givenCardinals = new List<Direction> { cardinal1, cardinal2 };
            return givenCardinals.Contains(Direction.South) ?
                givenCardinals.Contains(Direction.East) ? Direction.SouthEast : Direction.SouthWest :
                givenCardinals.Contains(Direction.West) ? Direction.NorthWest : Direction.NorthEast;
        }

        /// <summary>
        /// Return the cardinal direction between two diagonal directions.
        /// </summary>
        public static Direction GetCardinalBetweenTwoDiagonals(Direction diagonal1, Direction diagonal2)
        {
            List<Direction> givenDiagonals = new List<Direction> { diagonal1, diagonal2 };
            return givenDiagonals.Contains(Direction.SouthEast) ?
                givenDiagonals.Contains(Direction.NorthEast) ? Direction.East : Direction.South :
                givenDiagonals.Contains(Direction.SouthWest) ? Direction.West : Direction.North;
        }

        /// <summary>
        /// Return the angle between two directions, clock wise is positive.
        /// </summary>
        public static float AngleBetween(Direction from, Direction to)
        {
            return ((int)to - (int)from) * 45.0f;
        }

        /// <summary>
        /// Get the opposite direction from the one in parameter.
        /// E.g : North opposite is south. South-East opposite is North-West.
        /// </summary>
        public static Direction GetOpposite(Direction direction)
        {
            return (Direction)(((int)direction + 4) % 8);
        }

        /// <summary>
        /// Return the difference in coordinates for a neighbour tile in front of another one facing
        /// a particular direction.
        /// e.g If the original one is facing north, return (0,1), because, the tile in front of the original
        /// one will be just north of the original one (hence plus one on the y axis).
        /// TODO : isn't "to cardinal vector" method doing the same thing ?
        /// </summary>
        public static Vector2Int CoordinateDifferenceInFrontFacingDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.North:
                    return new Vector2Int(0, 1);

                case Direction.NorthEast:
                    return new Vector2Int(1, 1);

                case Direction.East:
                    return new Vector2Int(1, 0);

                case Direction.SouthEast:
                    return new Vector2Int(1, -1);

                case Direction.South:
                    return new Vector2Int(0, -1);

                case Direction.SouthWest:
                    return new Vector2Int(-1, -1);

                case Direction.West:
                    return new Vector2Int(-1, 0);

                case Direction.NorthWest:
                    return new Vector2Int(-1, 1);

                default:
                    Debug.LogError("direction not handled, returning (0,0)");
                    return new Vector2Int(0, 0);
            }
        }




        public static bool IsCardinalDirection(Direction dir)
        {
            return (int) dir == 0 || (int) dir == 2 || (int) dir == 4 || (int) dir == 6 ;
        }

        /// <summary>
        /// Create the right type of tile location depending on the layer it'll be on.
        /// </summary>
        public static ITileLocation CreateTileLocation(TileLayer layer, int x, int y)
        {
            switch (layer)
            {
                case TileLayer.Plenum:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.Turf:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.Wire:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.Disposal:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.PipeSurface:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.PipeMiddle:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.PipeRight:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.PipeLeft:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.WallMountHigh:
                    return new CardinalTileLocation(layer, x, y);
                case TileLayer.WallMountLow:
                    return new CardinalTileLocation(layer, x, y);
                case TileLayer.FurnitureBase:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.FurnitureTop:
                    return new SingleTileLocation(layer, x, y);
                case TileLayer.Overlays:
                    return new SingleTileLocation(layer, x, y);
                default:
                    Debug.LogError($"no objects defined for layer {layer}, add a case to this switch.");
                    return null;
            }
        }
    }
}