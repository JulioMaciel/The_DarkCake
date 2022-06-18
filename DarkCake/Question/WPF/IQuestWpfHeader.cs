using AussieCake.Util.WPF;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public interface IQuestWpfHeader
    {
        StackPanel Stk_items { get; set; }

        StackPanel Stk_insert { get; set; }
        Grid Grid_bulk_insert { get; set; }

        ComboBox Cob_bulk_imp { get; set; }
        TextBox Txt_bulk_insert { get; set; }

        TextBox Txt_avg_w { get; set; }
        TextBox Txt_avg_m { get; set; }
        TextBox Txt_avg_all { get; set; }
        TextBox Txt_tries { get; set; }
        TextBox Txt_chance { get; set; }

        Label Lbl_avg_w { get; set; }
        Label Lbl_avg_m { get; set; }
        Label Lbl_avg_all { get; set; }
        Label Lbl_tries { get; set; }
        Label Lbl_sen { get; set; }
        Label Lbl_chance { get; set; }
        Label Lbl_imp { get; set; }

        ComboBox Cob_imp { get; set; }
        ButtonActive Btn_isActive { get; set; }
        Button Btn_insert { get; set; }
        Button Btn_filter { get; set; }
    }
}
