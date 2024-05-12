using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThanhGiang_WebCuoiKi.Models
{
    public class ModelSanpham
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { set; get; }
    }
}