using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Com.BudgetMetal.Services.Base
{
    public class BaseService
    {
        public static string GetCurrentWeek()
        {
            string result = "";
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            int currentWeek = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
            int currentYear = DateTime.Now.Year;
            result = currentYear.ToString() + "-" + currentWeek.ToString();
            return result;
        }

        /// <summary>
        /// Copy one object type to another
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="destination">Destination.</param>
        /// <param name="skipPropertyNames">Skip property names.</param>
        /// <typeparam name="TSource">The 1st type parameter.</typeparam>
        /// <typeparam name="TDestination">The 2nd type parameter.</typeparam>
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

        /// <summary>
        /// Copies the list.
        /// </summary>
        /// <param name="sourceList">Source list.</param>
        /// <param name="destinationList">Destination list.</param>
        /// <param name="skipPropertyNames">Skip property names.</param>
        /// <typeparam name="TSource">The 1st type parameter.</typeparam>
        /// <typeparam name="TDestination">The 2nd type parameter.</typeparam>
        public static void CopyList<TSource, TDestination>(IEnumerable<TSource> sourceList, 
                                                           IEnumerable<TDestination> destinationList, 
                                                           string[] skipPropertyNames = null)
            where TSource : class
            where TDestination : class, new()
        {

            foreach(var sourceItem in sourceList)
            {
                TDestination tempDestObj = new TDestination();

                var sourceProperties = sourceItem.GetType().GetProperties();
                var destinationProperties = tempDestObj.GetType().GetProperties();

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
                                    destinationProperty.SetValue(tempDestObj, sourceProperty.GetValue(sourceItem));
                                    break;
                                }
                            }
                            else
                            {
                                destinationProperty.SetValue(tempDestObj, sourceProperty.GetValue(sourceItem));
                                break;
                            }
                        }
                    }
                }

                // add to destination list
                destinationList.Append(tempDestObj);
            }
        }
    }
}
