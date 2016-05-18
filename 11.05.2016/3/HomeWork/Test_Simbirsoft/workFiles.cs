using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security;

namespace HomeWork
{
    /// <summary>
    /// workFiles класс в котором описано все взаимодействие с файлами.
    /// </summary>
    class workFiles
    {
        int N, countLines; // N-количество строк на которые разбиваются файлы; countLines- количество линий в тексте.
        string adressDictionary, adressText; // adressDictionary-абсолютный адрес словаря; adressText-абсолютный адрес текста; 

        public workFiles(int N, string adressDictionary, string adressText)  //Конструктор присваивает значения полям, проверяет на наличие адресов файлов
        {                                                                    //и проеряет указывает ли адрес на текстовый файл.
            if ((adressDictionary != null) | (adressText != null))
            {
                if (adressDictionary.Contains(".txt"))
                { this.adressDictionary = adressDictionary; }
                else { Console.WriteLine("Ivalid format dictionary file!");}

                if (adressText.Contains(".txt"))
                { this.adressText = adressText; }
                else { Console.WriteLine("Ivalid format file of text!"); }

                this.N = N; 
            }

        }

        /// <summary>
        /// Создает файлы HTML и записывает туда обработанные данные из текста
        /// </summary>
        public void createHTMLFiles()
        {
            Queue<string> dictionary = new Queue<string>();
            dictionaryGetWords(ref dictionary);
            if (dictionary == null) { dictionary.Enqueue(" "); }

            countLine();

            string textForWrite="", text;             //textForWrite- строка для записи в файл; text-строка для обработки;
            int k = 0, m=1;                           //k-количество файлов(используется для создания имен); m-множитель N;

            try
            {
                for (int i = 0; i < countLines; i++)  //В цикле обрабатывается каждая строка и создаются файлы.
                {
                    text = File.ReadLines(adressText).Skip(i).First().ToString();

                    if (text.Contains(".") | text.Contains("!") | text.Contains("?"))
                    {
                        dotContainer(i, N, ref k, ref m, ref textForWrite, ref text, dictionary);
                        exclamationPointContainer(i, N, ref k, ref m, ref textForWrite, ref text, dictionary);
                        questionMarkContainer(i, N, ref k, ref m, ref textForWrite, ref text, dictionary);

                        textForWrite = text;
                    }
                    else
                    {
                        textForWrite += text;
                    }
                }

                File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary)), Encoding.Unicode);
                
                if (File.Exists(@"itplace\" + k.ToString() + ".html"))
                    {
                        File.Delete(@"itplace\" + k.ToString() + ".html");
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                    }
                    else
                    {
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                    }
            }
            catch
            {
                Console.WriteLine("When creating HTML file arose error. Please check input data! \nRead instruction.");
            }
        }

        /// <summary>
        /// Получает количество строк в тексте.
        /// </summary>
        public void countLine()
        {
            try
            {
                IEnumerable<string> countOfLines = File.ReadLines(adressText);
                this.countLines = countOfLines.Count();
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File text not found!");
            }
        }

        /// <summary>
        /// Находит словарные слова в строке и помечает их.
        /// </summary>
        /// <param name="textLine">Входная строка.</param>
        /// <param name="dictionary">Словарь.</param>
        /// <returns></returns>
        string changeText(ref string textLine, Queue<string> dictionary)
        {
           // List<string> wordList = dictionary.ToList();
            foreach(string word in dictionary)
            {
                if(textLine.Contains(word))
                {
                   textLine = textLine.Replace(word, (@"<i><b>" + (word) + @"</b></i>"));
                }
            }
            return textLine;
        }

        /// <summary>
        /// Получает словарные слова из файла и добавляет их в массив.
        /// </summary>
        /// <param name="dictionary">Словарь.</param>
        /// <returns>Массив слов всловаре.</returns>
        void dictionaryGetWords(ref Queue<string> dictionary)
        {
            try
            {
                FileStream dictionaryFile = new FileStream(adressDictionary, FileMode.Open);
                StreamReader readerDictionary = new StreamReader(dictionaryFile);
                string textDictionary = readerDictionary.ReadToEnd().ToString();
                Regex seekWords = new Regex(@"[\wа-яА-я]*[\wа-яА-я]",RegexOptions.IgnoreCase);
                MatchCollection dictionaryCollection = seekWords.Matches(textDictionary);
                for (int i = 0; i < dictionaryCollection.Count; i++)
                {
                    dictionary.Enqueue(dictionaryCollection[i].ToString());
                }
                readerDictionary.Close();
                dictionaryFile.Close();
            }
            catch(SecurityException)
            {
                Console.WriteLine("File acess denide!");
            }
            catch(PathTooLongException)
            {
                Console.WriteLine("File name or adress to file is long! \nEnter other file name or adress to file! ");
                
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File dictionary not found!");
            }
            catch(DirectoryNotFoundException)
            {
                Console.WriteLine("Directory itplace not found! Read instruction and follow requirements!");
            }
        }

        /// <summary>
        /// Обрабатывает строку если та содержит точку.
        /// </summary>
        /// <param name="i">Счетчик цикла.</param>
        /// <param name="N">Количество строк.</param>
        /// <param name="k">Количество файлов.</param>
        /// <param name="m">Множитель N.</param>
        /// <param name="textForWrite">Строка для записи в файл.</param>
        /// <param name="text">Строка для обработки.</param>
        /// <param name="dictionary">Словарь.</param>
        void dotContainer(int i, int N, ref int k, ref int m, ref string textForWrite, ref string text, Queue<string> dictionary)
        {
            if (text.Contains("."))
            {
                textForWrite = textForWrite + " " + text.Split('.')[0];
                if (i <= (N * m))
                {
                    File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "."), Encoding.Unicode);
                }
                else if (i > (N * m))
                {
                    if (File.Exists(@"itplace\" + k.ToString() + ".html"))
                    {
                        File.Delete(@"itplace\" + k.ToString() + ".html");
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "."), Encoding.Unicode);
                        m++;

                    }
                    else
                    {
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "."), Encoding.Unicode);
                        m++;
                    }
                }

