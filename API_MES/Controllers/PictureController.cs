using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using API_MES.Options;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using EF_ORACLE.Model.TMES;
using API_MES.Servise;
using Microsoft.AspNetCore.Cors;
using API_MES.Helper;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_MES.Controllers
{
    //[ApiVersion("2")]
    //[ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("any")]
    [ApiVersion("2")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SkipActionFilter]
    public class PictureController : ControllerBase
    {
        // MongodbHost信息
        private readonly MongodbHostOptions _mongodbHostOptions;
        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };
        // 图片选项
        private readonly PictureOptions _pictureOptions;
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IMesRepository _mesRepository;
        private readonly OtherInterface _otherInterface;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="mongodbHostOptions">MongodbHost信息</param>
        /// <param name="pictureOptions">图片选项</param>
        public PictureController(IOptions<MongodbHostOptions> mongodbHostOptions, IOptions<PictureOptions> pictureOptions,IWebHostEnvironment env, IMesRepository MesRepository,OtherInterface otherInterface)
        {
            _mongodbHostOptions = mongodbHostOptions.Value;
            _pictureOptions = pictureOptions.Value;
            //_routineDbContext = routineDbContext ?? throw new ArgumentNullException(nameof(routineDbContext));
            this.hostingEnv = env ?? throw new ArgumentNullException(nameof(env));
            _mesRepository = MesRepository ?? throw new ArgumentNullException(nameof(MesRepository));
            _otherInterface = otherInterface ?? throw new ArgumentNullException(nameof(otherInterface));
        }
        [HttpGet]
        public IActionResult Get()
        {
            //var MO = "51SF0620030000019";
            var ss = "8S%SB18C51767%9421056143ATL19BF------------NVT6AA6874";

            //IMGFIRST iMG_FIRST = new IMGFIRST
            //{
            //    ID = new Guid(),
            //    IMGNAME = "3424324",
            //    MO = MO,
            //    LINE = ss,
            //    MTYPE ="2"
            //};
            //_routineDbContext.IMGFIRSTs.Add(iMG_FIRST);
            //_routineDbContext.SaveChanges();
            //var  ddd =_routineDbContext.IMGFIRSTs.ToList();
            return Ok(ss);
        }


        [HttpGet]
        [Route("DEL_LENOVOSFTP")]
        public async Task<IActionResult> DEL_LENOVOSFTP()
        {
            var Result = await _otherInterface.DEL_LENOVOSFTP();
            return Ok(Result);
        }
        /// <summary>
        /// 接口上传图片方法
        /// </summary>
        /// <param name="fileDtos">文件传输对象,传过来的json数据</param>
        /// <returns>上传结果</returns>
        //[HttpPost]
        //public async Task<UploadResult> Post([FromBody] FileDtos fileDtos)
        //{
        //    UploadResult result = new UploadResult();
        //    if (ModelState.IsValid)
        //    {
        //        #region  验证通过
        //        //首先根据api参数判断是否为图片类型，是则处理，不是则返回对应的结果
        //        if (!string.IsNullOrEmpty(fileDtos.Type) && fileDtos.Type.ToLower() == "image")
        //        {
        //            //文件类型
        //            string FileEextension = Path.GetExtension(fileDtos.Filename).ToLower();//获取文件的后缀
        //                                                                                   //判断文件类型是否是允许的类型
        //            if (_pictureOptions.FileTypes.Split(',').Contains(FileEextension))
        //            {
        //                //图片类型是允许的类型
        //                Images_Mes fmster = new Images_Mes();//图片存储信息类，跟MongoDB里面表名一致
        //                string fguid = Guid.NewGuid().ToString().Replace("-", ""); //文件名称
        //                fmster.AddTime = DateTimeOffset.Now;//添加时间为当前时间
        //                fmster.AddUser = "server";//具体根据你的业务来获取
        //                if (Base64Helper.IsBase64String(fileDtos.Base64String, out byte[] fmsterByte))
        //                {
        //                    //判断是否是base64字符串，如果是则转换为字节数组，用来保存
        //                    fmster.FileCon = fmsterByte;
        //                }
        //                fmster.FileName = Path.GetFileName(fileDtos.Filename);//文件名称
        //                fmster.FileSize = fmster.FileCon.Length;//文件大小
        //                fmster.FileType = FileEextension;//文件扩展名
        //                fmster.GuidID = fguid;//唯一主键，通过此来获取图片数据
        //                await MongodbHelper<Images_Mes>.AddAsync(_mongodbHostOptions, fmster);//上传文件到mongodb服务器
        //                                                                                      //检查是否需要生产缩略图
        //                if (_pictureOptions.MakeThumbnail)
        //                {
        //                    //生成缩略图
        //                    Images_Mes fthum = new Images_Mes();
        //                    fthum.AddTime = DateTimeOffset.Now;
        //                    fthum.AddUser = "server";//具体根据你的业务来获取
        //                    fthum.FileCon = ImageHelper.GetReducedImage(fmster.FileCon, _pictureOptions.ThumsizeW, _pictureOptions.ThumsizeH);
        //                    fthum.FileName = Path.GetFileNameWithoutExtension(fileDtos.Filename) + "_thumbnail" + Path.GetExtension(fileDtos.Filename);//生成缩略图的名称
        //                    fthum.FileSize = fthum.FileCon.Length;//缩略图大小
        //                    fthum.FileType = FileEextension;//缩略图扩展名
        //                    fthum.GuidID = fguid + _pictureOptions.ThumbnailGuidKeys;//为了方面，缩略图的主键为主图主键+一个字符yilezhu作为主键
        //                    await MongodbHelper<Images_Mes>.AddAsync(_mongodbHostOptions, fthum);//上传缩略图到mongodb服务器
        //                }
        //                result.Errcode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
        //                result.Errmsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
        //                UploadEntity entity = new UploadEntity();
        //                entity.Picguid = fguid;
        //                entity.Originalurl = _pictureOptions.ImageBaseUrl + fguid;
        //                entity.Thumburl = _pictureOptions.ImageBaseUrl + fguid + _pictureOptions.ThumbnailGuidKeys;
        //                result.Data = entity;
        //                return result;
        //            }
        //            else
        //            {
        //                //图片类型不是允许的类型
        //                result.Errcode = ResultCodeAddMsgKeys.HttpFileInvalidCode;//对应的编码
        //                result.Errmsg = ResultCodeAddMsgKeys.HttpFileInvalidMsg;//对应的错误信息
        //                result.Data = null;//数据为null
        //                return result;
        //            }

        //        }
        //        else
        //        {
        //            result.Errcode = ResultCodeAddMsgKeys.HttpFileNotFoundCode;
        //            result.Errmsg = ResultCodeAddMsgKeys.HttpFileNotFoundMsg;
        //            result.Data = null;
        //            return result;
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        #region 验证不通过
        //        StringBuilder errinfo = new StringBuilder();
        //        foreach (var s in ModelState.Values)
        //        {
        //            foreach (var p in s.Errors)
        //            {
        //                errinfo.AppendFormat("{0}||", p.ErrorMessage);
        //            }
        //        }
        //        result.Errcode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
        //        result.Errmsg = errinfo.ToString();
        //        result.Data = null;
        //        return result;
        //        #endregion
        //    }
        //}
        #region
        //[HttpPost]
        //public ActionResult UploadFile(IEnumerable<FileDtos> filename)
        //{
        //    foreach (var file in filename)
        //    {
        //        if (file.ContentLength > 0)
        //        {
        //            ImageStore imageStore = new ImageStore();

        //            imageStore.Name = Path.GetFileName(file.FileName);
        //            imageStore.MimeType = file.ContentType;

        //            using (Stream inputStream = file.InputStream)
        //            {
        //                MemoryStream memoryStream = inputStream as MemoryStream;
        //                if (memoryStream == null)
        //                {
        //                    memoryStream = new MemoryStream();
        //                    inputStream.CopyTo(memoryStream);
        //                }
        //                imageStore.Content = memoryStream.ToArray();
        //            }

        //            ImageStoreEntity ise = new ImageStoreEntity();
        //            ise.Insert(imageStore);
        //        }
        //    }

        //    return RedirectToAction("ImageManagement");
        //}
        #endregion
        ///// <summary>
        ///// 删除图片
        ///// </summary>
        ///// <param name="guid">原始图片主键</param>
        ///// <returns>执行结果</returns>
        //[HttpDelete("{guid}")]
        //public async Task<BaseResult> Delete(string guid)
        //{
        //    await MongodbHelper<Images_Mes>.DeleteAsync(_mongodbHostOptions, guid);//删除mongodb服务器上对应的文件
        //    await MongodbHelper<Images_Mes>.DeleteAsync(_mongodbHostOptions, guid + _pictureOptions.ThumbnailGuidKeys);//删除mongodb服务器上对应的文件
        //    return new BaseResult(ResultCodeAddMsgKeys.CommonObjectSuccessCode, ResultCodeAddMsgKeys.CommonObjectSuccessMsg);
        //}

        ///// <summary>
        ///// 返回图片对象
        ///// </summary>
        ///// <param name="guid">图片的主键</param>
        ///// <returns>图片对象</returns>
        //[Route("Show/{guid}")]
        //[HttpGet]
        //public async Task<FileResult> ShowAsync(string guid)
        //{
        //    if (string.IsNullOrEmpty(guid))
        //    {
        //        return null;
        //    }
        //    FilterDefinition<Images_Mes> filter = Builders<Images_Mes>.Filter.Eq("GuidID", guid);
        //    var result = await MongodbHelper<Images_Mes>.FindListAsync(_mongodbHostOptions, filter);
        //    if (result != null && result.Count > 0)
        //    {
        //        return File(result[0].FileCon, "image/jpeg", result[0].FileName);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //} 
        ///// <summary>
        ///// 文件流的方式输出        /// </summary>
        ///// <returns></returns>
        //public IActionResult DownLoad(string file)
        //{
        //    var addrUrl = file;
        //    var stream = System.IO.File.OpenRead(addrUrl);
        //    string fileExt = GetFileExt(file);
        //    //获取文件的ContentType
        //    var provider = new FileExtensionContentTypeProvider();
        //    var memi = provider.Mappings[fileExt];
        //    return File(stream, memi, Path.GetFileName(addrUrl));
        //}
        [HttpPost("Photos")]
        public IActionResult UploadPhotos(IFormFileCollection files)
        {
            long size = files.Sum(f => f.Length);
            //var fileFolder = Path.Combine(_env.WebRootPath, "Photos");

            //if (!Directory.Exists(fileFolder))
            //    Directory.CreateDirectory(fileFolder);

            //foreach (var file in files)
            //{
            //    if (file.Length > 0)
            //    {
            //        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +
            //                       Path.GetExtension(file.FileName);
            //        var filePath = Path.Combine(fileFolder, fileName);

            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await file.CopyToAsync(stream);
            //        }
            //    }
            //}

            return Ok(new { count = files.Count, size });
        }
        [HttpPost]
        [Route("Post2")]
        public async Task<IActionResult> Post2()
        {
            var MO = Request.Form["MO"].ToString();
            var LINE = Request.Form["LINE"].ToString();
            var MYTYPE = Request.Form["MYTYPE"].ToString();
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 100MB refuse upload !
            if (size > 104857600)
            {
                return Ok(("pictures total size > 100MB , server refused !"));
            }
            var imgs =await GetIMGLOG(MO, LINE, MYTYPE);
            if (imgs!=null)
            {
               
            }
            List<string> filePathResultList = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var unfileName = fileName;
                string basepath = hostingEnv.WebRootPath;        //en.WebRootPath-》wwwroot的目录; .ContentRootPath到达WebApplication的项目目录
                //string path_server = basepath + "\\upfile\\" + path;
                string filePath = hostingEnv.WebRootPath + $@"\Files\Pictures\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string suffix = fileName.Split('.')[1];

                if (!pictureFormatArray.Contains(suffix))
                {
                    return Ok("the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'.");
                }

                fileName = Guid.NewGuid() + "." + suffix;

                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                filePathResultList.Add($"/Files/Pictures/{fileName}");
                FQA_FIRSTARTICLEIMG fQA_FIRSTARTICLEIMG = new FQA_FIRSTARTICLEIMG
                {
                    ID = Guid.NewGuid().ToString(),
                    IMGNAME= unfileName,
                    IMGLIST= $"/Files/Pictures/{fileName}",
                    LINE=LINE,
                    MO=MO,
                    MTYPE=Convert.ToInt32(MYTYPE),
                    IMGNEW= fileName,
                };
                  _mesRepository.AddFQA_FIRSTARTICLEIMG(fQA_FIRSTARTICLEIMG);
                await  _mesRepository.SaveAsync();
            }

            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!"; 
            return Ok(new JsonResult(new { message, filePathResultList , filePathResultList.Count }));
        }
        [HttpDelete]
        [Route("IMGLOG")]
        public async Task<IActionResult> IMGLOG(string MO, string LINE, string Type)
        {
            if (_mesRepository.DeleteFQA_FIRSTARTICLEIMG(MO, LINE, Type)) { await _mesRepository.SaveAsync(); } 
            string dd = "删除完成";
            return Ok(dd);
        }
        [HttpGet]
        [Route("GetIMGLOG")]
        public async Task<IActionResult> GetIMGLOG(string MO, string LINE, string Type)
        {
            var Result =await _mesRepository.GetfQA_FIRSTARTICLEIMG(MO, LINE, Type);
            return Ok(Result);
        }
        [HttpDelete]
        [Produces("application/json",
            "application/vnd.company.hateoas+json",
            "application/vnd.company.company.friendly+json",
            "application/vnd.company.company.friendly.hateoas+json",
            "application/vnd.company.company.full+json",
            "application/vnd.company.company.full.hateoas+json")]
        [Route("DELETEIMG")]
        public async Task<IActionResult> DELETEIMG(string id)
        {
            var de = await _mesRepository.GetfQA_FIRSTARTICLEIMGAsync(id);
            var imgNmae = de.IMGNEW;
            _mesRepository.DeleteFQA_FIRSTARTICLEIMGls(de);
            await _mesRepository.SaveAsync();
            if (imgNmae!="")
            {
                string filePath = hostingEnv.WebRootPath + $@"\Files\Pictures\";
                string fileFullPath = filePath + imgNmae;
                if (System.IO.File.Exists(fileFullPath))
                {
                    // 2、根据路径字符串判断是文件还是文件夹
                    //FileAttributes attr = File.GetAttributes(fileFullPath);
                    //// 3、根据具体类型进行删除
                    //if (attr == FileAttributes.Directory)
                    //{
                    //    // 3.1、删除文件夹
                    //    Directory.Delete(fileFullPath, true);
                    //}
                    //else
                    //{
                    //    // 3.2、删除文件
                    //    File.Delete(fileFullPath);
                    //}
                    System.IO.File.Delete(fileFullPath);
                } 
            }
            return Ok(imgNmae);
        }
    }
} 