using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.Enums
{
    public enum LCLASSEnum
    {
        [Description("甲種建築用地")]
        EA = 0,

        [Description("乙種建築用地")]
        EB = 1,

        [Description("丙種建築用地")]
        EC = 2,

        [Description("丁種建築用地")]
        ED = 3,

        [Description("農牧用地")]
        EE = 4,

        [Description("礦業用地")]
        EF = 5,

        [Description("交通用地")]
        EG = 6,

        [Description("水利用地")]
        EH = 7,

        [Description("遊憩用地")]
        EJ = 8,

        [Description("古蹟保存用地")]
        EK = 9,

        [Description("生態保護用地")]
        EL = 10,

        [Description("國土保安用地")]
        EM = 11,

        [Description("殯葬用地")]
        EN = 12,

        [Description("特定目的事業用地")]
        EP = 13,

        [Description("鹽業用地")]
        EQ = 14,

        [Description("窯業用地")]
        ER = 15,

        [Description("林業用地")]
        ES = 16,

        [Description("養殖用地")]
        ET = 17,

        [Description("暫未編定")]
        EZ = 18,

    }
}
