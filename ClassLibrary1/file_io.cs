using System;
using System.Collections;
using System.Text;
using System.IO;

namespace ClassLibrary1
{
    public class file_io
    {
        public static void read_file_lines_to_al(ref ArrayList lines, string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (sr.Peek() >= 0)
                {
                    lines.Add(sr.ReadLine());
                }
            }
        }


        public string ReadFileToStr(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return (text);
        }

        public static string ReadFileToStr1(string filePath)
            {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return (text);
            }

        public static void write_text_array_to_file(string filePath, string[] lines)
        {
            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.
            //string[] lines = {"First line", "Second line", "Third line"};
            System.IO.File.WriteAllLines(filePath, lines);//@"C:\Users\Public\TestFolder\WriteLines.txt", lines);
        }


        public static void write_text_to_file(string filePath, string text)
        {
            // Example #2: Write one string to a text file.
            //string text = "A class is the most powerful data type in C#. Like structures, " +                        
            //"a class defines the data and behavior of the data type. ";
            System.IO.File.WriteAllText(filePath, text);
        }


        /// <summary>
        /// reads [numLines] from srcFile into destFile
        /// intended for CSV files as number of lines to write can be specified
        /// will be used in case SSPA-4537
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        /// <param name="numLines"></param>
        public static void read_file_to_another(string srcFile, string destFile, int numLines)
        {
            int linesRead = 0;
            StringBuilder szTxtInFile = new StringBuilder();

            using (var sr = new StreamReader(srcFile))
            {
                while (sr.Peek() >= 0 && linesRead < numLines)
                {
                    szTxtInFile.Append(sr.ReadLine() + Environment.NewLine);
                    linesRead++;
                }
            }

            if (System.IO.File.Exists(destFile)) System.IO.File.Delete(destFile);
            System.IO.File.WriteAllText(destFile, szTxtInFile.ToString());
        }

    }

}
