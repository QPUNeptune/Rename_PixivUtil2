using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

namespace rename_pixiv
{
    class Program
    {
        static void Main(string[] args)
        {
            string old1, old2;
            int index = 0;
            bool isRenaming;
            int test = 0;

            Console.WriteLine("Do you wand to rename files? (1 - yes, 0 - no)");
            do
            {
                string ansStr = Console.ReadLine() ?? "-1";
                bool isSUCCessful = int.TryParse(ansStr, out int ansInt);
                if (!isSUCCessful || ansInt is not (0 or 1))
                {
                    Console.Clear();
                    Console.WriteLine("is your brain made of cheap plywood? i said to put a 1(yes) or a 0(no), not whatever garbage you typed");
                }
                else
                {
                    isRenaming = Convert.ToBoolean(ansInt);
                    break;
                }
            } while (true);

            if (isRenaming)
            {
                Console.Clear();
                string dirPath = Directory.GetCurrentDirectory(); //get the directory path the program is currently in
                List<string> files = Directory.GetFiles(dirPath).ToList(); //get all file paths in the directory the program is currently in
                files.Remove(files.Find(s => s.Contains(Environment.GetCommandLineArgs()[0]))); //remove the program path to avoid renaming itself
                
                for (var i = 0; i < files.Count; i++)
                    files[i] = Path.GetFileName(files[i]); //get just the file name instead of the full path 
                
                files.Reverse(); //since i want to order by newer first
                old2 = files[0].Substring(0, files[0].IndexOf("_", StringComparison.Ordinal)); //this is to avoid an error on the next section
                

                foreach (string filename in files)
                {
                    string newFilename;

                    old1 = filename.Substring(0, filename.IndexOf("_", StringComparison.Ordinal));
                    newFilename = filename.Remove(0, old1.Length); //this is to remove just the first numbers on the filename

                    if (old1 != old2) index++; //since i want the images from the same posts on pixiv to still be grouped

                    newFilename = index + newFilename;

                    Directory.CreateDirectory("New Filename");
                    File.Copy(filename, Path.Combine("New Filename", newFilename));

                    Console.WriteLine($"Old Filename: {filename} | New Filename: {newFilename}");
                    old2 = filename.Substring(0, filename.IndexOf("_", StringComparison.Ordinal));
                }
            }

            ExitProgram();
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
