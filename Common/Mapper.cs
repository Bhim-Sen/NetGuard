using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Mapper
    {
        public static async Task<List<TDestination>> MapListToDto<TSource, TDestination>(IEnumerable<TSource> sourceList)
        {
            var destinationList = new List<TDestination>();

            foreach (var source in sourceList)
            {
                var destination = await MapToDto<TSource, TDestination>(source);
                destinationList.Add(destination);
            }

            return destinationList;
        }
 

        public static async Task<TDestination> MapToDto<TSource, TDestination>(TSource source)
        {
            var destination = Activator.CreateInstance<TDestination>();

            foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties())
            {
                PropertyInfo destinationProperty = typeof(TDestination).GetProperty(sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                }
            }

            return destination;
        }
        public static async Task<List<TSource>> MapListToDbTbl<TDestination, TSource>(List<TDestination> destinationList)
        {
            var sourceList = new List<TSource>();

            foreach (var destination in destinationList)
            {
                var source = await MapToDbTbl<TDestination, TSource>(destination);
                sourceList.Add(source);
            }

            return sourceList;
        }

        public static async Task<TSource> MapToDbTbl<TDestination, TSource>(TDestination destination)
        {
            var source = Activator.CreateInstance<TSource>();

            foreach (PropertyInfo destinationProperty in typeof(TDestination).GetProperties())
            {
                PropertyInfo sourceProperty = typeof(TSource).GetProperty(destinationProperty.Name);

                if (sourceProperty != null && sourceProperty.CanWrite)
                {
                    sourceProperty.SetValue(source, destinationProperty.GetValue(destination));
                }
            }

            return source;
        }
    }
}
