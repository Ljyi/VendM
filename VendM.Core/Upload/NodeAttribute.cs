using System;

namespace VendM.Core.Upload
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NodeAttribute : Attribute
    {
        public NodeAttribute()
        {
            this.Type = NodeAttribute.NodeType.Simple;
        }

        public NodeAttribute(string nodename, NodeAttribute.NodeType type = NodeAttribute.NodeType.Simple)
        {
            this.NodeName = nodename;
            this.Type = type;
        }

        public string NodeName { get; set; }

        public NodeAttribute.NodeType Type { get; set; }

        public enum NodeType
        {
            Simple,
            Class,
            List,
        }
    }
}
