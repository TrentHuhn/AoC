using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day20
    {
        /// <summary>
        /// Day 20 - Trench Map
        /// </summary>
        public static void Run()
        {
            //  ------------------------ Part 1 ---------------------------------
            // 

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input20.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            if (lines.Length < 3 || string.IsNullOrEmpty(lines[0]))
                throw new Exception("Invalid input file.");

            var imgAlgorithm = lines[0].ToCharArray().Select(x => Equals(x,'.') ? false : true).ToArray();

            var inputImgJagged = lines.TakeLast(lines.Length - 2).Select(x => x.ToCharArray().Select(y => y == '.' ? false : true).ToArray()).ToArray();
            var inputImg = ConvertJaggedArray(inputImgJagged); // Will throw exception if input is not square

            var defaultEdgeCondition = false;
            var outputImage = EnhanceImage(inputImg, imgAlgorithm, defaultEdgeCondition);
            outputImage = EnhanceImage(outputImage, imgAlgorithm, defaultEdgeCondition ^ imgAlgorithm[0]);

            var numLitPixels = 0;
            for (var i = 0; i <= outputImage.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= outputImage.GetUpperBound(1); j++)
                {
                    if (outputImage[i, j])
                        numLitPixels++;
                }
            }

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {numLitPixels} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------
            //  


            start = DateTime.Now;

            outputImage = inputImg;
            for (int i = 1; i <= 50; i++)
            {
                outputImage = EnhanceImage(outputImage, imgAlgorithm, defaultEdgeCondition);
                defaultEdgeCondition = defaultEdgeCondition ^ imgAlgorithm[0]; // Use XOR logic to determine whether the default edge condition toggles each step
            }

            numLitPixels = 0;
            for (var i = 0; i <= outputImage.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= outputImage.GetUpperBound(1); j++)
                {
                    if (outputImage[i, j])
                        numLitPixels++;
                }
            }

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {numLitPixels} ({diff} ms)");

        }

        private static bool[,] EnhanceImage(bool[,] inputImg, bool[] imgAlgorithm, bool defaultEdgeCondition)
        {
            var outputImg = new bool[inputImg.GetUpperBound(0)+3, inputImg.GetUpperBound(1)+3]; // +1 to account for zero index, +2 to increase output image dimensions by 1 in each direction
            //PopulateArray(outputImg, false); // initially populate array with all FALSEs
            var arrMaxDim = outputImg.GetUpperBound(0);
            //var defaultEdgeCondition = imgAlgorithm[0]; // set the boundary condition based on what a value of [0 0 0 0 0 0 0 0 0] maps to

            for (int i = 0; i <= arrMaxDim; i++)
            {
                for (int j = 0; j <= arrMaxDim; j++)
                {
                    var pixel = new BitArray(9);

                    pixel[0] = i < 2 || j < 2 ? defaultEdgeCondition : inputImg[i - 2, j - 2];
                    pixel[1] = i < 2 || j == 0 || j == arrMaxDim ? defaultEdgeCondition : inputImg[i - 2, j - 1];
                    pixel[2] = i < 2 || j > arrMaxDim - 2 ? defaultEdgeCondition : inputImg[i - 2, j];

                    pixel[3] = i == 0 || i == arrMaxDim || j < 2 ? defaultEdgeCondition : inputImg[i - 1, j - 2];
                    pixel[4] = i == 0 || i == arrMaxDim || j == 0 || j == arrMaxDim ? defaultEdgeCondition : inputImg[i - 1, j - 1];
                    pixel[5] = i == 0 || i == arrMaxDim || j > arrMaxDim - 2 ? defaultEdgeCondition : inputImg[i - 1, j];

                    pixel[6] = i > arrMaxDim - 2 || j < 2 ? defaultEdgeCondition : inputImg[i, j - 2];
                    pixel[7] = i > arrMaxDim - 2 || j == 0 || j == arrMaxDim ? defaultEdgeCondition : inputImg[i, j - 1];
                    pixel[8] = i > arrMaxDim - 2 || j > arrMaxDim - 2 ? defaultEdgeCondition : inputImg[i, j];

                    var decimalVal = GetIntFromBitArray(pixel);

                    outputImg[i, j] = imgAlgorithm[decimalVal];

                }
            }
            return outputImg;
        }

        private static int GetIntFromBitArray(BitArray bitArray)
        {
            int value = 0;
            int count = bitArray.Count - 1;

            for (int i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i])
                    value += Convert.ToInt32(Math.Pow(2, count - i));
            }
            return value;
        }

        private static bool[,] ConvertJaggedArray(bool[][] jaggedArray)
        {
            // Convert jagged array to 2D array
            var newArray = new bool[jaggedArray.Length, jaggedArray.GroupBy(row => row.Length).Single().Key]; // throws InvalidOperationException if source is not rectangular
            for (int i = 0; i < jaggedArray.Length; ++i)
                for (int j = 0; j < jaggedArray.GroupBy(row => row.Length).Single().Key; ++j)
                    newArray[i, j] = jaggedArray[i][j];

            return newArray;
        }
    }
   

   
}

