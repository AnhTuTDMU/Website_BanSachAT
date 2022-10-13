using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website_BanSachAT.Models
{
    
    public class GioHang
    {
        SachOnlineDataContext db = new SachOnlineDataContext();

        public int iMasach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public double sDonGia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * sDonGia; }
        }
        public GioHang(int ms)
        {
            iMasach = ms;
            SACH s = db.SACHes.Single(n => n.MaSach == iMasach);
            sTenSach = s.TenSach;
            sAnhBia = s.AnhBia;
            sDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }


    }
}