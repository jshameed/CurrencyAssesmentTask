using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DemoCurrency.Test
{
    internal static class TestData
    {

        public static Task<Stream> GetStreamAsync(string file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "TestData", file);

            if (File.Exists(filePath))
                return Task.FromResult<Stream>(File.OpenRead(filePath));

            throw new FileNotFoundException(filePath);
        }

    }
}
