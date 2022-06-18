using AussieCake.Util;
using System.ComponentModel;

namespace AussieCake
{
    public enum Model
    {
        [Description("Vocabulary")]
        Voc,

        [Description("Sentence")]
        Sen,

        [Description("Verb")]
        Verb,

        [Description("Essay")]
        Essay,

        [Description("SumRetell")]
        SumRetell,

        [Description("DescribeImage")]
        DescImg,

        [Description("Pronunciation")]
        Pron,

        [Description("Spelling")]
        Spell,

        [Description("Pearson_WD")]
        P_Dit,

        [Description("Pearson_FIB")]
        P_FIB,
    }

    public enum PearsonType
    {
        [Description("Pearson_SST")] // 0
        SST,

        [Description("Pearson_RL")] // 1
        RL,

        [Description("Pearson_RA")] // 2
        RA,

        [Description("Pearson_FIBR")] // 3
        FIB_R,

        [Description("Pearson_FIBRW")] // 4
        FIB_RW,

        [Description("Pearson_RS")] // 5
        RS,

        [Description("Pearson_WFD")] // 6
        WFD,

        [Description("Pearson_ASQ")] // 7
        ASQ,
    }

    public static class PearsonTypeHelper
    {
        public static string PersonModelToFileAbvMp3(PearsonType type)
        {
            switch (type)
            {
                case PearsonType.SST:
                    return "sst";
                case PearsonType.RL:
                    return "rl";
                case PearsonType.RA:
                    return "ra";
                case PearsonType.FIB_R:
                    return "fibr";
                case PearsonType.FIB_RW:
                    return "fibrw";
                case PearsonType.RS:
                    return "rs";
                case PearsonType.WFD:
                    return "wfd";
                case PearsonType.ASQ:
                    return "asq";
                default:
                    Errors.ThrowErrorMsg(ErrorType.InvalidModelType, type);
                    return string.Empty;
            }
        }

        public static PearsonType GetTypeFromPearson(int pearsonType)
        {
            switch (pearsonType)
            {
                case 0:
                    return PearsonType.SST;
                case 1:
                    return PearsonType.RL;
                case 2:
                    return PearsonType.RA;
                case 3:
                    return PearsonType.FIB_R;
                case 4:
                    return PearsonType.FIB_RW;
                case 5:
                    return PearsonType.RS;
                case 6:
                    return PearsonType.WFD;
                case 7:
                    return PearsonType.ASQ;
            }

            return PearsonType.RS;
        }
    }
}
