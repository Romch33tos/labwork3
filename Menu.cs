using System;

namespace Matrix
{
  class Program
  {
    static void Main(string[] args)
    {
      bool isRunning = true;
      string userChoice;

      while (isRunning)
      {
        Console.Write("\nВыбор действий:\n1 - Демонстрация работы программы\n2 - Выполнение операций над матрицами\n0 - Выход\nВведите число: ");
        userChoice = Console.ReadLine();

        switch (userChoice)
        {
          case "0":
            isRunning = false;
            break;
          case "1":
            ShowTest();
            break;
          case "2":
            PerformMatrixOperations();
            break;
          default:
            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
            break;
        }
      }
    }

    static void PerformMatrixOperations()
    {
      bool isCalculating = true;
      string operationChoice;

      Console.Write("Введите размерность для первой матрицы (от 1 до 3): ");
      if (!int.TryParse(Console.ReadLine(), out int firstMatrixSize) || firstMatrixSize < 1 || firstMatrixSize > 3)
      {
        Console.WriteLine("Некорректная размерность матрицы. Размерность должна быть от 1 до 3.");
        return;
      }
      SquareMatrix firstMatrix = new SquareMatrix(GetMatrixElements(firstMatrixSize));
      Console.WriteLine(firstMatrix.ToString());

      Console.Write("Введите размерность для второй матрицы (от 1 до 3): ");
      if (!int.TryParse(Console.ReadLine(), out int secondMatrixSize) || secondMatrixSize < 1 || secondMatrixSize > 3)
      {
        Console.WriteLine("Некорректная размерность матрицы. Размерность должна быть от 1 до 3.");
        return;
      }
      SquareMatrix secondMatrix = new SquareMatrix(GetMatrixElements(secondMatrixSize));
      Console.WriteLine(secondMatrix.ToString());

      while (isCalculating)
      {
        Console.Write("\nСписок операций:\n1 - Сложение\n2 - Вычитание\n3 - Умножение\n4 - Вычисление детерминанта\n5 - Вычисление обратной матрицы\n6 - Сравнение матриц\n0 - Выйти в главное меню\nВведите число: ");
        operationChoice = Console.ReadLine();

        switch (operationChoice)
        {
          case "0":
            isCalculating = false;
            break;
          case "1":
            try
            {
              Console.WriteLine("\nРезультат операции сложения:");
              SquareMatrix sumResult = firstMatrix + secondMatrix;
              Console.WriteLine(sumResult.ToString());
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(ex.Message);
            }
            break;
          case "2":
            try
            {
              Console.WriteLine("\nРезультат операции вычитания:");
              SquareMatrix subtractionResult = firstMatrix - secondMatrix;
              Console.WriteLine(subtractionResult.ToString());
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(ex.Message);
            }
            break;
          case "3":
            try
            {
              Console.WriteLine("\nРезультат операции умножения:");
              SquareMatrix multiplicationResult = firstMatrix * secondMatrix;
              Console.WriteLine(multiplicationResult.ToString());
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(ex.Message);
            }
            break;
          case "4":
            try
            {
              Console.WriteLine($"\nДетерминант первой матрицы: {firstMatrix.CalculateDeterminant()}");
              Console.WriteLine($"Детерминант второй матрицы: {secondMatrix.CalculateDeterminant()}");
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(ex.Message);
            }
            break;
          case "5":
            try
            {
              SquareMatrix inverseFirst = firstMatrix.InverseMatrix();
              Console.WriteLine("\nМатрица, обратная для первой:");
              Console.WriteLine(inverseFirst.ToString());

              SquareMatrix inverseSecond = secondMatrix.InverseMatrix();
              Console.WriteLine("\nМатрица, обратная для второй:");
              Console.WriteLine(inverseSecond.ToString());
            }
            catch (InvalidOperationException ex)
            {
              Console.WriteLine(ex.Message);
            }
            break;
          case "6":
            CompareMatrices(firstMatrix, secondMatrix);
            break;
          default:
            Console.WriteLine("Данная операция не может быть реализована.");
            break;
        }
      }
    }

    static void CompareMatrices(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      Console.WriteLine("\nРезультаты сравнения:");
      Console.WriteLine($"Первая матрица > Вторая матрица: {firstMatrix > secondMatrix}");
      Console.WriteLine($"Первая матрица < Вторая матрица: {firstMatrix < secondMatrix}");
      Console.WriteLine($"Первая матрица >= Вторая матрица: {firstMatrix >= secondMatrix}");
      Console.WriteLine($"Первая матрица <= Вторая матрица: {firstMatrix <= secondMatrix}");
      Console.WriteLine($"Первая матрица == Вторая матрица: {firstMatrix == secondMatrix}");
      Console.WriteLine($"Первая матрица != Вторая матрица: {firstMatrix != secondMatrix}");
    }