                text = text.Split('.')[1];
            }
        }

        /// <summary>
        /// Обрабатывает строку если та содержит восклицательный знак.
        /// </summary>
        /// <param name="i">Счетчик цикла.</param>
        /// <param name="N">Количество строк.</param>
        /// <param name="k">Количество файлов.</param>
        /// <param name="m">Множитель N.</param>
        /// <param name="textForWrite">Строка для записи в файл.</param>
        /// <param name="text">Строка для обработки.</param>
        /// <param name="dictionary">Словарь.</param>
        void exclamationPointContainer(int i, int N, ref int k, ref int m, ref string textForWrite, ref string text, Queue<string> dictionary)
        {
            if (text.Contains("!"))
            {
                textForWrite = textForWrite + " " + text.Split('!')[0];
                if (i <= (N * m))
                {
                    File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "!"), Encoding.Unicode);
                }
                else if (i > (N * m))
                {
                    if (File.Exists(@"itplace\" + k.ToString() + ".html"))
                    {
                        File.Delete(@"itplace\" + k.ToString() + ".html");
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "!"), Encoding.Unicode);
                        m++;
                    }
                    else
                    {
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "!"), Encoding.Unicode);
                        m++;
                    }
                }

                text = text.Split('!')[1];
            }
        }

        /// <summary>
        /// Обрабатывает строку если та содержит знак вопроса.
        /// </summary>
        /// <param name="i">Счетчик цикла.</param>
        /// <param name="N">Количество строк.</param>
        /// <param name="k">Количество файлов.</param>
        /// <param name="m">Множитель N.</param>
        /// <param name="textForWrite">Строка для записи в файл.</param>
        /// <param name="text">Строка для обработки.</param>
        /// <param name="dictionary">Словарь.</param>
        void questionMarkContainer(int i, int N, ref int k, ref int m, ref string textForWrite, ref string text, Queue<string> dictionary)
        {
            if (text.Contains("?"))
            {
                textForWrite = textForWrite + " " + text.Split('?')[0];
                if (i <= (N * m))
                {
                    File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "?"), Encoding.Unicode);
                }
                else if (i > (N * m))
                {
                    if (File.Exists(@"itplace\" + k.ToString() + ".html"))
                    {
                        File.Delete(@"itplace\" + k.ToString() + ".html");
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "?"), Encoding.Unicode);
                        m++;
                    }
                    else
                    {
                        File.Copy(@"itplace\" + k.ToString() + ".txt", @"itplace\" + k.ToString() + ".html");
                        File.Delete(@"itplace\" + k.ToString() + ".txt");
                        k++;
                        if (File.Exists(@"itplace\" + k.ToString() + ".txt")) { File.Delete(@"itplace\" + k.ToString() + ".txt"); }
                        File.AppendAllText(@"itplace\" + k.ToString() + ".txt", (changeText(ref textForWrite, dictionary) + "?"), Encoding.Unicode);
                        m++;
                    }
                }

                text = text.Split('?')[1];
            }
        }
    }
}