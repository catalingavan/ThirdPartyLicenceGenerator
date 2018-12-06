using System;

namespace ThirdPartyLicenceGenerator.Parsers
{
    public static class FileParserFactory
    {
        public static IFileParser CreateParser(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName);

            IFileParser fileParser = null;

            if (string.Compare(fileName, "packages.config", StringComparison.OrdinalIgnoreCase) == 0)
            {
                fileParser = new PackagesConfigFileParser();
            }
            else if (string.Compare(extension, ".csproj", StringComparison.OrdinalIgnoreCase) == 0)
            {
                fileParser = new CsprojFileParser();
            }

            return fileParser ?? new NullFileParser();
        }
    }
}
