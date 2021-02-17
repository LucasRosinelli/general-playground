using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BluePrism.Hackathon.Package.Temp;
using BluePrism.Hackathon.Package.Temp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BluePrism.Hackathon.Temp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            string answer = string.Empty;
            do
            {
                Console.WriteLine("Blue Prism - Hackathon");
                Console.WriteLine("Author: Lucas Rosinelli");
                Console.WriteLine("Challenge: Write a program to solve a word puzzle often known as a ‘Word - Ladder’.");
                Console.WriteLine();
                Console.Write("Type the START word: ");
                var startWord = Console.ReadLine();
                Console.Write("Type the END word: ");
                var endWord = Console.ReadLine();
                //Console.Write("Type the OFFSET: ");
                //var offset = Convert.ToInt32(Console.ReadLine());

                var wordLadder = serviceProvider.GetRequiredService<IWordLadder>();
                IWordLadderNode endNode = wordLadder.Solve(startWord, endWord);

                if (endNode is null)
                {
                    Console.WriteLine("Not found!");
                }
                else
                {
                    Console.Write($"Found {endNode.Word} at level {endNode.Level}: ");
                    var result = new Stack<IWordLadderNode>();
                    result.Push(endNode);
                    while (endNode.Parent != null)
                    {
                        endNode = endNode.Parent;
                        result.Push(endNode);
                    }

                    while (result.Any())
                    {
                        Console.Write($"{result.Pop().Word}, ");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine("Good bye!");
                Console.Write("Do you want to continue? (y/n) ");
                answer = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(answer) || answer.Equals("y", StringComparison.OrdinalIgnoreCase));
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config => config.AddConsole());
            services.AddSingleton<WordLadderOptions>(new WordLadderOptions("words-english.txt"));
            services.AddSingleton<IWordLadder, WordLadder>();
        }

        //public static async Task Main(string[] args)
        //{
        //    IEnumerable<string> words = new List<string>()
        //    {
        //        //"dog", "cat", "hot", "fat", "hat", "fog", "dot", "try", "big", "red", "jaw", "job", "zip", "jab", "jam", "box", "fox", "fax", "fix", "log", "fun", "fry", "cut", "cow", "bot", "bow", "max", "mic", "mid", "mod", "mom", "dad",
        //    };

        //    words = await LoadFromFile("words-english.txt");

        //    //const int asciiA = 97;
        //    //const int asciiZ = 122;
        //    //for (int i = asciiA; i <= asciiZ; i++)
        //    //{
        //    //    for (int j = asciiA; j <= asciiZ; j++)
        //    //    {
        //    //        for (int k = asciiA; k <= asciiZ; k++)
        //    //        {
        //    //            words.Add($"{(char)i}{(char)j}{(char)k}");
        //    //        }
        //    //    }
        //    //}

        //    string answer = string.Empty;
        //    do
        //    {
        //        Console.WriteLine("Blue Prism - Hackathon");
        //        Console.WriteLine("Author: Lucas Rosinelli");
        //        Console.WriteLine("Challenge: XPTO");
        //        Console.WriteLine();
        //        Console.Write("Type the START word: ");
        //        var startWord = Console.ReadLine();
        //        Console.Write("Type the END word: ");
        //        var endWord = Console.ReadLine();
        //        Console.Write("Type the OFFSET: ");
        //        var offset = Convert.ToInt32(Console.ReadLine());

        //        var root = Node.CreateNode(startWord);
        //        IEnumerable<Node> nodes = CreateNodes(startWord, words);

        //        var endNode = BFS(nodes, root, endWord, offset);

        //        if (endNode is null)
        //        {
        //            Console.WriteLine("Not found!");
        //        }
        //        else
        //        {
        //            Console.Write($"Found {endNode.Word} at level {endNode.Level}: ");
        //            Console.Write(endNode.Word);
        //            Node? pathNode = endNode.Parent;
        //            do
        //            {
        //                Console.Write($" <== {pathNode.Word}");
        //                pathNode = pathNode.Parent;
        //            } while (pathNode != null);
        //            Console.WriteLine();
        //        }

        //        Console.WriteLine("Good bye!");
        //        Console.Write("Do you want to continue? (y/n) ");
        //        answer = Console.ReadLine();
        //    } while (string.IsNullOrWhiteSpace(answer) || answer.Equals("y", StringComparison.OrdinalIgnoreCase));
        //}

        public static IEnumerable<Node> CreateNodes(string startWord, IEnumerable<string> words)
        {
            var wordNodes = words
                .Where(w => w.Length == startWord.Length && !w.Equals(startWord, StringComparison.OrdinalIgnoreCase))
                .Select(w => Node.CreateNode(w))
                .ToList();

            return wordNodes;
        }

        public static Node? BFS(IEnumerable<Node> G, Node root, string endWord, int offset = 1)
        {
            var Q = new Queue<Node>();
            root.MarkAsDiscovered();
            Q.Enqueue(root);
            while (Q.Any())
            {
                var v = Q.Dequeue();
                if (v.Word.Equals(endWord, StringComparison.OrdinalIgnoreCase))
                {
                    return v;
                }

                foreach (var w in G)
                {
                    if (!w.Discovered && w.IsMatchOffset(offset, false, v))
                    {
                        w.MarkAsDiscovered(v);
                        Q.Enqueue(w);
                    }
                }
            }

            return null;
        }

        private static async Task<IEnumerable<string>> LoadFromFile(string file)
        {
            return await File.ReadAllLinesAsync(file, default(CancellationToken));
        }
    }

    public class Node
    {
        public Node()
        {
        }

        public Node(string word)
            : this()
        {
            Word = word;
        }

        public string Word { get; set; }
        public bool Discovered { get; set; } = false;
        public Node? Parent { get; set; } = null;
        public int Level { get; set; } = 0;

        public static Node CreateNode(string word)
        {
            return new Node(word);
        }

        public void MarkAsDiscovered(Node? parent = null)
        {
            Discovered = true;
            Parent = parent;
            Level = parent == null ? 0 : parent.Level + 1;
        }

        public bool IsMatchOffset(int offset, bool exactOffset, Node wordNode)
        {
            if (wordNode.Word.Length != Word.Length || wordNode.Word.Equals(Word, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var offsetCounter = 0;

            for (int i = 0; i < Word.Length; i++)
            {
                offsetCounter += (Word[i] == wordNode.Word[i] ? 0 : 1);
                if (offsetCounter > offset)
                {
                    break;
                }
            }

            return (exactOffset ? offsetCounter == offset : offsetCounter <= offset);
        }
    }
}
