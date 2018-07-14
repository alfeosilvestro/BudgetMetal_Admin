using System.Linq;

namespace Com.BudgetMetal.Services.Base
{
    public class BaseService
    {
        public static void Copy<TSource, TDestination>(TSource source, TDestination destination, string[] skipPropertyNames = null)
            where TSource : class
            where TDestination : class
        {
            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (sourceProperty.Name == destinationProperty.Name && sourceProperty.PropertyType == destinationProperty.PropertyType)
                    {
                        if (skipPropertyNames != null)
                        {
                            var skipPropertyName = skipPropertyNames.FirstOrDefault(n => n.ToLower().Equals(destinationProperty.Name.ToLower()));

                            if (string.IsNullOrEmpty(skipPropertyName))
                            {
                                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                                break;
                            }
                        }
                        else
                        {
                            destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                            break;
                        }
                    }
                }
            }
        }
    }
}
