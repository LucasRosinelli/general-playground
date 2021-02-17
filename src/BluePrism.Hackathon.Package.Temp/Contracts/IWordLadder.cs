using System;

namespace BluePrism.Hackathon.Package.Temp.Contracts
{
    public interface IWordLadder : IDisposable
    {
        IWordLadderNode Solve(string startWord, string endWord);
    }
}
