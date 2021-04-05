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
    }
}