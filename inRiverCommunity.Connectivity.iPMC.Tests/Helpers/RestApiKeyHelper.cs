using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inRiverCommunity.Connectivity.iPMC.Tests.Helpers
{
    public static class RestApiKeyHelper
    {

        private const string RestApiKeyFilePath = @"..\..\..\..\rest_api.key";

        public static string GetRestApiKeyFromFile()
        {
            /* Reads the REST API key from the file rest_api.key.
             * TODO: Add file rest_api.key to the solution folder with just the key as contents. 
             * NOTE: the file rest_api.key is ignored by git, see .gitignore.
             */
            Assert.IsTrue(File.Exists(RestApiKeyFilePath), @"File rest_api.key does not exist in the solution folder!");

            string fileContents = File.ReadAllText(RestApiKeyFilePath);

            Match match = Regex.Match(fileContents, @"\w+");
            Assert.IsTrue(match.Success, $@"Invalid REST API key stored in file: {RestApiKeyFilePath}!");

            return match.Value;
        }

    }
}
