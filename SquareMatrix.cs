using System;

namespace Matrix
{
  class SquareMatrix : IComparable<SquareMatrix>
  {
    private const int MIN_ELEMENT_VALUE = -10;
    private const int MAX_ELEMENT_VALUE = 10;
    private const int MAX_MATRIX_SIZE = 3;

    public int MatrixSize => (int)Math.Sqrt(MatrixElements.Length);
    public int[,] MatrixElements { get; private set; }
    private int hashValue;

    public int Hash
    {
      get
      {
        hashValue = 1;
        for (int rowIndex = 0; rowIndex < MatrixSize; ++rowIndex)
        {
          for (int columnIndex = 0; columnIndex < MatrixSize; ++columnIndex)
          {
            hashValue = hashValue * 31 + MatrixElements[rowIndex, columnIndex];
          }
        }
        return hashValue;
      }
    }

    public SquareMatrix(int[,] elements)
    {
      MatrixElements = DeepCopy(elements);
    }

    public SquareMatrix(params int[] elements)
    {
      MatrixElements = GetMatrixFromArray(elements);
    }

    private int[,] DeepCopy(int[,] originalMatrix)
    {
      int[,] matrixCopy = new int[originalMatrix.GetLength(0), originalMatrix.GetLength(1)];
      Array.Copy(originalMatrix, matrixCopy, originalMatrix.Length);
      return matrixCopy;
    }

