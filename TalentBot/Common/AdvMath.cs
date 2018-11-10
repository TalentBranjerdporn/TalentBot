using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TalentBot.Common
{
    class AdvMath
    {
        public static double factorial(int num)
        {
            if (num <= 1)
            {
                return 1;
            }
            return num * factorial(num - 1);
        }

        public static double nck(int n, int k)
        {
            return factorial(n) / (factorial(k) * factorial(n - k));
        }

        public static double multihypergeo(int[] x, int N, int n, int[] k)
        {
            if (x.Length != k.Length)
                return -1;

            double ret = 1;
            int Nt = N;
            int nt = n;
            for (int i = 0; i < x.Length; i++)
            {
                ret *= nck(k[i], x[i]);
                Nt -= k[i];
                nt -= x[i];
            }

            ret *= nck(Nt, nt);

            return ret / nck(N,n);
        }

        /*
        N is population
        k is number of successes
        n is the number of items in the sample
        x the number of items in the sample that are successes
        */
        public static double hypergeometric(int x, int N, int n, int k)
        {
            return nck(k, x) * nck(N - k, n - x) / nck(N, n);
        }
    }
}
