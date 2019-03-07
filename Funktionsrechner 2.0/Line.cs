using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    //class Line:Function
    //{

    //    public Line(double[] creatorParameters, int[] creatorExponents)
    //    {
    //        parameters = creatorParameters;
    //        exponents = creatorExponents;
    //    }

    //    public override string getFunctionType() { return "line"; }

    //    public override double calculateYValue(double x)
    //    {
    //        return parameters[0] * Math.Pow(x, exponents[0]) + parameters[1];
    //    }

    //    public override Function createDerivative()
    //    {
    //        double[] newParameters = new double [2];
    //        int[] newExponents = new int[2];
    //        newParameters[0] = parameters[0] * exponents[0];
    //        newExponents[0] = exponents[0] - 1;
    //        newParameters[1] = parameters[1] * exponents[1];
    //        newExponents[1] = exponents[1] - 1;

    //        Function derivative = new Line(parameters, exponents);
    //        derivative.primeCount++;
    //        derivative.name = createDerivativeName(primeCount, name);
    //        return derivative;
    //    }

    //    public override Function createIntegral()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override bool checkDerivationPossible()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override bool checkIntegrationPossible()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string printFunction()
    //    {
    //        string function="";
    //        function += name.ToString() + "(x) = ";
    //        if (parameters[0] != 0)
    //        {
    //            function += parameters[0].ToString() + "x";
    //        }
    //        else
    //        {
    //            if (parameters[1] != 0)
    //            {
    //                function += parameters[1].ToString();
    //            }
    //            else
    //            {
    //                function += "0";
    //            }
    //            return function;
    //        }

    //        if (parameters[1] != 0)
    //        {
    //            if (parameters[1] > 0)
    //            {
    //                function += " + " + parameters[1].ToString();
    //            }
    //            else
    //            {
    //                function += " - " + parameters[1].ToString();
    //            }
    //        }
    //        return function;

    //    }

    //    public override double[] calculateZeros()
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
}
