namespace SwiftTD.Interfaces
{
    public interface ISaveAsJSON : IJsonSerializable
    {
        public string GetFilePath();
        public string GetID();
    }
}
