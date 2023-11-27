using System.Linq.Expressions;
using System.Reflection;
using Hi.Types;

namespace Hi.Tools;

public static class Mapper
{
    public static TTarget Map<TTarget>(this object source) where TTarget : new()
    {
        var target = new TTarget();

        if (source == null)
            return target;

        var targetProperties = target.GetType().GetProperties().ToList();

        Map(source, target, targetProperties);

        return target;
    }
    
    public static TTarget Map<TSource, TTarget>(TSource source, TTarget target, List<string> targetPropertyNames = null)
    {
        if (source == null || target == null)
            return target;

        var targetProperties = target.GetType().GetProperties().ToList();

        if (targetPropertyNames != null && targetPropertyNames.Any())
            targetProperties = targetProperties.Where(x => targetPropertyNames.Contains(x.Name)).ToList();

        Map(source, target, targetProperties);

        return target;
    }

    public static void Map<TSource, TTarget, TX>(TSource source, TTarget target, Expression<Func<TSource, TX>> ignoreExpression) where TX : class
    {
        if (target == null || source == null)
            return;

        var targetProperties = target.GetType().GetProperties().ToList();

        if (ignoreExpression != null)
        {
            var ignoreProperties = ignoreExpression.Body.Type.GetProperties().ToList().Select(x => x.Name).ToList();
            targetProperties = targetProperties.Where(x => !ignoreProperties.Contains(x.Name)).ToList();
        }

        Map(source, target, targetProperties);
    }

    private static void Map<TSource, TTarget>(TSource source, TTarget target, IEnumerable<PropertyInfo> targetProperties)
    {
        foreach (var targetProperty in targetProperties)
        {
            var sourceProperty = source.GetType().GetProperties().FirstOrDefault(p => p.Name == targetProperty.Name);
            if (sourceProperty == null)
                continue;

            if (targetProperty.SetMethod == null)
                continue;

            if (sourceProperty.PropertyType == targetProperty.PropertyType)
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null));
            else
            if (sourceProperty.PropertyType == typeof(HashInt) && targetProperty.PropertyType == typeof(int))
            {
                var hashInt = (HashInt)sourceProperty.GetValue(source, null);
                targetProperty.SetValue(target, hashInt?.ToInt() ?? 0);
            }
            else
            if (sourceProperty.PropertyType == typeof(int) && targetProperty.PropertyType == typeof(HashInt))
            {
                var valuem = (int?)sourceProperty.GetValue(source, null);
                targetProperty.SetValue(target, new HashInt(valuem));
            }

        }
    }
    
}