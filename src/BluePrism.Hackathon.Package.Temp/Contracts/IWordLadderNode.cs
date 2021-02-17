using System;

namespace BluePrism.Hackathon.Package.Temp.Contracts
{
    /// <summary>
    /// Represents the word-ladder node.
    /// </summary>
    public interface IWordLadderNode : IDisposable
    {
        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        string Word { get; set; }

        /// <summary>
        /// Specifies whether this node has already been discovered.
        /// </summary>
        bool Discovered { get; set; }

        /// <summary>
        /// Specifies the parent of this node.
        /// </summary>
        /// <remarks>
        /// This value can be <see langword="null"/>.
        /// </remarks>
        IWordLadderNode Parent { get; set; }

        /// <summary>
        /// Indicates the level of the node in the discovery depth.
        /// </summary>
        int Level { get; set; }

        void MarkAsDiscovered(IWordLadderNode parent = null);
        bool IsMatchOffset(IWordLadderNode node, WordLadderOptions options, bool exactOffset = false);
        //bool IsMatchOffset(IWordLadderNode node, int offset, bool exactOffset = false, StringComparison wordComparison = StringComparison.OrdinalIgnoreCase);
    }
}
