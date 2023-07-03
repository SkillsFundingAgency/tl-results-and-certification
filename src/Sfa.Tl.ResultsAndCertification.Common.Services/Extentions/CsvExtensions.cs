using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class CsvExtensions
    {
        public static async Task<byte[]> WriteFileAsync<T>(IList<T> data, string delimeter = ",", bool writeHeader = true, Type classMapType = null)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = delimeter,
                ShouldQuote = args => false                
            };

            await using var ms = new MemoryStream();
            await using (var sw = new StreamWriter(ms))
            await using (var cw = new CsvWriter(sw, config))
            {
                if (classMapType != null && typeof(ClassMap).IsAssignableFrom(classMapType))
                {
                    var map = (ClassMap)ObjectResolver.Current.Resolve(classMapType);
                    cw.Context.RegisterClassMap(map);
                }

                if (writeHeader)
                {
                    var headerNames = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<DisplayNameAttribute>(false) != null).Select(pr => pr.GetCustomAttribute<DisplayNameAttribute>(false).DisplayName).ToList();
                    headerNames.ForEach(headerName =>
                    {
                        cw.WriteField(headerName);
                    });
                    cw.NextRecord();
                }

                await cw.WriteRecordsAsync<T>(data);
            }
            return ms.ToArray();
        }
    }
}