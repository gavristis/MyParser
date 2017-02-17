namespace MyParser.BLL.Interfaces
{
    public interface ISiteTreeService
    {
        void BuildTree(string startingPage, int maxDepth);
        void BuildTree(int maxDepth);
    }
}