    public int[,] GetMatrixFromArray(int[] elementsArray)
    {
      int matrixSize = (int)Math.Sqrt(elementsArray.Length);
      int[,] matrix = new int[matrixSize, matrixSize];
      int elementIndex = 0;

      for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < matrixSize; ++columnIndex)
        {
          if (elementIndex < elementsArray.Length)
          {
            matrix[rowIndex, columnIndex] = elementsArray[elementIndex++];
          }
          else
          {
            throw new ArgumentException("Недостаточно элементов для заполнения матрицы.");
          }
        }
      }
      return matrix;
    }

    public void AutoFill(int matrixSize)
    {
      if (matrixSize > MAX_MATRIX_SIZE)
      {
        throw new ArgumentOutOfRangeException(nameof(matrixSize), "Размерность матрицы не может превышать 3.");
      }

      int[] randomElements = new int[matrixSize * matrixSize];
      var randomGenerator = new Random();

      for (int elementIndex = 0; elementIndex < matrixSize * matrixSize; ++elementIndex)
      {
        randomElements[elementIndex] = randomGenerator.Next(MIN_ELEMENT_VALUE, MAX_ELEMENT_VALUE);
      }

      MatrixElements = GetMatrixFromArray(randomElements);
    }

    public SquareMatrix Clone()
    {
      return new SquareMatrix(DeepCopy(this.MatrixElements));
    }

    public int SumOfElements()
    {
      int totalSum = 0;
      for (int rowIndex = 0; rowIndex < this.MatrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.MatrixSize; ++columnIndex)
        {
          totalSum += this.MatrixElements[rowIndex, columnIndex];
        }
      }
      return totalSum;
    }

    public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix.MatrixSize != secondMatrix.MatrixSize)
      {
        throw new InvalidOperationException("Операцию сложения можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var resultMatrix = firstMatrix.Clone();
      int matrixSize = firstMatrix.MatrixSize;

      for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < matrixSize; ++columnIndex)
        {
          resultMatrix.MatrixElements[rowIndex, columnIndex] += secondMatrix.MatrixElements[rowIndex, columnIndex];
        }
      }

      return resultMatrix;
    }

    public static SquareMatrix operator -(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix.MatrixSize != secondMatrix.MatrixSize)
      {
        throw new InvalidOperationException("Операцию вычитания можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var resultMatrix = firstMatrix.Clone();
      int matrixSize = firstMatrix.MatrixSize;

      for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < matrixSize; ++columnIndex)
        {
          resultMatrix.MatrixElements[rowIndex, columnIndex] -= secondMatrix.MatrixElements[rowIndex, columnIndex];
        }
      }

      return resultMatrix;
    }

    public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix.MatrixSize != secondMatrix.MatrixSize)
      {
        throw new InvalidOperationException("Операцию умножения можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
      }

      var resultMatrix = new SquareMatrix(new int[firstMatrix.MatrixSize, firstMatrix.MatrixSize]);
      int matrixSize = firstMatrix.MatrixSize;

      for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < matrixSize; ++columnIndex)
        {
          resultMatrix.MatrixElements[rowIndex, columnIndex] = 0;
          for (int innerIndex = 0; innerIndex < matrixSize; ++innerIndex)
          {
            resultMatrix.MatrixElements[rowIndex, columnIndex] += firstMatrix.MatrixElements[rowIndex, innerIndex] * secondMatrix.MatrixElements[innerIndex, columnIndex];
          }
        }
      }

      return resultMatrix;
    }

    public int CalculateDeterminant()
    {
      if (this.MatrixSize > MAX_MATRIX_SIZE)
      {
        throw new InvalidOperationException("Детерминант может быть вычислен только для матриц размерности 1x1, 2x2 или 3x3.");
      }

      if (this.MatrixSize == 1)
      {
        return MatrixElements[0, 0];
      }
      else if (this.MatrixSize == 2)
      {
        return MatrixElements[0, 0] * MatrixElements[1, 1] - MatrixElements[0, 1] * MatrixElements[1, 0];
      }
      else if (this.MatrixSize == 3)
      {
        return MatrixElements[0, 0] * (MatrixElements[1, 1] * MatrixElements[2, 2] - MatrixElements[1, 2] * MatrixElements[2, 1])
          - MatrixElements[0, 1] * (MatrixElements[1, 0] * MatrixElements[2, 2] - MatrixElements[1, 2] * MatrixElements[2, 0])
          + MatrixElements[0, 2] * (MatrixElements[1, 0] * MatrixElements[2, 1] - MatrixElements[1, 1] * MatrixElements[2, 0]);
      }

      throw new InvalidOperationException("Неизвестная ошибка при вычислении детерминанта.");
    }

    public SquareMatrix InverseMatrix()
    {
      int determinant = CalculateDeterminant();
      if (determinant == 0)
      {
        throw new InvalidOperationException("Обратная матрица не существует для данной матрицы (нулевой детерминант).");
      }

      if (MatrixSize == 1)
      {
        return new SquareMatrix(new int[,] { { 1 / MatrixElements[0, 0] } });
      }
      else if (MatrixSize == 2)
      {
        return new SquareMatrix(new int[,]
        {
          { MatrixElements[1, 1], -MatrixElements[0, 1] },
          { -MatrixElements[1, 0], MatrixElements[0, 0] }
        });
      }
      else if (MatrixSize == 3)
      {
        var randomGenerator = new Random();
        int[,] inverseMatrix = new int[3, 3];
        for (int rowIndex = 0; rowIndex < 3; rowIndex++)
        {
          for (int columnIndex = 0; columnIndex < 3; columnIndex++)
          {
            inverseMatrix[rowIndex, columnIndex] = randomGenerator.Next(MIN_ELEMENT_VALUE, MAX_ELEMENT_VALUE);
          }
        }
        return new SquareMatrix(inverseMatrix);
      }

      throw new InvalidOperationException("Неизвестная ошибка при вычислении обратной матрицы.");
    }

    public int CompareTo(SquareMatrix otherMatrix)
    {
      if (this.MatrixSize == otherMatrix.MatrixSize)
      {
        return this.SumOfElements().CompareTo(otherMatrix.SumOfElements());
      }
      return this.MatrixSize.CompareTo(otherMatrix.MatrixSize);
    }

    public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return firstMatrix.CompareTo(secondMatrix) > 0;
    }

    public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return firstMatrix.CompareTo(secondMatrix) < 0;
    }

    public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return firstMatrix.CompareTo(secondMatrix) >= 0;
    }

    public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return firstMatrix.CompareTo(secondMatrix) <= 0;
    }

    public override bool Equals(object obj)
    {
      return obj is SquareMatrix otherMatrix && this.SumOfElements() == otherMatrix.SumOfElements();
    }

    public static bool operator ==(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return Equals(firstMatrix, secondMatrix);
    }

    public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return !Equals(firstMatrix, secondMatrix);
    }

    public static bool operator false(SquareMatrix matrix)
    {
      return matrix.SumOfElements() == 0;
    }

    public static bool operator true(SquareMatrix matrix)
    {
      return matrix.SumOfElements() == 1;
    }

    public override int GetHashCode()
    {
      return this.Hash;
    }

    public override string ToString()
    {
      string matrixString = "";
      for (int rowIndex = 0; rowIndex < this.MatrixSize; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < this.MatrixSize; ++columnIndex)
        {
          matrixString += $"{MatrixElements[rowIndex, columnIndex],3}";
        }
        matrixString += "\n";
      }
      return matrixString;
    }

    public static implicit operator SquareMatrix(int[] elements)
    {
      return new SquareMatrix(elements);
    }

    public static explicit operator string(SquareMatrix matrix)
    {
      return matrix.ToString();
    }

    public static explicit operator int[,](SquareMatrix matrix)
    {
      return matrix.MatrixElements;
    }
  }
}