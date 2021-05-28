using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace FemDesign.Results
{
    /// <summary>
    /// Reads FEM-Design results from list tables text files.
    /// </summary>
    public class ResultsReader : CsvReader
    {
        public FemDesign.Calculate.ResultType ResultType;
        private Regex HeaderExpression = new Regex(@"(?'type'[\w\ ]+), (?'result'[\w\ ]+), (?'loadcasetype'[\w\ ]+) - Load (?'casecomb'[\w\ ]+): (?'casename'[\w\ ]+)");
        private Regex IsHeaderExpression = new Regex(@"([\w\ ,-]+|(ID)|(\[.*\])");
        private Regex LabelledSectionIdExpresstion = new Regex(@"(?'id'.*\.\d+)( - Part (?'part'\d+))?");
        public ResultsReader(string filePath) : base(filePath, delimiter: '\t')
        {

        }

        private PointSupportReaction parsePointSupportReaction(string[] row, CsvReader reader)
        {
            string supportname = row[0];
            double x = Double.Parse(row[1], CultureInfo.InvariantCulture);
            double y = Double.Parse(row[2], CultureInfo.InvariantCulture);
            double z = Double.Parse(row[3], CultureInfo.InvariantCulture);
            int nodeId = int.Parse(row[4], CultureInfo.InvariantCulture);
            double fx = Double.Parse(row[5], CultureInfo.InvariantCulture);
            double fy = Double.Parse(row[6], CultureInfo.InvariantCulture);
            double fz = Double.Parse(row[7], CultureInfo.InvariantCulture);
            double mx = Double.Parse(row[8], CultureInfo.InvariantCulture);
            double my = Double.Parse(row[9], CultureInfo.InvariantCulture);
            double mz = Double.Parse(row[10], CultureInfo.InvariantCulture);
            double fr = Double.Parse(row[11], CultureInfo.InvariantCulture);
            double mr = Double.Parse(row[12], CultureInfo.InvariantCulture);
            string lc = row[13];
            return new PointSupportReaction(supportname, x, y, z, nodeId, fx, fy, fz, mx, my, mz, fr, mr, lc);
        }

        private bool parseHeaderDefault(string line, CsvReader reader)
        {
            bool isHeader = IsHeaderExpression.IsMatch(line);
            if (isHeader)
                Header = line;
            return isHeader;
        }

        protected sealed override void BeforeParse(Type type)
        {
            HeaderParser = parseHeaderDefault;

            if (type == typeof(Results.PointSupportReaction))
                RowParser = parsePointSupportReaction;
            else
                throw new NotImplementedException($"Parser for {type} has not yet been implemented.");
            
            base.BeforeParse(type);
        }
    }

    /// <summary>
    /// Base class for ResultsReader
    /// </summary>
    public class CsvReader
    {
        public StreamReader File;
        private char Delimiter;
        protected Func<string, CsvReader, bool> HeaderParser;
        protected Func<string[], CsvReader, object> RowParser;
        /// <summary>
        /// Last header row read by the header parser.
        /// </summary>
        internal string Header;
        public bool IsDone { get { return File.Peek() == -1; } }

        protected CsvReader(string filePath, char delimiter = ',', Func<string[], CsvReader, object> rowParser = null, Func<string, CsvReader, bool> headerParser = null)
        {
            File = new System.IO.StreamReader(filePath);
            Delimiter = delimiter;
            RowParser = rowParser;
            HeaderParser = headerParser;
        }

        protected string[] ReadRow()
        {
            string line = File.ReadLine();
            bool isHeader = false;
            if (HeaderParser != null)
            {
                isHeader = HeaderParser(line, this);
            }
            return isHeader || string.IsNullOrEmpty(line) ? null : line.Split(new char[] { Delimiter });
        }

        protected T ParseRow<T>()
        {
            var row = ReadRow();
            return row == null ? default(T) : (T)RowParser(row, this);
        }

        public List<T> ParseAll<T>(bool skipNull = true)
        {
            BeforeParse(typeof(T));

            List<T> results = new List<T>();
            while (!this.IsDone)
            {
                T parsed = ParseRow<T>();
                if (skipNull && parsed == null)
                    continue;
                results.Add(parsed);
            }
            return results;
        }

        protected virtual void BeforeParse(Type type)
        {
            if (RowParser == null)
                throw new ApplicationException("Row parser was not initialized properly.");
            if (HeaderParser == null)
                throw new ApplicationException("Header parser was not initialized properly.");
        }
    }
}
