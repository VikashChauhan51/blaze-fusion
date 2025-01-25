namespace BlazeFusion;
 

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class BlazeValueMapper<T> : IBlazeValueMapper
{
    private Func<T, Task<T>> MapperAsync { get; set; }
    private Func<T, T> Mapper { get; set; }

    /// <summary />
    public Type MappedType => typeof(T);

    /// <summary/>
    public BlazeValueMapper(Func<T, Task<T>> mapperAsync) => MapperAsync = mapperAsync;

    /// <summary/>
    public BlazeValueMapper(Func<T, T> mapper) => Mapper = mapper;

    /// <summary />
    public async Task<object> Map(object value) =>
        MapperAsync != null
            ? await MapperAsync((T)value)
            : Mapper((T)value);
}

/// <summary/>
public interface IBlazeValueMapper
{
    /// <summary />
    Task<object> Map(object value);

    /// <summary />
    Type MappedType { get; }
}
