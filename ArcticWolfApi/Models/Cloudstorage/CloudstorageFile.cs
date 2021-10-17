using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Cloudstorage
{
    public class CloudstorageFile
    {
        public CloudstorageFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            this.FileName = fileInfo.Name;
            this.Hash = string.Concat(((IEnumerable<byte>)new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(File.ReadAllText(filePath)))).Select<byte, string>((Func<byte, string>)(b => b.ToString("x2"))));
            this.Hash256 = string.Concat(((IEnumerable<byte>)new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(File.ReadAllText(filePath)))).Select<byte, string>((Func<byte, string>)(b => b.ToString("x2"))));
            this.Length = fileInfo.Length;
            this.Uploaded = fileInfo.LastWriteTime;
        }

        [JsonProperty("uniqueFilename")]
        public string UniqueFilename { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; }

        [JsonProperty("hash256")]
        public string Hash256 { get; }

        [JsonProperty("length")]
        public long Length { get; }

        [JsonProperty("contentType")]
        public string ContentType => "application/octet-stream";

        [JsonProperty("uploaded")]
        public DateTime Uploaded { get; }

        [JsonProperty("storageType")]
        public string StorageType => "S3";

        [JsonProperty("doNotCache")]
        public bool DoNotCache => true;
    }
}
