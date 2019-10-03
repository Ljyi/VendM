using System.Collections.Generic;


namespace VendM.Core.Upload
{
    class DeleteStaticConfig
    {
        [Node("DelStatic/DelEntity", NodeAttribute.NodeType.List)]
        public List<DeleteConfigEntity> EntityList { get; set; }
    }

    class DeleteConfigEntity
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
        /// 文件扩展名子节点
        /// </summary>
        [Node("ItemList/Item", NodeAttribute.NodeType.List)]
        public List<Extension> FileExtList { get; set; }

    }
    class Extension
    {
        /// <summary>
        /// 扩展名
        /// </summary>
        [Node]
        public string fileExtension { get; set; }
    }
}