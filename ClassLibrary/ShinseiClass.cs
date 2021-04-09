using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WhereEver.ClassLibrary
{
    /// <summary>
    ///　各申請書類のValidationControlを行うPublicクラスです。
    /// </summary>
    public class ShinseiClass
    {
        [Required(ErrorMessage =" ")]
        public string purchaseName
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string classification
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string howMany
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string howMach
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string marketPlace
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string buy_purpose
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string Notification_Purpose
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_Date
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_W_Place
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_Use
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_In
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_Out
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_T_Waste
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_Place
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_P_Waste
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_ps
        {
            get;
            set;
        }
        [Required(ErrorMessage = " ")]
        public string T_Teiki
        {
            get;
            set;
        }
    }
}