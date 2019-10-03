using System.Collections.Generic;
namespace VendM.Model
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuTree
    {
        public MenuTree()
        {
            menuTrees = new List<MenuTree>();
        }
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuCode { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单级别
        /// </summary>
        public int MenuLevel { get; set; }
        /// <summary>
        /// 父节点Id
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        ///子菜单
        /// </summary>
        public List<MenuTree> menuTrees { get; set; }
    }
    /// <summary>
    /// 菜单树
    /// </summary>
    public class MenuZTree
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 菜单父级Id
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool isChecked { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool chkDisabled { get; set; }
    }
}
