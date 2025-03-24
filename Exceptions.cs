using System;

namespace Matrix
{
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
}