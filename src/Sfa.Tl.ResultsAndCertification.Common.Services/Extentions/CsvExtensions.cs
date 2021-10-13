﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class CsvExtensions
    {
        public static async Task<byte[]> WriteFileAsync<T>(IList<T> data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = "|"
            };

            await using var ms = new MemoryStream();
            await using (var sw = new StreamWriter(ms))
            await using (var cw = new CsvWriter(sw, config))
            {
                await cw.WriteRecordsAsync<T>(data);
            }
            return ms.ToArray();
        }
    }
}