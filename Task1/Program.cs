using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:/Users/ir_ya/Dropbox/Github/KamazTasks/Task1/data/employees.txt";

            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path, Encoding.UTF8);


                List<string> lines = new List<string>();

                // Построчное считывание файла в список
                string rdLine;
                while ((rdLine = reader.ReadLine()) != null)
                    lines.Add(rdLine);

                // Сортировка без учёта полной вложенности.
                string[] frame = new string[lines.Count + 1];
                for (int firstId = 0; lines.Count > firstId; firstId++)
                {
                    for (int secondId = 0; lines.Count > secondId; secondId++)
                    {
                        if (firstId.ToString() == lines[secondId].Split("|")[1])
                        {
                            if (frame[firstId] == null)
                            {
                                frame[firstId] = lines[secondId];
                            }
                            else
                            {
                                frame[firstId] = frame[firstId] + ";" + lines[secondId];
                            }
                        }
                    }
                }

                // Массив тех, кому никто не принадлежит
                string[] nobodyBelongs = new string[frame.Length];
                for (int i = 0; frame.Length > i; i++)
                {
                    // Если верно, значит пользователю с id = i никто не принадлежит
                    if (frame[i] == null)
                    {
                        string user = SearchUser(i, frame);
                        nobodyBelongs[Convert.ToInt32(user.Split("|")[1])] = user;
                    }
                }

                // Перебор из массива в список
                List<string> users = new List<string>();
                for (int a = 0; frame.Length > a; a++)
                    if (frame[a] != null)
                        users.Add(frame[a]);

                // Вывод иерархии сотрдуников
                int c = 0;
                foreach (string u in users)
                {
                    string[] us = u.Split(";");
                    foreach (string user in us)
                    {
                        bool output = true;
                        for (int a = 0; nobodyBelongs.Length > a; a++)
                        {
                            if (user == nobodyBelongs[a])
                            {
                                output = false;
                                break;
                            }
                        }
                        if (output)
                        {
                            for (int i = 0; c > i; i++)
                                Console.Write("-");

                            Console.Write(user.Split("|")[2]);
                            Console.WriteLine();
                        }
                    }
                    c++;
                }

                // Вывод сотрудников, у которых в подичении никого нет
                for (int a = 0; nobodyBelongs.Length > a; a++)
                {
                    if (nobodyBelongs[a] != null)
                    {
                        for (int b = 0; a > b; b++)
                            Console.Write("-");

                        Console.Write(nobodyBelongs[a].Split("|")[2]);
                        Console.WriteLine();
                    }
                }
            }
            Console.ReadKey();
        }

        /// <summary>
        /// Ищет пользователя в массиве.
        /// </summary>
        /// <param name="searchId">Id искомого</param>
        /// <param name="array">Массив пользователей</param>
        /// <returns></returns>
        static string SearchUser(int searchId, string[] array)
        {
            string result = "";
            for (int i = 0; array.Length > i; i++)
            {
                if (array[i] == null)
                {
                    continue;
                }
                string[] users = array[i].Split(";");
                foreach (string user in users)
                {
                    if (user.Split("|")[0] == searchId.ToString())
                    {
                        result = user;
                    }
                }
            }
            return result;
        }
    }
}