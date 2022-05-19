using System.Data;

namespace daliborBotNET;

public class Complex
{
    public readonly double Real;
    public readonly double Imaginary;
    
    public Complex(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }
    
    public double GetMagnitude()
    {
        return Math.Sqrt(Real * Real + Imaginary * Imaginary);
    }
    
    public static Complex Add(Complex a, Complex b)
    {
        return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }
    
    public static Complex Subtract(Complex a, Complex b)
    {
        return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
    }
    
    public static Complex Divide(Complex number1, Complex number2)
    {
        double real = (number1.Real * number2.Real + number1.Imaginary * number2.Imaginary) / (number2.Real * number2.Real + number2.Imaginary * number2.Imaginary);
        double imaginary = (number1.Imaginary * number2.Real - number1.Real * number2.Imaginary) / (number2.Real * number2.Real + number2.Imaginary * number2.Imaginary);
        return new Complex(real, imaginary);
    }

    public static Complex Multiply(Complex number1, Complex number2)
    {
        double real = number1.Real * number2.Real - number1.Imaginary * number2.Imaginary;
        double imaginary = number1.Real * number2.Imaginary + number1.Imaginary * number2.Real;
        return new Complex(real, imaginary);
    }

    public override string ToString()
    {
        return $"{Real} + {Imaginary}i";
    }
}