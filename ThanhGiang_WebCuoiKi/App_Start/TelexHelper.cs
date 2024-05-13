using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhGiang_WebCuoiKi.App_Start
{
    public class TelexHelper
    {
        public static string RemoveTelexMarks(string input)
        {
            // Tạo bảng dịch ký tự
            string[] telexChars = { "aàáảãạăằắẳẵặâầấẩẫậ", "dđ", "eèéẻẽẹêềếểễệ", "iìíỉĩị", "oòóỏõọôồốổỗộơờớởỡợ", "uùúủũụưừứửữự", "yỳýỷỹỵ" };

            // Loại bỏ dấu thanh
            foreach (string telexSet in telexChars)
            {
                foreach (char c in telexSet.Substring(1))
                {
                    input = input.Replace(c, telexSet[0]);
                }
            }

            return input;
        }

    }
}