namespace BlazeFusion.Configuration;
 
/// <summary>
/// Blaze options
/// </summary>
public class BlazeOptions
{
    private IEnumerable<IBlazeValueMapper> _valueMappers;

    internal Dictionary<Type, IBlazeValueMapper> ValueMappersDictionary { get; set; } = new();

    /// <summary>
    /// Indicates if antiforgery token should be exchanged during the communication
    /// </summary>
    public bool AntiforgeryTokenEnabled { get; set; }

    /// <summary>
    /// Performs mapping of each value that goes through binding mechanism in all the components
    /// </summary>
    public IEnumerable<IBlazeValueMapper> ValueMappers
    {
        get => _valueMappers;
        set
        {
            _valueMappers = value;

            if (value != null)
            {
                ValueMappersDictionary = value.ToDictionary(mapper => mapper.MappedType, mapper => mapper);
            }
        }
    }
}
