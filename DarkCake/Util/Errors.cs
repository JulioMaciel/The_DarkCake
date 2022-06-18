using System.ComponentModel;
using System.Media;

namespace AussieCake.Util
{
    public static class Errors
    {
        public static bool ThrowErrorMsg(ErrorType errorType, object error)
        {
            Footer.LogError("Error: " + errorType.ToDesc(), error);
            SystemSounds.Hand.Play();

            return false;
        }

        public static bool IsNullSmallerOrBigger(string original, int minimum, int maximum, bool showErrorsbox)
        {
            if (original.IsEmpty())
                return showErrorsbox ? !ThrowErrorMsg(ErrorType.NullOrEmpty, original) : true;
            else if (original.Length < minimum)
                return showErrorsbox ? !ThrowErrorMsg(ErrorType.TooSmall, original) : true;
            else if (original.Length > maximum)
                return showErrorsbox ? !ThrowErrorMsg(ErrorType.TooBig, original) : true;

            return false;
        }

        public static bool IsDigitsOnly(string input)
        {
            if (!input.IsDigitsOnly())
                return ThrowErrorMsg(ErrorType.InvalidCharacters, input);

            return true;
        }
    }

    public enum ErrorType
    {
        [Description("AlreadyInserted")]
        AlreadyInserted,

        [Description("TooSmall")]
        TooSmall,

        [Description("InvalidModelType")]
        InvalidModelType,

        [Description("NullOrEmpty")]
        NullOrEmpty,

        [Description("InvalidCharacters")]
        InvalidCharacters,

        [Description("TooBig")]
        TooBig,

        [Description("Inexistent")]
        Inexistent,

        [Description("SQLite")]
        SQLite,

        [Description("InitUnrealItem")]
        InitUnrealItem,

        [Description("LoadNonInitItem")]
        LoadNonInitItem,

        [Description("NoPunctuation")]
        NoPunctuation,

        [Description("InitialLowerCase")]
        InitialLowerCase,

        [Description("Inexistent word or site off")]
        InexistentWordOrSiteOff,

        [Description("InvalidLoadStatus")]
        InvalidLoadStatus,

        [Description("SynonymsNotFound")]
        SynonymsNotFound,

        [Description("AnswerNotFound")]
        AnswerNotFound,
    }
}
