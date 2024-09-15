using System;

public abstract class Document
{
    public string FilePath { get; }
    public string Content { get; protected set; }

    protected Document(string filePath)
    {
        FilePath = filePath;
        Parse();
    }

    public abstract void Parse();
}