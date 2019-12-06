using System;

namespace hw3_j_oneill
{
    class Polynomial
    {

        //Stores all the coefficients for the polynomial.
        //The index corresponds to the power of x
        //i.e: coefficients[n] gives the
        //coefficient for x^n
        private double[] coefficients;
        //Highest power in polynomial
        private int degree;
        //Max number of times root finding algorithm iterates
        private int maxiters;
        //Tolerance for finding roots
        private double tolerance;

        public Polynomial()
        {
            //Default constructor for polynomial class

            degree = 0;
            maxiters = 1000;
            tolerance = 0.000001;
            coefficients = new double[1];
        }
        public Polynomial(int degree):this()
        {
            //Overloaded constructor function
            //Calls default constructor function first,
            //then recreates array of coefficients of length "degree + 1"

            if (degree >= 0)
            {
                //Set member variable to argument "degree"
                this.degree = degree;
                //Create array of coefficients
                coefficients = new double[degree + 1];
                //And set all the entries to 0
                for (int i = 0; i < coefficients.Length; i++)
                    coefficients[i] = 0;
            }
        }
        public int Degree
        {
            //Create property for degree called Degree
            get
            {
                return degree;
            }
            set
            {
                //Only set new value for degree if it's non negative
                if(value >= 0)
                {
                    //Only create new array of coefficients, if the value of
                    //degree has changed, otherwise don't bother to create a
                    //new array.

                    if(value != degree)
                    {
                        degree = value;
                        //Create new array for coefficients
                        //and copy over data from old array

                        //Create temporary array
                        double[] oldCoefficients = coefficients;
                        //Create new array of coefficients
                        coefficients = new double[degree + 1];
                        //Find length of shortest array
                        int Length;
                        if (oldCoefficients.Length < coefficients.Length)
                            Length = oldCoefficients.Length;
                        else
                            Length = coefficients.Length;

                        //Copy elements over
                        for(int i = 0; i< Length; i++)
                        {
                            coefficients[i] = oldCoefficients[i];
                        }
                    }
                }
                
            }
        }
        public bool SetCoefficient(double val, int power)
        {
            //Sets coefficients of polynomial
            //power is used as an index for coefficients[]
            //so a power of 2, is stored in coefficients[2]

            //Check that parameters are in correct range
            if(0 <= power && power <=degree)
            {
                //parameters given are valid
                //Assign value as coefficient
                coefficients[power] = val;
                return true;
            }
            //paramters not in correct range, so return false
            return false;
        }
        public void evaluate(double x, ref double p, ref double q)
        {
            //Evaluates polynomial and its derivative at x
            //using Horner's Algorithm.

            //Use variables to store running total from algorithm
            double val = coefficients[coefficients.Length - 1];
            double deriv = val; ;

            //Loop backwards through list of coefficients
            //and update val and deriv
            for (int i = coefficients.Length -2; i > 0; i--)
            {
                val = x * val + coefficients[i];
                deriv = x * deriv + val;
            }
            val = x * val + coefficients[0];

            //Finally set p to q to val and deriv respectively
            //(Using reference types)

            p = val;   //Value of polynomial at point x
            q = deriv; //Derivative of polynomial at point x
        }
        public double FindRoot(double initguess)
        {
            //Uses Newton's method for finding roots
            //Takes an argument "initguess" as a starting guess
            //and then repeats the algorithm to converge on the root
            
            //p1 and p2 store the suceesive guesses for where the root is located
            //Algorithm starts with initial guess then becomes more
            //accurate with each iteration
            //Stop iterating once the differnce between p1 and p2 is less than the tolerance
            double p1 = initguess;
            double p2 = initguess;

            //temporary variables to store the evaluation of the polynomial
            //and its derivative at the point p1
            double value = 0.0 ;
            double deriv = 0.0;

            //Keep track of iterations of algorithm
            int count = 0;

            //Repeat the algorithm until the difference between successive guesses
            //is less than some tolerance
            //We want our algorithm to run at least once, so a "do while" loop is used
            do
            {
                //Check if algorithm has iterated too many times
                if (count > maxiters)
                {
                    Console.WriteLine("ERROR IN FINDING ROOTS OF FUNCTION: MAX NUMBER OF ITERATIONS EXCEEDED\n");
                    break;
                
                }
                //take old value for p2 and store it in p1 for fresh interation
                p1 = p2;
                //Evaluate polynomial and its derivative at current guess p1
                evaluate(p1, ref value, ref deriv);
                //Check if derivative is becoming too close to 0
                //to avoid dividing by 0 during algorithm
                if ( Math.Abs(deriv) < 1.0E-6)
                {
                    Console.WriteLine("ERROR IN FINDING ROOTS OF FUNCTION: DERIVATIVE TOO CLOSE TO ZERO\n");
                    break;
                }
                //Generate new guess p2
                p2 = p1 - (value) / (deriv);

                //count iterations
                count++;
            }
            while (Math.Abs(p2 - p1) > tolerance); //Check if the points are still far enough apart to keep searching for a root

            //Return the value stored in p2, as this will be the value closest to the root
            return p2;
        }
        
    }
    class Test
    {
        static void Main(string[] args)
        {
            
            //Test root finding function
            //Create polynomial of degree 3
            Polynomial poly = new Polynomial(3);
            //Set coefficients to make polynomial:
            // -2x^3 - 3x^2 + 2x + 1
            poly.SetCoefficient(1.0, 0);
            poly.SetCoefficient(2.0, 1);
            poly.SetCoefficient(-3.0, 2);
            poly.SetCoefficient(-2.0, 3);

            //Output text to user in console
            Console.WriteLine("This program demonstrates Newton's Method for finding roots of a polynomial.\n");
            Console.WriteLine("The program will find the roots of:");
            Console.WriteLine("-2x^3 - 3x^2 + 2x + 1 = 0\n");
            Console.WriteLine("The result are:\n");

            //Now, solve for roots of
            //-2x^3 - 3x^2 + 2x + 1 = 0

            //Set up variables to store roots and initial guesses
            double guess;
            double root1 = 0.0;
            double root2 = 0.0;
            double root3 = 0.0;
            
            //FIRST ROOT
            //Take an initial guess
            guess = -5.0;
            //Find the root using Newton's method
            root1 = poly.FindRoot(guess);
            //output results to console
            Console.WriteLine("FIRST ROOT:");
            Console.WriteLine("Initial guess is :{0}", guess);
            Console.WriteLine("root 1 = {0:0.0000}\n", root1);

            //SECOND ROOT
            //Take an initial guess
            guess = 0.0;
            //Find the root using Newton's method
            root2 = poly.FindRoot(guess);
            //output results to console
            Console.WriteLine("SECOND ROOT:");
            Console.WriteLine("Initial guess is :{0}", guess);
            Console.WriteLine("root 2 = {0:0.0000}\n", root2);

            //THIRD ROOT
            //Take an initial guess
            guess = 5.0;
            //Find the root using Newton's method
            root3 = poly.FindRoot(guess);
            //output results to console
            Console.WriteLine("THIRD ROOT:");
            Console.WriteLine("Initial guess is :{0}", guess);
            Console.WriteLine("root 3 = {0:0.0000}\n", root3);

            //Pause for user to consider results
            Console.WriteLine("Press Enter to continue:");
            Console.ReadLine();
        }
    }
}
