using SuperManCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperManWebApi.Models.Business
{
    public class UploadIconModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string status { get; set; }
    }
    public enum UploadIconStatus
    {
        Success,

        [DisplayText("未传过来任何变量")]
        NOFormParameter,

        [DisplayText("无效的用户")]
        InvalidUserId,

        [DisplayText("真实姓名不能为空")]
        TrueNameEmpty,

        [DisplayText("无效的文件格式")]
        InvalidFileFormat,

        [DisplayText("图片的尺寸最小为150px*150px")]
        InvalidImageSize,
    }
}