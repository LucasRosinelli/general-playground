using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BluePrism.Hackathon.Package.Temp.Contracts;
using Microsoft.Extensions.Logging;

namespace BluePrism.Hackathon.Package.Temp
{
    public class WordLadder : IWordLadder
    {
        private readonly WordLadderOptions _options;
        private readonly ILogger<WordLadder> _logger;
        private bool _disposed = false;
        private IEnumerable<IWordLadderNode> _nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordLadderOptions"/> class.
        /// </summary>
        /// <param name="options">The <see cref="WordLadderOptions"/> used to solve the word-ladder problem.</param>
        /// <param name="logger">The <see cref="ILogger"/> used to log messages, warnings and errors.</param>
        public WordLadder(WordLadderOptions options, ILogger<WordLadder> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IEnumerable<IWordLadderNode> GenerateNodes(int wordLength)
        {
            _logger.LogInformation("Started generating nodes.");

            var stopwatch = CreateAndStartStopwatch();

            IEnumerable<IWordLadderNode> nodes = _options.Dictionary
                .Where(w => w.Length.Equals(wordLength))
                .Select(w => WordLadderNode.CreateNode(w))
                .ToList();

            EndAndLogStopwatch(stopwatch, "to generate nodes");

            if (nodes.Any())
            {
                _logger.LogInformation("{QuantityOfNodes} nodes created.", nodes.LongCount());
            }
            else
            {
                _logger.LogWarning($"No nodes created. There is no words in the dictionary.");
            }

            return nodes;
        }

        public IWordLadderNode Solve(string startWord, string endWord)
        {
            if (!startWord.Length.Equals(endWord.Length))
            {
                _logger.LogWarning("The length of {StartWord} and {EndWord} must be the same.", startWord, endWord);
                return null;
            }

            _nodes = GenerateNodes(startWord.Length);

            _logger.LogInformation("Started solving word-ladder from {StartWord} to {EndWord}.", startWord, endWord);

            var stopwatch = CreateAndStartStopwatch();

            var Q = new Queue<IWordLadderNode>();
            IWordLadderNode root = WordLadderNode.CreateNode(startWord);
            root.MarkAsDiscovered();
            Q.Enqueue(root);
            while (Q.Any())
            {
                var v = Q.Dequeue();
                if (v.Word.Equals(endWord, _options.WordComparison))
                {
                    _logger.LogInformation("Success! Word-ladder found from {StartWord} to {EndWord}!", startWord, endWord);

                    EndAndLogStopwatch(stopwatch, "to solve word-ladder");
                    return v;
                }

                foreach (var w in _nodes)
                {
                    if (!w.Discovered && w.IsMatchOffset(v, _options, false))
                    {
                        w.MarkAsDiscovered(v);
                        Q.Enqueue(w);
                    }
                }
            }

            EndAndLogStopwatch(stopwatch, "trying to solve word-ladder");

            _logger.LogWarning("Fail! No word-ladder found from {StartWord} to {EndWord}.", startWord, endWord);

            return null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }

        private Stopwatch CreateAndStartStopwatch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return stopwatch;
        }

        private void EndAndLogStopwatch(Stopwatch stopwatch, string operation)
        {
            stopwatch.Stop();

            _logger.LogDebug(string.Concat("Time elapsed [", operation, "]: {ElapsedMilliseconds} milliseconds ({ElapsedTimeSpan})."), stopwatch.ElapsedMilliseconds, stopwatch.Elapsed);
        }
    }
}
