using System;

namespace Program {
  class SquareMatrix : IComparable<SquareMatrix> {
    public int Extension {
      get {
        return (int)Math.Sqrt(MatrixArray.Length);
      }
    }
    public int[,] MatrixArray { get; set; }
    private int hash;
    
    public int Hash {
      get {
        hash = MatrixArray[0, 0];

        for (int rowIndex = 0; rowIndex < Extension; ++rowIndex) {
          for (int columnIndex = 0; columnIndex < Extension; ++columnIndex) {
            hash = hash * MatrixArray[rowIndex, columnIndex] + hash % MatrixArray[rowIndex, columnIndex];
          }
        }

        return hash;
      }
    }

    public SquareMatrix(int[,] elements) {
      MatrixArray = elements;
    }

    public SquareMatrix(params int[] elements) {
      MatrixArray = GetMatrixFromArray(elements);
    }
  }
}