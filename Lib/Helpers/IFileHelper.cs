namespace Lib.Helpers
{
    public interface IFileHelper
    {
        void WriteToFile<T>(string filepath, T content);
    }
}