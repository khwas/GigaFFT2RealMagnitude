using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GigaFFT2RealMagnitude
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"d:\Temp\0e189873-9e52-43cc-be03-247c70cbe03f-1";
            MemoryMappedFile file = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            long bytesCount = new System.IO.FileInfo(path).Length;
            long structCount = (bytesCount / 16) / 2;
            long bin = 0;
            double inverseSqrtN = 1 / Math.Sqrt(structCount * 2);
            double minM = double.MaxValue;
            double maxM = double.MinValue;
            for (int index = 0; index < structCount / 4096; index++)
            {
                Complex[] array = new Complex[4096];
                using (MemoryMappedViewAccessor accessor = file.CreateViewAccessor(index * 4096 * 16L, 4096 * 16L))
                {
                    accessor.ReadArray<Complex>(0, array, 0, array.Length);
                }
                for (int i = 0; i < array.Length; i++)
                {
                    double x = inverseSqrtN * array[i].Real;
                    minM = Math.Min(minM, x);
                    maxM = Math.Max(maxM, x);
                    bin++;
                    if ((bin % (4 * 1024)) == 0)
                    {
                        Console.WriteLine(minM + "," + maxM);
                        minM = double.MaxValue;
                        maxM = double.MinValue;
                    }
                }
            }

        }
    }
}
