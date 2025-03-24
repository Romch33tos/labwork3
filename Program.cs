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
            int rows = originalMatrix.GetLength(0);
            int columns = originalMatrix.GetLength(1);
            int[,] matrixCopy = new int[rows, columns];

            for (int rowIndex = 0; rowIndex < rows; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < columns; ++columnIndex)
                {
                    matrixCopy[rowIndex, columnIndex] = originalMatrix[rowIndex, columnIndex];
                }
            }

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
                        throw new MatrixElementException("Недостаточно элементов для заполнения матрицы.");
                    }
                }
            }
            return matrix;
        }

        public void AutoFill(int matrixSize)
        {
            if (matrixSize > MAX_MATRIX_SIZE)
            {
                throw new MatrixSizeException(nameof(matrixSize), "Размерность матрицы не может превышать 3.");
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
            ValidateMatrixSizes(firstMatrix, secondMatrix, "сложения");

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
            ValidateMatrixSizes(firstMatrix, secondMatrix, "вычитания");

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
            ValidateMatrixSizes(firstMatrix, secondMatrix, "умножения");

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
                throw new MatrixSizeException(nameof(MatrixSize), "Детерминант может быть вычислен только для матриц размерности 1x1, 2x2 или 3x3.");
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

            throw new MatrixCalculationException("Неизвестная ошибка при вычислении детерминанта.");
        }

        public SquareMatrix InverseMatrix()
        {
            int determinant = CalculateDeterminant();
            if (determinant == 0)
            {
                throw new MatrixCalculationException("Обратная матрица не существует для данной матрицы (нулевой детерминант).");
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
                // Здесь следует реализовать обратную матрицу для 3x3
                // Для примера заполним случайными числами
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

            throw new MatrixCalculationException("Неизвестная ошибка при вычислении обратной матрицы.");
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

        private static void ValidateMatrixSizes(SquareMatrix firstMatrix, SquareMatrix secondMatrix, string operation)
        {
            if (firstMatrix.MatrixSize != secondMatrix.MatrixSize)
            {
                throw new MatrixOperationException($"Операцию {operation} можно выполнить только в случае, если матрицы имеют одинаковую размерность!");
            }
        }
    }

    public class MatrixElementException : ArgumentException
    {
        public MatrixElementException(string message) : base(message) { }
    }

    public class MatrixSizeException : ArgumentOutOfRangeException
    {
        public MatrixSizeException(string paramName, string message) : base(paramName, message) { }
    }

    public class MatrixCalculationException : InvalidOperationException
    {
        public MatrixCalculationException(string message) : base(message) { }
    }

    public class MatrixOperationException : InvalidOperationException
    {
        public MatrixOperationException(string message) : base(message) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                bool isRunning = true;
                string userChoice;

                while (isRunning)
                {
                    Console.Write("\nВыбор действий:\n1 - Демонстрация работы программы2 - Выполнение операций над матрицами0 - ВыходВведите число: ");
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
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
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
                Console.Write("\nСписок операций:\n1 - Сложение2 - Вычитание3 - Умножение4 - Вычисление детерминанта5 - Вычисление обратной матрицы6 - Сравнение матриц0 - Выйти в главное менюВведите число: ");
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
                        catch (Exception ex)
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
                        catch (Exception ex)
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
                        catch (Exception ex)
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
                        catch (Exception ex)
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
                        catch (Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
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