namespace Dotnet.Template.Application.Converters;

public class ModelConverterBase<T, Y>
    where T: class
    where Y: class
{
    public Y Convert(T source)
    {
        return DefaultConvert<T,Y>(source);
    }

    public virtual T Convert(Y source)
    {
        return DefaultConvert<Y,T>(source);
    }

    protected Z? DefaultConvert<X,Z>(X? source) where Z : class
        where X: class
    {
        if (source == null)
        {
            return null;
        }

        var sourceType = typeof(X);
        var targetType = typeof(Z);
        
        var sourceProperties = sourceType.GetProperties().ToDictionary(p => p.Name, p=> p);
        var targetProperties = targetType.GetProperties().ToDictionary(p=> p.Name, p => p);

        var result = Activator.CreateInstance<Z>();
        
        foreach (var prop in targetProperties.Keys.Where(prop => sourceProperties.ContainsKey(prop) != false))
        {
            targetProperties[prop].SetValue(result, sourceProperties[prop].GetValue(source));
        }

        return result;
    }
}
