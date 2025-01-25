namespace BlazeFusion;
internal interface IPersistentState
{
    string Compress(string value);
    string Decompress(string value);
}
