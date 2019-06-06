using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchLink.Data.Models
{


    public class Sessions_Triton_TMK
    {
        public string TMK { get; set; }
        public string TMK_CHK { get; set; }
    }

    public class Sessions_Triton_Location
    {
        public string name_location_name { get; set; }
        public string name_location_city { get; set; }
        public string name_location_state { get; set; }
        public string name_location_country { get; set; }  
    }

    public class Sessions_Core_ZPK_to_TPK
    {
        public  string TPK_LMK { get; set; }
        public string ZPK_LMK { get; set; }
    }

    public class TerminalDetails
    {
        public string TPK_TMK { get; set; }
     
    }

    public class Trnaslate_Zpk_Lmk
    {
        public string ZPK_LMK { get; set; }
    }

    public class GetKeks
    {
        public string Keks { get; set; }
    }

    public class GetKekr
    {
        public string Kekr { get; set; }
    }
}
