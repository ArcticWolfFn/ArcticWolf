using ArcticWolfApi.Exceptions;
using ArcticWolfApi.Models.Cloudstorage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("fortnite/api/[controller]")]
    [ApiController]
    public class CloudstorageController : ControllerBase
    {
        [HttpGet("system")]
        public ActionResult<List<CloudstorageFile>> GetCloudstorageSystemFiles()
        {
            switch (Request.GetSeasonNumber())
            {
                case 9:
                case 10:
                case 17:
                    throw new NotFoundException();

                default:
                    return ((IEnumerable<string>)Directory.GetFiles(Path.Join("Resources", "Cloudstorage", "System"))).Select(file => new CloudstorageFile(file)
                    {
                        UniqueFilename = GetCloudstorageFile(Path.GetFileNameWithoutExtension(file))
                    }).ToList();
            }
        }

        [HttpGet("system/config")]
        public ActionResult<Config> GetCloudstorageSystemConfig() => new Config();

        [HttpGet("system/{uniqueFilename}")]
        public ActionResult GetCloudstorageSystemFile(string uniqueFilename)
        {
            string str = Path.Join("Resources", "Cloudstorage", "System", GetCloudstorageFile(uniqueFilename));

            return System.IO.File.Exists(str + ".ini") ? File(System.IO.File.OpenRead(str + ".ini"), "application/octet-stream") : throw new NotFoundException();
        }

        [HttpGet("user/{accountId}")]
        public ActionResult<List<CloudstorageFile>> GetCloudstorageUserFiles(
          string accountId)
        {
            string path = Path.Join("Resources", "Cloudstorage", "ClientSettings");
            Directory.CreateDirectory(path);

            int clNum = Request.GetCLNumber();

            List<CloudstorageFile> list = ((IEnumerable<string>)Directory.GetFiles(path)).Select(file =>
            {
                if (!file.Contains(string.Format("ClientSettings-{0}", clNum)))
                {
                    return null;
                }

                return new CloudstorageFile(file)
                {
                    FileName = "ClientSettings.Sav",
                    UniqueFilename = "ClientSettings.Sav"
                };
            }).ToList();

            list.RemoveAll(x => x == null);

            return list;
        }

        [HttpGet("user/{accountId}/{uniqueFilename}")]
        public ActionResult GetCloudstorageUserFile(string accountId, string uniqueFilename)
        {
            string path1 = Path.Join("Resources", "Cloudstorage", "ClientSettings");
            int clNumber = Request.GetCLNumber();
            return System.IO.File.Exists(Path.Join(path1, string.Format("ClientSettings-{0}.Sav", clNumber))) 
                ? 
                File(System.IO.File.OpenRead(Path.Join(path1, string.Format("ClientSettings-{0}.Sav", clNumber))), "application/octet-stream")
                : 
                throw new Exceptions.Cloudstorage.FileNotFoundException(uniqueFilename);
        }

        [HttpPut("user/{accountId}/{uniqueFilename}")]
        [RequestSizeLimit(5000000)]
        public async Task<ActionResult> UpdateCloudstorageUserFile(
          string accountId,
          string uniqueFilename)
        {
            if (!uniqueFilename.Contains("ClientSettings"))
            {
                throw new Exceptions.Cloudstorage.FileNotFoundException(uniqueFilename);
            }

            string cloudstoragePath = Path.Join("Resources", "Cloudstorage", "ClientSettings");
            int clNum = Request.GetCLNumber();

            if (!System.IO.File.Exists(Path.Join(cloudstoragePath, string.Format("ClientSettings-{0}.Sav", clNum))))
            {
                await System.IO.File.WriteAllTextAsync(Path.Join(cloudstoragePath, string.Format("ClientSettings-{0}.Sav", clNum)), "");
            }

            FileStream input = System.IO.File.OpenWrite(Path.Join(cloudstoragePath, string.Format("ClientSettings-{0}.Sav", clNum)));
            await Request.Body.CopyToAsync(input);
            input.Close();

            return NoContent();
        }

        private static string GetCloudstorageFile(string file)
        {
            Dictionary<string, string> source = new()
            {
                {
                    "DefaultGame",
                    "a22d837b6a2b46349421259c0a5411bf"
                },
                {
                    "DefaultEngine",
                    "3460cbe1c57d4a838ace32951a4d7171"
                },
                {
                    "DefaultInput",
                    "mhl5jvb7fm85e157u49k1lbf8p9kpj50"
                },
                {
                    "DefaultRuntimeOptions",
                    "mhl5jvb7fm85e157u49k1lbf8p9kpj60"
                }
            };

            return !source.ContainsKey(file) 
                ? 
                source.FirstOrDefault(x => x.Value == file).Key 
                : 
                source[file];
        }
    }
}
