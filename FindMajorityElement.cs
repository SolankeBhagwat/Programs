using System;

namespace ProgramNamespace
{
    public class FindMajorityElement
    {
        // Driver Code 
        public static void Main(String[] args)
        {
            var numberOfTestCases = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < numberOfTestCases; i++)
            {
                var input = Console.ReadLine().Trim().Split(' ');
                var array = new int[input.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    array[j] = Convert.ToInt32(input[j]);
                }

                var output = FindMajorityElementInArray(array, input.Length);

                Console.WriteLine(output);
            }
            Console.ReadLine();
        }

        private static int FindMajorityElementInArray(int[] array, int length)
        {
            int votes = 0, candidate = -1;

            for (int i = 0; i < length; i++)
            {
                if (votes == 0)
                {
                    candidate = array[i];
                }
                if (candidate == array[i])
                {
                    votes++;
                }
                else
                {
                    votes--;
                }
            }
            votes = 0;
            for (int i = 0; i < length; i++)
            {
                if (candidate == array[i])
                {
                    votes++;
                }
                if (votes > (length / 2))
                {
                    return candidate;
                }
            }

            return -1;
        }
    }
}