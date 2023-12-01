using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Specialized;
using ICSharpCode.SharpZipLib.BZip2;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SharpCompress.Compressors.BZip2;

namespace lexfunction
{
    public class lexunzip
    {
        [FunctionName("lexunzip")]
        public static async Task Run([BlobTrigger("input-files/{name}", Connection = "lexstorageacc_STORAGE")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name}");

            try 
            {
                if (name.Split('.').Last().ToLower() == "bz2")
                {              
                    //BZip2Stream zip = new BZip2Stream(myBlob, SharpCompress.Compressors.CompressionMode.Decompress, true);
                    BZip2InputStream stream = new BZip2InputStream(myBlob);
                    var memory = new MemoryWebClient(/*Kernel memory service URL here*/);

                    await memory.ImportDocumentAsync(stream);         
                }
            }
            catch(Exception ex)
            {
                log.LogInformation($"Error! Something went wrong: {ex.Message}");
                log.LogInformation(ex.StackTrace);
            } 
        }
    }
}
