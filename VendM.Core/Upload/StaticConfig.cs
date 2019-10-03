using System;
using System.Collections.Generic;


namespace VendM.Core.Upload
{
    class StaticConfig
    {
        [Node("Static/Entity", NodeAttribute.NodeType.List)]
        public List<StaticConfigEntity> EntityList { get; set; }
    }

    class StaticConfigEntity
    {
        /// <summary>
        /// 上传者身份
        /// </summary>
        [Node]
        public string name { get; set; }

        /// <summary>
        /// ftp地址
        /// </summary>
        [Node]
        public string ftpUrl { get; set; }

        /// <summary>
        /// ftp端口
        /// </summary>
        [Node]
        public int ftpPort { get; set; }

        /// <summary>
        /// ftp用户名
        /// </summary>
        [Node]
        public string ftpUsername { get; set; }

        /// <summary>
        /// ftp密码
        /// </summary>
        [Node]
        public string ftpPassword { get; set; }

        /// <summary>
        /// 上传文件的ftp路径
        /// </summary>
        [Node]
        public string ftpPath { get; set; }

        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        [Node]
        public int fileSize { get; set; }

        /// <summary>
        /// 返回上传文件的url
        /// </summary>
        [Node]
        public string httpUrl { get; set; }

        /// <summary>
        /// 是否重新生成文件名
        /// </summary>
        [Node]
        public Boolean generateFileName { get; set; }

        /// <summary>
        /// 是否生成时间的子目录
        /// </summary>
        [Node]
        public Boolean generateTimeSubdirectory { get; set; }

        /// <summary>
        /// 子目录模式(yyyyMMdd yyyy/MM/dd 等)仅当generateTimeSubdirectory为true时有效
        /// </summary>
        [Node]
        public string subdirectoryModel { get; set; }

        /// <summary>
        /// 文件扩展名子节点
        /// </summary>
        [Node("ItemList/Item", NodeAttribute.NodeType.List)]
        public List<Item> FileExtList { get; set; }

        /// <summary>
        /// 文件扩展名子节点
        /// </summary>
        [Node("Watermark", NodeAttribute.NodeType.List)]
        public List<Watermark> WatermarkList { get; set; }
    }

    class Item
    {
        /// <summary>
        /// 扩展名
        /// </summary>
        [Node]
        public string fileExtension { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Watermark
    {
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public int Position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public int Quality { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public string WaterWord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public string WaterImgUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public string WordFont { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Node]
        public int FontSize { get; set; }

        public float Alpha
        {
            get { return (float) Quality/100; }
        }
    }
}