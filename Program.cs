using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        // Объедините две предыдущих работы(практические работы 2 и 3):
        // поиск файла и поиск текста в файле написав утилиту
        // которая ищет файлы определенного расширения с указанным текстом.
        // Рекурсивно.Пример вызова утилиты: utility.exe txt текст.
        {
            DirectoryInfo startdir = new DirectoryInfo(Directory.GetCurrentDirectory());
            DirectoryInfo currentdir = startdir.Parent.Parent.Parent;
            string path = currentdir.FullName;

            string name = "txt";
            string text = "текст";
            if (args.Length != 0)
            {
                name = args[0];
                text = args[1];
                path = Directory.GetCurrentDirectory();
            }

            List<string> list = SearchIn(path, name);
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (string file in list)
            {
                if (Filter2(text, ReadFrom(file), out string target))
                {
                    res.Add(file, target);
                }
            }
            foreach (var file in res)
            {
                Console.WriteLine(file);
            }
        }

        private static List<string> SearchIn(string path, string name)
        // Напишите утилиту рекурсивного поиска файлов в заданном каталоге и подкаталогах
        {
            var list = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            try
            {
                DirectoryInfo[] directories = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo item in files)
                {
                    if (item.Extension.Contains(name))
                    {
                        list.Add(item.FullName);
                    }
                }
                foreach (var item in directories)
                {
                    list.AddRange(SearchIn(item.FullName, name));
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
            }
            return list;
        }

        static List<string> ReadFrom(string path)
        // Напишите утилиту читающую тестовый файл и выводящую на экран строки содержащие искомое слово.
        {
            List<string> result = new List<string>();

            try
            {
                using StreamReader sr = new StreamReader(path);
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (line != null) result.Add(line);
                    }
                }
            }
            catch { }
            return result;
        }

        static bool Filter2(string text, List<string> rows, out string? some)
        {
            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    if (row.ToLower().Contains(text.ToLower()))
                    {
                        some = row;
                        return true;
                    }
                }
            }
            some = null;
            return false;
        }

        private static void WriteTo(string str, string path)
        // Напишите консольную утилиту для копирования файлов 
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using StreamWriter sw = new StreamWriter(fs); sw.Write(str);
            }
        }
        private static string ReaderFrom(string path)
        {
            using StreamReader sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}