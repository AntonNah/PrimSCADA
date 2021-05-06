using System;
using System.IO;
using ProtoBuf;

[ProtoContract]
public class Settings
{
    [ProtoMember(1)]
    public bool CreateDirectory { get; set; }
    [ProtoMember(2)]
    public string DirectoryPath { get; set; }

    public Settings()
    {
        CreateDirectory = true;
        DirectoryPath = Directory.GetCurrentDirectory();
    }
}