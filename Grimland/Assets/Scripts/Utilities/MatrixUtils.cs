using System;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

public static class MatrixUtils
{
    public enum MatrixRotation : byte
    {
        None = 0,
        Left = 1,
        Right = 2,
        Flipped = 3
    }

    public static Vector3Int[,] RotateCoordMatrix(Vector3Int[,] oldMatrix, Vector3Int pivot, MatrixRotation rotation)
    {
        // GetLength(1) gets columns, GetLength(0) gets rows
        Vector3Int[,] newMatrix = new Vector3Int[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newRow = 0;
        for (int oldColumn = 0; oldColumn < oldMatrix.GetLength(1); oldColumn++)
        {
            int newColumn = 0;
            for (int oldRow = oldMatrix.GetLength(0) - 1; oldRow >= 0; oldRow--)
            {
                newMatrix[newRow, newColumn] = RotateCoordinate(oldMatrix[oldRow, oldColumn], pivot, rotation);
                newColumn++;
            }

            newRow++;
        }

        return newMatrix;
    }

    public static T[,] RotateMatrix<T>(T[,] oldMatrix, MatrixRotation rotation)
    {
        switch (rotation)
        {
            case MatrixRotation.Left:
                return RotateMatrixLeft(oldMatrix);
            case MatrixRotation.Right:
                return RotateMatrixRight(oldMatrix);
            case MatrixRotation.Flipped:
                return RotateMatrix180(oldMatrix);
            case MatrixRotation.None:
                return oldMatrix;
            default:
                throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
        }
    }

    private static T[,] RotateMatrixLeft<T>(T[,] oldMatrix)
    {
        // GetLength(1) gets columns, GetLength(0) gets rows
        T[,] newMatrix = new T[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newRow = 0;
        for (int oldColumn = oldMatrix.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            int newColumn = 0;
            for (int oldRow = 0; oldRow < oldMatrix.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }

            newRow++;
        }

        return newMatrix;
    }

    private static T[,] RotateMatrixRight<T>(T[,] oldMatrix)
    {
        // GetLength(1) gets columns, GetLength(0) gets rows
        T[,] newMatrix = new T[oldMatrix.GetLength(1), oldMatrix.GetLength(0)];
        int newRow = 0;
        for (int oldColumn = 0; oldColumn < oldMatrix.GetLength(1); oldColumn++)
        {
            int newColumn = 0;
            for (int oldRow = oldMatrix.GetLength(0) - 1; oldRow >= 0; oldRow--)
            {
                newMatrix[newRow, newColumn] = oldMatrix[oldRow, oldColumn];
                newColumn++;
            }

            newRow++;
        }

        return newMatrix;
    }

    private static T[,] RotateMatrix180<T>(T[,] oldMatrix)
    {
        var firstRotation = RotateMatrixRight(oldMatrix);
        var secondRotation = RotateMatrixRight(firstRotation);
        return secondRotation;
    }


    private static Vector3Int RotateCoordinate(Vector3Int oldCoord, Vector3Int pivot, MatrixRotation rotation)
    {
        float[,] rotationArray;

        switch (rotation)
        {
            case MatrixRotation.Left:
                rotationArray = new float[,] { { 0, -1, pivot.x + pivot.y }, { 1, 0, pivot.y - pivot.x } };
                break;
            case MatrixRotation.Right:
                rotationArray = new float[,] { { 0, 1, pivot.x - pivot.y }, { -1, 0, pivot.y + pivot.x } };
                break;
            case MatrixRotation.Flipped:
                rotationArray = new float[,] { { -1, 0, 2 * pivot.x }, { 0, -1, 2 * pivot.y } };
                break;
            case MatrixRotation.None:
                rotationArray = new float[,] { { 1, 0, 0 }, { 0, 1, 0 } };
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
        }

        return RotateCoordinate(oldCoord, rotationArray);
    }

    private static Vector3Int RotateCoordinate(Vector3Int oldCoord, float[,] rotationArray)
    {
        var matrixBuilder = Matrix<float>.Build;
        float[,] targetArray = { { oldCoord.x }, { oldCoord.y }, { 1 } };

        Matrix<float> rotationMatrix = matrixBuilder.DenseOfArray(rotationArray);
        Matrix<float> targetMatrix = matrixBuilder.DenseOfArray(targetArray);

        Matrix<float> resultMatrix = rotationMatrix.Multiply(targetMatrix);

        int xResult = (int)resultMatrix[0, 0];
        int yResult = (int)resultMatrix[1, 0];

        return new Vector3Int(xResult, yResult, 0);
    }

    public static Vector3Int[,] CalculateCoordMatrixFromPivot(Vector3Int pivot, Structure.StructureType[,] structure,
        MatrixRotation rotation)
    {
        Vector3Int[,] coordMatrix = new Vector3Int[structure.GetLength(0), structure.GetLength(1)];

        int pivotRow = 0;
        int pivotColumn = 0;

        for (int row = 0; row < coordMatrix.GetLength(0); row++)
        {
            for (int column = 0; column < coordMatrix.GetLength(1); column++)
            {
                if (structure[row, column] != Structure.StructureType.Pivot)
                {
                    continue;
                }

                pivotRow = row;
                pivotColumn = column;
            }
        }

        for (int row = 0; row < coordMatrix.GetLength(0); row++)
        {
            for (int column = 0; column < coordMatrix.GetLength(1); column++)
            {
                int xDiff = 0;
                int yDiff = 0;

                if (row < pivotRow)
                {
                    yDiff = pivotRow + row;
                }
                else if (row > pivotRow)
                {
                    yDiff = pivotRow - row;
                }
                else if (row == pivotRow)
                {
                    yDiff = 0;
                }

                if (column < pivotColumn)
                {
                    xDiff = column - pivotColumn;
                }
                else if (column > pivotColumn)
                {
                    xDiff = column - pivotColumn;
                }
                else if (column == pivotColumn)
                {
                    xDiff = 0;
                }


                Vector3Int coordinate = pivot + new Vector3Int(xDiff, yDiff, 0);
                coordMatrix[row, column] = coordinate;
            }
        }

        return rotation != MatrixRotation.None ? RotateCoordMatrix(coordMatrix, pivot, rotation) : coordMatrix;
    }
}