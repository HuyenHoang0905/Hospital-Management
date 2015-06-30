using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ChoKhamCLS_Entities
    {
        public int MaBenhNhan;
        public int MaLoaiKhamCLS;
        public int MaBSYeuCau;
        public string TinhTrangThanhToan;

        public int maBenhNhan
        {
            get { return MaBenhNhan; }
            set { MaBenhNhan = value; }
        }

        public int maLoaiKhamCLS
        {
            get { return MaLoaiKhamCLS; }
            set { MaLoaiKhamCLS = value; }
        }

        public int maBSYeuCau
        {
            get { return MaBSYeuCau; }
            set { MaBSYeuCau = value; }
        }

        public string tinhTrangThanhToan
        {
            get { return TinhTrangThanhToan; }
            set { TinhTrangThanhToan = value; }
        }
    }
}
