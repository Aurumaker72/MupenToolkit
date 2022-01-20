using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MupenToolkit.Core.Helper
{
    // thank you eric
    // https://stackoverflow.com/users/452845/eric-ouellet
    public static class ClipboardHelper
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = null;
            object clipboardRawData = null;
            ParseFormat parseFormat = null;

            IDataObject dataObj = System.Windows.Clipboard.GetDataObject();
            if ((clipboardRawData = dataObj.GetData(DataFormats.CommaSeparatedValue)) != null)
            {
                parseFormat = ParseCsvFormat;
            }
            else if ((clipboardRawData = dataObj.GetData(DataFormats.Text)) != null)
            {
                parseFormat = ParseTextFormat;
            }

            if (parseFormat != null)
            {
                string rawDataStr = clipboardRawData as string;

                if (rawDataStr == null && clipboardRawData is MemoryStream)
                {
                    MemoryStream ms = clipboardRawData as MemoryStream;
                    StreamReader sr = new StreamReader(ms);
                    rawDataStr = sr.ReadToEnd();
                }
                Debug.Assert(rawDataStr != null, string.Format("clipboardRawData: {0}, could not be converted to a string or memorystream.", clipboardRawData));

                string[] rows = rawDataStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (rows != null && rows.Length > 0)
                {
                    clipboardData = new List<string[]>();
                    foreach (string row in rows)
                    {
                        clipboardData.Add(parseFormat(row));
                    }
                }
                else
                {
                    Debug.WriteLine("unable to parse row data.  possibly null or contains zero rows.");
                }
            }

            return clipboardData;
        }

        public static string[] ParseCsvFormat(string value)
        {
            return ParseCsvOrTextFormat(value, true);
        }

        public static string[] ParseTextFormat(string value)
        {
            return ParseCsvOrTextFormat(value, false);
        }

        private static string[] ParseCsvOrTextFormat(string value, bool isCSV)
        {
            List<string> outputList = new List<string>();

            char separator = isCSV ? ',' : '\t';
            int startIndex = 0;
            int endIndex = 0;

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];
                if (ch == separator)
                {
                    outputList.Add(value.Substring(startIndex, endIndex - startIndex));

                    startIndex = endIndex + 1;
                    endIndex = startIndex;
                }
                else if (ch == '\"' && isCSV)
                {
                    // skip until the ending quotes
                    i++;
                    if (i >= value.Length)
                    {
                        throw new FormatException(string.Format("value: {0} had a format exception", value));
                    }
                    char tempCh = value[i];
                    while (tempCh != '\"' && i < value.Length)
                        i++;

                    endIndex = i;
                }
                else if (i + 1 == value.Length)
                {
                    // add the last value
                    outputList.Add(value.Substring(startIndex));
                    break;
                }
                else
                {
                    endIndex++;
                }
            }

            return outputList.ToArray();
        }
    }

    public class CsvHelper
    {
        public static Dictionary<LineSeparator, Func<string, string[]>> DictionaryOfLineSeparatorAndItsFunc = new Dictionary<LineSeparator, Func<string, string[]>>();

        static CsvHelper()
        {
            DictionaryOfLineSeparatorAndItsFunc[LineSeparator.Unknown] = ParseLineNotSeparated;
            DictionaryOfLineSeparatorAndItsFunc[LineSeparator.Tab] = ParseLineTabSeparated;
            DictionaryOfLineSeparatorAndItsFunc[LineSeparator.Semicolon] = ParseLineSemicolonSeparated;
            DictionaryOfLineSeparatorAndItsFunc[LineSeparator.Comma] = ParseLineCommaSeparated;
        }

        // ******************************************************************
        public enum LineSeparator
        {
            Unknown = 0,
            Tab,
            Semicolon,
            Comma
        }

        // ******************************************************************
        public static LineSeparator GuessCsvSeparator(string oneLine)
        {
            List<Tuple<LineSeparator, int>> listOfLineSeparatorAndThereFirstLineSeparatedValueCount = new List<Tuple<LineSeparator, int>>();

            listOfLineSeparatorAndThereFirstLineSeparatedValueCount.Add(new Tuple<LineSeparator, int>(LineSeparator.Tab, CsvHelper.ParseLineTabSeparated(oneLine).Count()));
            listOfLineSeparatorAndThereFirstLineSeparatedValueCount.Add(new Tuple<LineSeparator, int>(LineSeparator.Semicolon, CsvHelper.ParseLineSemicolonSeparated(oneLine).Count()));
            listOfLineSeparatorAndThereFirstLineSeparatedValueCount.Add(new Tuple<LineSeparator, int>(LineSeparator.Comma, CsvHelper.ParseLineCommaSeparated(oneLine).Count()));

            Tuple<LineSeparator, int> bestBet = listOfLineSeparatorAndThereFirstLineSeparatedValueCount.MaxBy((n) => n.Item2);

            if (bestBet != null && bestBet.Item2 > 1)
            {
                return bestBet.Item1;
            }

            return LineSeparator.Unknown;
        }

        // ******************************************************************
        public static string[] ParseLineCommaSeparated(string line)
        {
            // CSV line parsing : From "jgr4" in http://www.kimgentes.com/worshiptech-web-tools-page/2008/10/14/regex-pattern-for-parsing-csv-files-with-embedded-commas-dou.html
            var matches = Regex.Matches(line, @"\s?((?<x>(?=[,]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^,]+)),?",
                                        RegexOptions.ExplicitCapture);

            string[] values = (from Match m in matches
                               select m.Groups["x"].Value.Trim().Replace("\"\"", "\"")).ToArray();

            return values;
        }

        // ******************************************************************
        public static string[] ParseLineTabSeparated(string line)
        {
            //var matchesTab = Regex.Matches(line, @"\s?((?<x>(?=[\t]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^\t]+))\t?",
            //                RegexOptions.ExplicitCapture);

            // Correction according to Cesar Morigaki from SO question: https://stackoverflow.com/questions/4118617/wpf-datagrid-pasting/5436437?noredirect=1#comment115043404_5436437
            var matchesTab = Regex.Matches(line, @"[\r\n\f\v ]*?((?<x>(?=[\t]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^\t]+))\t?",
                 RegexOptions.ExplicitCapture);

            string[] values = (from Match m in matchesTab
                               select m.Groups["x"].Value.Trim().Replace("\"\"", "\"")).ToArray();

            return values;
        }

        // ******************************************************************
        public static string[] ParseLineSemicolonSeparated(string line)
        {
            // CSV line parsing : From "jgr4" in http://www.kimgentes.com/worshiptech-web-tools-page/2008/10/14/regex-pattern-for-parsing-csv-files-with-embedded-commas-dou.html
            var matches = Regex.Matches(line, @"\s?((?<x>(?=[;]+))|""(?<x>([^""]|"""")+)""|""(?<x>)""|(?<x>[^;]+));?",
                                        RegexOptions.ExplicitCapture);

            string[] values = (from Match m in matches
                               select m.Groups["x"].Value.Trim().Replace("\"\"", "\"")).ToArray();

            return values;
        }

        // ******************************************************************
        public static string[] ParseLineNotSeparated(string line)
        {
            string[] lineValues = new string[1];
            lineValues[0] = line;
            return lineValues;
        }

        // ******************************************************************
        public static List<string[]> ParseText(string text)
        {
            string[] lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            return ParseString(lines);
        }

        // ******************************************************************
        public static List<string[]> ParseString(string[] lines)
        {
            List<string[]> result = new List<string[]>();

            LineSeparator lineSeparator = LineSeparator.Unknown;
            if (lines.Any())
            {
                lineSeparator = GuessCsvSeparator(lines[0]);
            }

            Func<string, string[]> funcParse = DictionaryOfLineSeparatorAndItsFunc[lineSeparator];

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                result.Add(funcParse(line));
            }

            return result;
        }

        // ******************************************************************
    }

    public static class ClipboardHelper2
    {
        public delegate string[] ParseFormat(string value);

        public static List<string[]> ParseClipboardData()
        {
            List<string[]> clipboardData = new List<string[]>();

            // get the data and set the parsing method based on the format
            // currently works with CSV and Text DataFormats            
            IDataObject dataObj = System.Windows.Clipboard.GetDataObject();

            if (dataObj != null)
            {
                string[] formats = dataObj.GetFormats();
                if (formats.Contains(DataFormats.CommaSeparatedValue))
                {
                    string clipboardString = (string)dataObj.GetData(DataFormats.CommaSeparatedValue);
                    {
                        // EO: Subject to error when a CRLF is included as part of the data but it work for the moment and I will let it like it is
                        // WARNING ! Subject to errors
                        string[] lines = clipboardString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        string[] lineValues;
                        foreach (string line in lines)
                        {
                            lineValues = CsvHelper.ParseLineCommaSeparated(line);
                            if (lineValues != null)
                            {
                                clipboardData.Add(lineValues);
                            }
                        }
                    }
                }
                else if (formats.Contains(DataFormats.Text))
                {
                    string clipboardString = (string)dataObj.GetData(DataFormats.Text);
                    clipboardData = CsvHelper.ParseText(clipboardString);
                }
            }

            return clipboardData;
        }
    }

}
