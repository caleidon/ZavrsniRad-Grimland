using System;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    public enum StructureType : byte
    {
        Pivot = 0, // The point around which the entire Structure is going to be rotated
        Normal = 1, // A normal part of the structure with it's properties applied
        Empty = 2, // Not part of the structure, only used to define a N*N matrix
        UsePoint = 3 // A point where a creature needs to stand to use a certain structure
    }

    public static StructureType[,] ConvertYamlStructure(List<string[]> yamlStructure)
    {
        var depth = yamlStructure.Count;
        var width = yamlStructure[0].Length;

        StructureType[,] structureArray = new StructureType[depth, width];

        for (int currentDepth = 0; currentDepth < depth; currentDepth++)
        {
            for (int currentWidth = 0; currentWidth < width; currentWidth++)
            {
                var structureType = yamlStructure[currentDepth][currentWidth];
                structureArray[currentDepth, currentWidth] = (StructureType)Enum.Parse(typeof(StructureType), structureType);
            }
        }

        return structureArray;
    }

    public static Dictionary<Vector3Int, StructureType> InitializeWorldStructure(Vector3Int pivot, StructureType[,] structure, MatrixUtils.MatrixRotation rotation)
    {
        Dictionary<Vector3Int, StructureType> structureInfo = new Dictionary<Vector3Int, StructureType>();

        Vector3Int[,] coordMatrix = MatrixUtils.CalculateCoordMatrixFromPivot(pivot, structure, rotation);
        StructureType[,] structureMatrix = MatrixUtils.RotateMatrix(structure, rotation);

        for (int depth = 0; depth < structure.GetLength(0); depth++)
        {
            for (int width = 0; width < structure.GetLength(0); width++)
            {
                structureInfo.Add(coordMatrix[depth, width], structureMatrix[depth, width]);
            }
        }

        return structureInfo;
    }
}