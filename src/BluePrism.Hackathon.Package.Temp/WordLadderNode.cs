using System;
using BluePrism.Hackathon.Package.Temp.Contracts;

namespace BluePrism.Hackathon.Package.Temp
{
    public class WordLadderNode : IWordLadderNode
    {
        private bool _disposed = false;

        public WordLadderNode(string word)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        /// <inheritdoc/>
        public string Word { get; set; }

        /// <inheritdoc/>
        public bool Discovered { get; set; } = false;

        /// <inheritdoc/>
        public IWordLadderNode Parent { get; set; } = null;

        /// <inheritdoc/>
        public int Level { get; set; } = 0;

        public static WordLadderNode CreateNode(string word)
        {
            return new WordLadderNode(word);
        }

        public void MarkAsDiscovered(IWordLadderNode parent = null)
        {
            Discovered = true;
            Parent = parent;
            Level = parent == null ? 0 : parent.Level + 1;
        }

        public bool IsMatchOffset(IWordLadderNode node, WordLadderOptions options, bool exactOffset = false)
        {
            return IsMatchOffset(node, options.Offset, exactOffset, options.WordComparison);
        }

        private bool IsMatchOffset(IWordLadderNode node, int offset, bool exactOffset = false, StringComparison wordComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (node.Word.Length != Word.Length || node.Word.Equals(Word, wordComparison))
            {
                return false;
            }

            var offsetCounter = 0;

            for (int i = 0; i < Word.Length; i++)
            {
                offsetCounter += (Word[i].Equals(node.Word[i]) ? 0 : 1);
                if (offsetCounter > offset)
                {
                    break;
                }
            }

            return (exactOffset ? offsetCounter == offset : offsetCounter <= offset);
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

            if (disposing)
            {
                Parent?.Dispose();
            }

            _disposed = true;
        }
    }
}
