using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mime;

namespace Library.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : Controller
    {
        private readonly IEntityRepository entityRepository;

        public FileController(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        [HttpGet("[action]/{*fileName}")]
        public ActionResult Download(string fileName)
        {
            return RetrieveFile(fileName, true);
        }

        [HttpGet("{*fileName}")]
        public ActionResult Get(string fileName)
        {
            return RetrieveFile(fileName, false);
        }

        private ActionResult RetrieveFile(string fileName, bool shouldDownload)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                int fileExtensionStart = fileName.LastIndexOf(".");
                string id;

                if (fileExtensionStart != -1)
                {
                    id = fileName.Substring(0, fileExtensionStart);
                }
                else
                {
                    id = fileName;
                }

                Guid fileId;
                if (Guid.TryParse(id, out fileId))
                {
                    var fileRequest = BuildFileRequest(fileId);
                    var fileResponse = entityRepository.Get(fileRequest).FirstOrDefault();

                    if (fileResponse == null)
                    {
                        return new NotFoundResult();
                    }
                    else if (!fileResponse.Contains("ContentType")
                        || !fileResponse.Contains("Content")
                        || !fileResponse.Contains("Name")
                        || string.IsNullOrEmpty(fileResponse["ContentType"] as string)
                        || string.IsNullOrEmpty(fileResponse["Name"] as string))
                    {
                        return StatusCode(500, "Malformed file content");
                    }

                    string contentType = (string)fileResponse["ContentType"];
                    string name = (string)fileResponse["Name"];
                    byte[] content = (byte[])fileResponse["Content"];

                    if (shouldDownload)
                    {
                        ContentDisposition cd = new ContentDisposition
                        {
                            FileName = name,
                            Inline = false  // false = prompt the user for downloading; true = browser to try to show the file inline
                        };
                        Response.Headers.Add("Content-Disposition", cd.ToString());
                        Response.Headers.Add("X-Content-Type-Options", "nosniff");
                        return File(content, contentType);
                    }
                    else
                    {
                        return File(content, contentType);
                    }
                }
            }

            return new NotFoundResult();
        }

        private QueryExpression BuildFileRequest(Guid fileId)
        {
            ConditionExpression condition = new ConditionExpression("Id", ConditionOperator.Equal, fileId);
            QueryExpression query = new QueryExpression("File");
            query.ColumnSet.AddColumns("Content", "ContentType", "Name");
            query.Criteria.AddQuery(condition);

            return query;
        }
    }
}
