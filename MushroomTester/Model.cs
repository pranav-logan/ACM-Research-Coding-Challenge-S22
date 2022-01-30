// By: Pranav Logan (psl210000)
// Date: 1/30/2022
// Purpose: Generates a model using the genetic algorithm to guess the edibility of mushrooms
// Note: CSV needs to be in all uppercase + no title row (Requires .NET 6 to run)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushroomTester
{
    internal class Model
    {
        List<Double> weights = new List<double>();
        double personalScore = 0;

        static double currentBestScore = 0;
        static Model currentBestModel = null;
        static int generationsSinceLastChange = 0;
        static int mutationReset = 0;
        static double mutationRate = 0.05;
        
        static void Main()
        {
            System.Console.WriteLine(

                "This program generates models that try to determine mushroom edibility by starting with a random set of weights and \n" +
                "uses the genetic algorithm to find a model that works on >99.5% of the training data.\n\n" +
                "Please note that the program can take a while before it finds an optimal model (~5min). During this process the program will print when:\n" +
                "- a better model than the current is found \n" +
                "- it has been too long since a better model has been found and the mutation (change) rate has been increased\n" +
                "- the mutation rate has increased too much and is reset back to normal\n" +
                "- benchmark statistics (current highest score)\n" +
                "Note: Program automatically stops after 1500 iterations\n" +
                "(press any key to begin)\n"

         );

            Console.ReadKey();

            int modelSize = 150;
            int testDataSize = 3000;

            Model[] brain = new Model[modelSize];
            List<char[]> allMush = new List<char[]>(); //training set

            using (var reader = new StreamReader("mushrooms.csv"))
            {
                for(int i =0; i < testDataSize; i++) // read 500 mushrooms for training data
                {
                    var line = reader.ReadLine();
                    line = line.Replace(",", string.Empty);

                    char[] singleMush = line.ToCharArray();

                    allMush.Add(singleMush);
                }
                reader.Close();
            }

                for (int i = 0; i < brain.Length; i++)
                {
                    brain[i] = new Model();
                    brain[i].evaluateModel(allMush);
                }
            
            Console.WriteLine("Generated " + brain.Length + " models. Now training off data set.");
            Thread.Sleep(1000);

            int safety = 0;

            while(currentBestScore < testDataSize * 0.995)
            {
                brain = new Model[modelSize];
                for (int i = 0; i < brain.Length; i++)
                {
                    brain[i] = new Model(currentBestModel);
                    brain[i].evaluateModel(allMush);
                }

                safety++;
                generationsSinceLastChange++;

                if(safety > 1500)
                {
                    Console.WriteLine("NOTE: Unable to find a model that works 99.5% of the time in the alloted # of iterations. Continuing with current best model. (please try re-running)");
                    break;
                }
                if(safety % 50 == 0 || safety <= 10)
                {
                    Console.WriteLine("Benkmark: iteration " + safety + ". Current Best Score = " + currentBestScore + " out of " + testDataSize + ".");
                    Thread.Sleep(1000);
                }


                if(generationsSinceLastChange > 20)
                {
                    mutationRate *= 1.5;
                    Console.WriteLine("Mutation rate up");
                    mutationReset++;
                    generationsSinceLastChange = 0;
                }
                if(mutationReset > 10)
                {
                    Console.WriteLine("Mutation rate too high. Reseting back to default");
                    mutationReset = 0;
                    mutationRate = 0.05;
                }


            }

            Console.WriteLine("\nRESULTS: -------------------------");
            Console.WriteLine("Model found with score: " + currentBestScore + " out of " + testDataSize);
            Console.WriteLine("Weights: "); currentBestModel.printWeights();
            Console.WriteLine("------------------------------------\n");
            Console.WriteLine("(Press any key to try on full test data set)");

            Console.ReadKey();

            allMush.Clear();

            using (var reader = new StreamReader("mushrooms.csv"))
            {
                for (int i = 0; i < 8000; i++) // read all mushrooms from test data
                {
                    var line = reader.ReadLine();
                    line = line.Replace(",", string.Empty);

                    char[] singleMush = line.ToCharArray();

                    allMush.Add(singleMush);
                }
            }

            currentBestModel.evaluateModel(allMush);
            Console.WriteLine("Model gets a score of: " + currentBestModel.personalScore + " out of 8000 elements in the test data (" + (int) (currentBestModel.personalScore / 8000 *100) + "%).");

        }

        Model() // generate random weights  
        {
            Random random = new Random();
            Double[] sample = new double[22];
            Double sum = 0;
            for(int i = 0; i < sample.Length; i++)
            {
                sample[i] = random.NextDouble();
                sum += sample[i];
            }
            for(int i = 0; i<sample.Length; i++)
            {
                weights.Add(sample[i] / sum);
            }
        }

        Model(Model parent) // generate mutated version based off parent 
        {
            Random random = new Random();
            Double[] sample = new double[22];
            Double sum = 0;
            for (int i = 0; i < sample.Length; i++)
            {
                if (random.NextDouble() < 0.5)
                    sample[i] = parent.weights[i] + (random.NextDouble() * mutationRate); 
                else
                    sample[i] = parent.weights[i] - (random.NextDouble() * mutationRate); 
            
                sum += sample[i];
            }
            for (int i = 0; i < sample.Length; i++)
            {
                weights.Add(sample[i] / sum);
            }
        }

        Double evaluateModel(List<char[]> attributes)
        {
            Double score = 0;

            for(int i = 0;i < attributes.Count;i++)
            {
                double mushScore = computeWeights(attributes[i]);

                if(attributes[i][0] == 'P' && mushScore < 79.5)
                    score++;
                if (attributes[i][0] == 'E' && mushScore >= 79.5)
                    score++;

            }

            if(currentBestScore < score)
            {
                Console.WriteLine("New Model found");
                generationsSinceLastChange = 0;
                mutationRate = 0.05;
                mutationReset = 0;

                currentBestScore = score;
                currentBestModel = this;
            }

            personalScore = score;

            return score / attributes.Count;
        }

        Double computeWeights(char[] array)
        {
            double finalValue = 0;

            for(int i =1;i<array.Length;i++)
            {
                finalValue += weights[i-1] * array[i];
            }

            return finalValue;
        }

        void printWeights()
        {
            for(int i =0;i<weights.Count;i++)
            {
                Console.Write(weights[i] + " ");
            }
            Console.WriteLine();
        }

    }
}
