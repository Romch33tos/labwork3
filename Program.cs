﻿using System;

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
  
    public int[,] GetMatrixFromArray(int[] elements) {
      int extension = (int)Math.Sqrt(elements.Length);
      int[,] matrix = new int[extension, extension];
      int elementIndex = 0;

      for (int rowIndex = 0; rowIndex < extension; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < extension; ++columnIndex) {

          try {
            matrix[rowIndex, columnIndex] = elements[elementIndex];
          } catch (System.IndexOutOfRangeException exception) {
            Console.WriteLine(exception.Message);
          }

          ++elementIndex;
        }
      }

      return matrix;
    }

    public void AutoFill(int extension, int minElement = -10, int MaxElement = 10) {
      int[] elements = new int[extension * extension];
      var random = new Random();

      for (int elementIndex = 0; elementIndex < extension * extension; ++elementIndex) {
        elements[elementIndex] = random.Next(minElement, MaxElement);
      }

      MatrixArray = GetMatrixFromArray(elements);
    }

    public SquareMatrix Clone() {
      int[,] elements = new int[this.Extension, this.Extension];

      for (int rowIndex = 0; rowIndex < this.Extension; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < this.Extension; ++columnIndex) {
          elements[rowIndex, columnIndex] = this.MatrixArray[rowIndex, columnIndex];
        }
      }

      return new SquareMatrix(elements);
    }

    public int SumOfElements() {
      int sumOfElements = 0;

      for (int rowIndex = 0; rowIndex < this.Extension; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < this.Extension; ++columnIndex) {
          sumOfElements += this.MatrixArray[rowIndex, columnIndex];
        }
      }

      return sumOfElements;
    }

        public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
      if (firstMatrix.Extension != secondMatrix.Extension) {
        throw new DifferentSizesException("Операцию сложения можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var result = firstMatrix.Clone();
      int extension = firstMatrix.Extension;;

      for (int rowIndex = 0; rowIndex < extension; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < extension; ++columnIndex) {
          result.MatrixArray[rowIndex, columnIndex] += secondMatrix.MatrixArray[rowIndex, columnIndex];
        }
      }

      return result;
    }

    public static SquareMatrix operator -(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
      if (firstMatrix.Extension != secondMatrix.Extension) {
        throw new DifferentSizesException("Операцию вычитания можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var result = firstMatrix.Clone();
      int extension = firstMatrix.Extension;

      for (int rowIndex = 0; rowIndex < extension; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < extension; ++columnIndex) {
          result.MatrixArray[rowIndex, columnIndex] -= secondMatrix.MatrixArray[rowIndex, columnIndex];
        }
      }

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
      if (firstMatrix.Extension != secondMatrix.Extension) {
        throw new DifferentSizesException("Операцию умножения можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var result = firstMatrix.Clone();
      int extension = firstMatrix.Extension;

      for (int rowIndexOfFirstMatrix = 0; rowIndexOfFirstMatrix < firstMatrix.Extension; ++rowIndexOfFirstMatrix) {
        for (int columnIndex = 0; columnIndex < extension; ++columnIndex) {
          result.MatrixArray[rowIndexOfFirstMatrix, columnIndex] = 0;

          for (int indexOfSecondElement = 0; indexOfSecondElement < extension; ++indexOfSecondElement) {
            result.MatrixArray[rowIndexOfFirstMatrix, columnIndex] += firstMatrix.MatrixArray[rowIndexOfFirstMatrix, indexOfSecondElement] * secondMatrix.MatrixArray[indexOfSecondElement, columnIndex];
          }
        }
      }

      return result;
    }

    public int CompareTo(SquareMatrix other) {
      if (this.Extension == other.Extension) {
        if (this.SumOfElements() > other.SumOfElements()) {
          return 1;
        } else if (this.SumOfElements() == other.SumOfElements()) {
          return 0;
        } else if (this.SumOfElements() < other.SumOfElements()) {
          return -1;
        }
      }
      return -1;
    }
