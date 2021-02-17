using System;
using System.Collections.Generic;
using System.IO;
using BluePrism.Hackathon.Package.Temp.Contracts;

namespace BluePrism.Hackathon.Package.Temp
{
    /// <summary>
    /// Options for <see cref="IWordLadder"/>.
    /// </summary>
    public class WordLadderOptions
    {
        /// <summary>
        /// The <see cref="Offset"/> default value.
        /// </summary>
        public const int OffsetDefault = 1;

        private int _offset = OffsetDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordLadderOptions"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary of words.</param>
        /// <param name="wordComparison">The <see cref="StringComparison"/> used for word comparison. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <param name="offset">The maximum <see cref="Offset"/> allowed while comparing two words. Must be greater or equal to <c>1</c>. Defaults to <see cref="OffsetDefault"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// dictionary is null.
        /// </exception>
        public WordLadderOptions(IEnumerable<string> dictionary, StringComparison wordComparison = StringComparison.OrdinalIgnoreCase, int offset = OffsetDefault)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            WordComparison = wordComparison;
            Offset = offset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordLadderOptions"/> class.
        /// </summary>
        /// <param name="file">The file containing dictionary of words (one word per line).</param>
        /// <param name="wordComparison">The <see cref="StringComparison"/> used for word comparison. Defaults to <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <param name="offset">The maximum <see cref="Offset"/> allowed while comparing two words. Must be greater or equal to <c>1</c>. Defaults to <see cref="OffsetDefault"/>.</param>
        public WordLadderOptions(string file, StringComparison wordComparison = StringComparison.OrdinalIgnoreCase, int offset = OffsetDefault)
            : this(File.ReadAllLines(file), wordComparison, offset)
        {
        }

        /// <summary>
        /// Gets the dictionary of words.
        /// </summary>
        public IEnumerable<string> Dictionary { get; }

        /// <summary>
        /// Get or sets the <see cref="StringComparison"/> used for word comparison.
        /// </summary>
        public StringComparison WordComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// Gets or sets the maximum offset allowed while comparing two words.
        /// </summary>
        /// <remarks>
        /// The minimum offset is <c>1</c>.
        /// </remarks>
        public int Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException($"{nameof(Offset)} must be greater or equal to 1.");
                }
                _offset = value;
            }
        }
    }
}
