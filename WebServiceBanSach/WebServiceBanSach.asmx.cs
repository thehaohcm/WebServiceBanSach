using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;

namespace WebServiceBanSach
{
    /// <summary>
    /// Summary description for WebServiceBanSach
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceBanSach : System.Web.Services.WebService
    {

        DataClassesBanSachDataContext db = new DataClassesBanSachDataContext();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListSach()// List<SACH> getListSach()
        {
            List<SACH> lsSach = db.SACHes.ToList();
            if (lsSach != null)
            {
                foreach (SACH s in lsSach)
                {
                    s.CTDHs.Clear();
                    s.NHANXETs.Clear();
                    s.CTTLs.Clear();
                }
            }
            //return lsSach;
            return new JavaScriptSerializer().Serialize(lsSach);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTheLoaibyMaSach(int masach) //List<THELOAI> getTheLoaibyMaSach(int masach)
        {
            List<CTTL> lsCTTL = db.CTTLs.Where(x => x.MaSach == masach).ToList();
            List<THELOAI> lsTheLoai = new List<THELOAI>();
            foreach (CTTL c in lsCTTL)
            {
                List<THELOAI> lst = db.THELOAIs.Where(y => y.MaTL == c.MaTL).ToList();
                foreach (THELOAI tl in lst)
                {
                    tl.CTTLs.Clear();
                    lsTheLoai.Add(tl);
                }
            }
            //return lsTheLoai;
            return new JavaScriptSerializer().Serialize(lsTheLoai);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListTheLoai()//List<THELOAI> getallTheLoai()
        {
            List<THELOAI> lsTheLoai = new List<THELOAI>();
            lsTheLoai = db.THELOAIs.ToList();
            foreach (THELOAI tl in lsTheLoai)
            {
                tl.CTTLs.Clear();
            }
            //return lsTheLoai;
            return new JavaScriptSerializer().Serialize(lsTheLoai);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getSachbyMaSach(int id)//SACH getSachbyMaSach(int id)
        {
            SACH s = db.SACHes.FirstOrDefault(x => x.MaSach == id);

            s.CTTLs.Clear();
            s.CTDHs.Clear();
            s.NHANXETs.Clear();
            //return s;
            return new JavaScriptSerializer().Serialize(s);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListNhanXetbyMaSach(int id)//List<NHANXET> getListNhanXetbyMaSach(int id)
        {
            List<NHANXET> ls = db.NHANXETs.Where(x => x.MaSach == id).ToList();
            if (ls != null)
                foreach (NHANXET nx in ls)
                    nx.SACH = null;
            //return ls;
            return new JavaScriptSerializer().Serialize(ls);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListDonHang()//List<DONHANG> getListDonHang()
        {
            List<DONHANG> lsDonHang = db.DONHANGs.ToList();
            if (lsDonHang != null)
            {
                foreach (DONHANG dh in lsDonHang)
                {
                    dh.CTDHs.Clear();
                }
            }
            //return lsDonHang;
            return new JavaScriptSerializer().Serialize(lsDonHang);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getDonHangbyMaDH(int id)//DONHANG getDonHangbyMaDH(int id)
        {
            DONHANG dh = db.DONHANGs.FirstOrDefault(x => x.MaDH == id);
            dh.CTDHs.Clear();
            return new JavaScriptSerializer().Serialize(dh);// dh;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getChiTietDonHangbyMaDH(int madh)//List<CTDH> getChiTietDonHangbyMaDH(int madh)
        {
            var kq = db.getCTDHbyMaDonHang(madh);
            return new JavaScriptSerializer().Serialize(kq);//ls;
        }

        [WebMethod]
        //[ScriptMethod(ResponseFormat=ResponseFormat.Json)]
        public List<CTDH> getListSachXML(int madh)
        {
            List<CTDH> ls = (from ct in db.CTDHs
                             select ct).ToList();
            return ls;//JavaScriptSerializer().Serialize(ls);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListTheLoaibyMaSach(int id)//List<THELOAI> getListTheLoaibyMaSach(int id)
        {
            List<THELOAI> ls = (from tl in db.THELOAIs
                                join ct in db.CTTLs on tl.MaTL equals ct.MaTL
                                join s in db.SACHes on ct.MaSach equals s.MaSach
                                where s.MaSach == id
                                select tl).ToList();
            foreach (THELOAI tl in ls)
            {
                tl.CTTLs.Clear();
            }
            //return ls;
            return new JavaScriptSerializer().Serialize(ls);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListSachbyTheLoai(int matl)
        {
            List<SACH> ls = (from s in db.SACHes
                             join ct in db.CTTLs on s.MaSach equals ct.MaSach
                             join tl in db.THELOAIs on ct.MaTL equals tl.MaTL
                             where tl.MaTL == matl
                             select s).ToList();

            foreach (SACH s in ls)
            {
                s.CTDHs.Clear();
                s.CTTLs.Clear();
                s.NHANXETs.Clear();
            }
            return new JavaScriptSerializer().Serialize(ls);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTongTienbyMaHang(decimal mahang) // decimal getTongTienbyMaHang(decimal mahang)
        {
            decimal tongtien = -1;
            try
            {
                tongtien = (from ct in db.CTDHs
                            join s in db.SACHes on ct.MaSach equals s.MaSach
                            where ct.MaDH == mahang && ct.MaSach == s.MaSach
                            select s.DonGia * ct.SoLuong).Sum();
                //return tongtien;
            }
            catch (Exception)
            {
                //return -1;
                //return null;
                tongtien = -1;
            }
            //return -1;
            return new JavaScriptSerializer().Serialize(tongtien);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTheLoaibyMaTheLoai(int matl)
        {
            THELOAI tl = db.THELOAIs.FirstOrDefault(x => x.MaTL == matl);
            tl.CTTLs.Clear();
            return new JavaScriptSerializer().Serialize(tl);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getListNhanXet()
        {
            List<NHANXET> ls = db.NHANXETs.ToList();
            foreach (NHANXET nx in ls)
            {
                nx.SACH = null;
            }
            return new JavaScriptSerializer().Serialize(ls);
        }

        //Them

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addSach(string tensach, string noidung, string hinhanh, decimal dongia, decimal slcl)// addSach(string tensach, string noidung, string hinhanh, decimal dongia, decimal slcl)
        {
            bool done = false;
            try
            {
                SACH s = new SACH();
                s.TenSach = tensach;
                s.NoiDung = noidung;
                s.HinhAnh = hinhanh;
                s.DonGia = dongia;
                s.SLCL = slcl;
                db.SACHes.InsertOnSubmit(s);
                db.SubmitChanges();
                //return true;
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
            }
            //return false;
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addTheLoai(string tentl)
        {
            bool done;
            try
            {
                db.THELOAIs.InsertOnSubmit(new THELOAI { TenTL = tentl });
                db.SubmitChanges();
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addTheLoaiSach(decimal masach, byte matl)
        {
            bool done = false;
            try
            {
                CTTL ct = db.CTTLs.FirstOrDefault(x => x.MaSach == masach && x.MaTL == matl) ?? null;
                if (ct == null)
                {
                    db.CTTLs.InsertOnSubmit(new CTTL { MaSach = masach, MaTL = matl });
                    db.SubmitChanges();
                    done = true;
                }
                else
                    done = false;
            }
            catch (Exception ex)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addNhanXet(int masach, string hoten, string tieude, string noidung, byte danhgia)//bool addNhanXet(int masach, string hoten, string tieude, string noidung, byte danhgia)
        {
            bool done;
            try
            {
                db.NHANXETs.InsertOnSubmit(new NHANXET { MaSach = masach, HoTen = hoten, TieuDe = tieude, NoiDung = noidung, DanhGia = danhgia });
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            //return false;
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addDonHang(string ngaydathang, string diachi, decimal sodt)//bool addDonHang(DateTime ngaydathang, string diachi, decimal sodt)
        {
            decimal done;
            try
            {
                DateTime ngaydathang_format = DateTime.Parse(ngaydathang);
                DONHANG dh = new DONHANG { NgayDatHang = ngaydathang_format, DiaChi = diachi, SoDT = sodt };
                db.DONHANGs.InsertOnSubmit(dh);
                db.SubmitChanges();
                done = dh.MaDH;
            }
            catch (Exception)
            {
                done = -1;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string addCTDH(int madh, int masach, int soluong)//bool addCTDH(int madh, int masach, int soluong)
        {
            bool done;
            try
            {
                SACH s = new SACH();
                s = db.SACHes.FirstOrDefault(x => x.MaSach == masach);
                if (s.SLCL > 0)
                {
                    s.SLCL -= soluong;
                }
                else
                {
                    return new JavaScriptSerializer().Serialize(false);
                }
                db.CTDHs.InsertOnSubmit(new CTDH { MaDH = madh, MaSach = masach, SoLuong = soluong });
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        //Edit
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string editSach(int id, string tensach, string noidung, string hinhanh, decimal dongia, decimal slcl)
        {
            bool done;
            try
            {
                SACH sach = (from s in db.SACHes
                             where s.MaSach == id
                             select s).First();
                sach.TenSach = tensach;
                sach.NoiDung = noidung;
                sach.HinhAnh = hinhanh;
                sach.DonGia = dongia;
                sach.SLCL = slcl;
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {

                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string editTheLoai(int matl, string tentl)
        {
            bool done;
            try
            {
                THELOAI tl = db.THELOAIs.First(x => x.MaTL == matl);
                tl.TenTL = tentl;
                db.SubmitChanges();
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string editDonHang(int madh, DateTime ngaydathang, string diachi, decimal sodt)
        {
            bool done;
            try
            {
                DONHANG tl = db.DONHANGs.First(x => x.MaDH == madh);
                tl.NgayDatHang = ngaydathang;
                tl.DiaChi = diachi;
                tl.SoDT = sodt;
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string editChiTietDonHang(decimal madonhang, decimal masach, decimal soluong)
        {
            bool done;
            try
            {
                CTDH ct = db.CTDHs.First(x => (x.MaDH == madonhang && x.MaSach == masach));
                ct.SoLuong = soluong;
                db.CTDHs.InsertOnSubmit(ct);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        //Delete
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteSach(int id)
        {
            bool done;
            try
            {
                SACH s = db.SACHes.First(x => x.MaSach == id);
                db.SACHes.DeleteOnSubmit(s);
                db.SubmitChanges();
                done = true;

            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteDonHang(int id)
        {
            bool done;
            try
            {
                DONHANG d = db.DONHANGs.First(x => x.MaDH == id);
                db.DONHANGs.DeleteOnSubmit(d);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {

                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteNhanXet(int id)
        {
            bool done;
            try
            {
                NHANXET n = db.NHANXETs.First(x => x.MaNX == id);
                db.NHANXETs.DeleteOnSubmit(n);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteTheLoai(int id)
        {
            bool done;
            try
            {
                THELOAI tl = db.THELOAIs.First(x => x.MaTL == id);
                //tl.CTTLs.Clear();
                db.THELOAIs.DeleteOnSubmit(tl);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteTheLoaiSach(decimal masach, byte matl)
        {
            bool done;
            try
            {
                CTTL ct = db.CTTLs.FirstOrDefault(x => x.MaSach == masach && x.MaTL == matl);
                db.CTTLs.DeleteOnSubmit(ct);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception ex)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string deleteCTTLSach(int masach, int matheloai)
        {
            bool done;
            try
            {
                CTTL ct = db.CTTLs.First(x => x.MaSach == masach && x.MaTL == matheloai);
                db.CTTLs.DeleteOnSubmit(ct);
                db.SubmitChanges();
                done = true;
            }
            catch (Exception)
            {
                done = false;
            }
            return new JavaScriptSerializer().Serialize(done);
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string searchSachs(string str)
        //{
        //    SACH s;
        //    try
        //    {
        //        s = db.SACHes.First(x=>x.TenSach );
        //    }
        //    catch (Exception)
        //    {
        //        s = null;
        //    }
        //    return new JavaScriptSerializer().Serialize(s);
        //}
    }
}