    static int[] GetMatrixElements(int matrixSize)
    {
      int[] elementsArray = new int[matrixSize * matrixSize];
      for (int elementIndex = 0; elementIndex < matrixSize * matrixSize; ++elementIndex)
      {
        Console.Write($"Введите {elementIndex + 1}-й элемент матрицы: ");
        while (!int.TryParse(Console.ReadLine(), out elementsArray[elementIndex]))
        {
          Console.WriteLine("Некорректный ввод. Попробуйте снова.");
        }
      }
      return elementsArray;
    }

    static void ShowTest()
    {
      bool isRunning = true;

      while (isRunning)
      {
        Console.WriteLine("\nВыбор действий для демонстрации работы программы:");
        Console.WriteLine("1 - Создать случайные матрицы A и B");
        Console.WriteLine("2 - Вычислить детерминанты матриц A и B");
        Console.WriteLine("3 - Вычислить обратные матрицы A и B");
        Console.WriteLine("4 - Выполнить операции сравнения между матрицами A и B");
        Console.WriteLine("5 - Продемонстрировать приведение типов");
        Console.WriteLine("0 - Вернуться в главное меню");
        Console.Write("Введите число: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
          case "0":
            isRunning = false;
            break;
          case "1":
            ShowRandomMatrices();
            break;
          case "2":
            CalculateDeterminants();
            break;
          case "3":
            CalculateInverseMatrices();
            break;
          case "4":
            PerformComparisons();
            break;
          case "5":
            ShowTypeConversions();
            break;
          default:
            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
            break;
        }
      }
    }

    static SquareMatrix randomMatrixA;
    static SquareMatrix randomMatrixB;

    static void ShowRandomMatrices()
    {
      Console.WriteLine("\nСоздание первой случайной матрицы размерности 3x3:");
      randomMatrixA = new SquareMatrix();
      randomMatrixA.AutoFill(3);
      Console.Write(randomMatrixA.ToString());

      Console.WriteLine("\nСоздание второй случайной матрицы размерности 3x3:");
      randomMatrixB = new SquareMatrix();
      randomMatrixB.AutoFill(3);
      Console.Write(randomMatrixB.ToString());
    }

    static void CalculateDeterminants()
    {
      if (randomMatrixA == null || randomMatrixB == null)
      {
        Console.WriteLine("Сначала создайте случайные матрицы.");
        return;
      }

      try
      {
        int determinantA = randomMatrixA.CalculateDeterminant();
        int determinantB = randomMatrixB.CalculateDeterminant();
        Console.WriteLine($"\nДетерминант первой матрицы: {determinantA}");
        Console.WriteLine($"Детерминант второй матрицы: {determinantB}");
      }
      catch (InvalidOperationException ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    static void CalculateInverseMatrices()
    {
      if (randomMatrixA == null || randomMatrixB == null)
      {
        Console.WriteLine("Сначала создайте случайные матрицы.");
        return;
      }

      try
      {
        SquareMatrix inverseA = randomMatrixA.InverseMatrix();
        SquareMatrix inverseB = randomMatrixB.InverseMatrix();
        Console.WriteLine("\nМатрица, обратная первой:");
        Console.Write(inverseA.ToString());
        Console.WriteLine("\nМатрица, обратная второй:");
        Console.Write(inverseB.ToString());
      }
      catch (InvalidOperationException ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    static void PerformComparisons()
    {
      if (randomMatrixA == null || randomMatrixB == null)
      {
        Console.WriteLine("Сначала создайте случайные матрицы.");
        return;
      }

      Console.WriteLine("\nОперации сравнения:");
      Console.WriteLine($"Вторая матрица > Первая матрица: {randomMatrixB > randomMatrixA}");
      Console.WriteLine($"Первая матрица < Вторая матрица: {randomMatrixA < randomMatrixB}");
      Console.WriteLine($"Первая матрица == Вторая матрица: {randomMatrixA == randomMatrixB}");
      Console.WriteLine($"Вторая матрица >= Первая матрица: {randomMatrixB >= randomMatrixA}");
      Console.WriteLine($"Первая матрица <= Вторая матрица: {randomMatrixA <= randomMatrixB}");
    }

    static void ShowTypeConversions()
    {
      if (randomMatrixA == null)
      {
        Console.WriteLine("Сначала создайте случайную матрицу A.");
        return;
      }

      Console.WriteLine("\nПриведение типов:");
      Console.WriteLine("\nМатрица A -> двумерный массив:");
      int[,] matrixToArray = (int[,])randomMatrixA;
      for (int rowIndex = 0; rowIndex < matrixToArray.GetLength(0); rowIndex++)
      {
        for (int columnIndex = 0; columnIndex < matrixToArray.GetLength(1); columnIndex++)
        {
          Console.Write(matrixToArray[rowIndex, columnIndex] + " ");
        }
        Console.WriteLine();
      }

      Console.WriteLine("\nМатрица A -> строка:");
      string matrixToString = (string)randomMatrixA;
      Console.WriteLine(matrixToString);

      Console.WriteLine("\nОдномерный массив -> матрица:");
      int[] arrayToMatrix = new int[4] { 0, 1, 2, 3 };
      SquareMatrix newMatrix = arrayToMatrix;
      Console.Write(newMatrix.ToString());
    }
  }
}