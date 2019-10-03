using System.ComponentModel;

namespace VendM.Model.EnumModel
{
    public enum AdvertisementEnum
    {
        [Description("上线")]
        Online = 0,
        [Description("下线")]
        Offline = 1,
        [Description("其他")]
        Other = 2
    }
}
