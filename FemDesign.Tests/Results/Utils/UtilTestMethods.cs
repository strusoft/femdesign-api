using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using FemDesign.Calculate;


namespace FemDesign.Results
{
    internal class UtilTestMethods
    {
        /// <summary>
        /// Open a model file, create result files in the current directory and calculate the number of results reading the .csv files.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface.</typeparam>
        /// <param name="modelPath">.str file path. The same directory has to contain the .strFEM file.</param>
        /// <returns>
        /// Number of result lines in each .csv file.<br />
        /// Headers in each .csv file.
        /// </returns>
        internal static (List<int> resultLines, List<string[]> headers, List<T> results) GetCsvParseData<T>(string modelPath) where T : IResult
        {
            // Get test file loacations
            string outDir = Directory.GetCurrentDirectory() + $"\\Results\\{typeof(T).Name}";
            var listProcs = (typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs ?? Enumerable.Empty<ListProc>()).ToList();
            var outPaths = listProcs.Select(l => OutputFileHelper.GetCsvPath(outDir, l.ToString())).ToList();

            // Get result files
            var results = new List<T>();
            using (var connection = new FemDesignConnection(minimized: true, outputDir: outDir, tempOutputDir: false))
            {
                connection.Open(modelPath);
                results = connection.GetResults<T>();
            }

            List<int> resultLines = new List<int>();
            List<string[]> headers = new List<string[]>();

            // Calculate count of results from file & get headers
            for (int i = 0; i < outPaths.Count; i++)
            {
                int multiplier = SetMultiplier(listProcs[i]);
                int rowCount = 0;
                int emptyRow = 0;
                int headerLength = SetHeaderLength(listProcs[i]);
                string[] header = new string[headerLength];

                using (var reader = new StreamReader(outPaths[i]))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();

                        if (row == "")
                        {
                            emptyRow++;
                        }
                        else if (rowCount < headerLength)
                        {
                            header[rowCount] = row;
                        }

                        rowCount++;
                    }
                }
                resultLines.Add(rowCount - emptyRow * multiplier);
                headers.Add(header);
            }

            // Remove test files
            Directory.Delete(outDir, true);

            return (resultLines, headers, results);
        }

        /// <summary>
        /// Auxiliary method for GetCsvParseData().
        /// </summary>
        /// <param name="listProc"></param>
        /// <returns></returns>
        private static int SetMultiplier(ListProc listProc)
        {
            if (listProc.ToString().StartsWith("QuantityEstimation"))
                return 5;

            switch(listProc)
            {
                case ListProc.FemBar:
                    return 3;
                case ListProc.FemShell:
                    return 3;
                default:
                    return 4;
            }
        }

        /// <summary>
        /// Auxiliary method for GetCsvParseData().
        /// </summary>
        /// <param name="listProc"></param>
        /// <returns></returns>
        private static int SetHeaderLength(ListProc listProc)
        {
            switch (listProc)
            {
                case ListProc.FemBar:
                    return 2;
                case ListProc.FemShell:
                    return 2;
                default:
                    return 3;
            }
        }
    }
}
