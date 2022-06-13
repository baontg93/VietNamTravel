using System.Collections.Generic;
public static class ProvincesParser
{
    static readonly Dictionary<string[], string> provinces = new()
    {
        { new string[] { "AnGiang", "An Giang", "an_giang" }, "An Giang" },
        { new string[] { "BaRiaVungTau", "Ba Ria – Vung Tau", "ba_ria_vung_tau" }, "Bà Rịa – Vũng Tàu" },
        { new string[] { "BacLieu", "Bac Lieu", "bac_lieu" }, "Bạc Liêu" },
        { new string[] { "BacGiang", "Bac Giang", "bac_giang" }, "Bắc Giang" },
        { new string[] { "BacKan", "Bac Kan", "bac_kan" }, "Bắc Kạn" },
        { new string[] { "BacNinh", "Bac Ninh", "bac_ninh" }, "Bắc Ninh" },
        { new string[] { "BenTre", "Ben Tre", "ben_tre" }, "Bến Tre" },
        { new string[] { "BinhDuong", "Binh Duong", "binh_duong" }, "Bình Dương" },
        { new string[] { "BinhDinh", "Binh Dinh", "binh_dinh" }, "Bình Định" },
        { new string[] { "BinhPhuoc", "Binh Phuoc", "binh_phuoc" }, "Bình Phước" },
        { new string[] { "BinhThuan", "Binh Thuan", "binh_thuan" }, "Bình Thuận" },
        { new string[] { "CaMau", "Ca Mau", "ca_mau" }, "Cà Mau" },
        { new string[] { "CaoBang", "Cao Bang", "cao_bang" }, "Cao Bằng" },
        { new string[] { "CanTho", "Can Tho", "can_tho" }, "Cần Thơ" },
        { new string[] { "DaNang", "Da Nang", "da_nang" }, "Đà Nẵng" },
        { new string[] { "DakLak", "Dak Lak", "dak_lak" }, "Đắk Lắk" },
        { new string[] { "DakNong", "Dak Nong", "dak_nong" }, "Đắk Nông" },
        { new string[] { "DienBien", "Dien Bien", "dien_bien" }, "Điện Biên" },
        { new string[] { "DongNai", "Dong Nai", "dong_nai" }, "Đồng Nai" },
        { new string[] { "DongThap", "Dong Thap", "dong_thap" }, "Đồng Tháp" },
        { new string[] { "GiaLai", "Gia Lai", "gia_lai" }, "Gia Lai" },
        { new string[] { "HaGiang", "Ha Giang", "ha_giang" }, "Hà Giang" },
        { new string[] { "HaNam", "Ha Nam", "ha_nam" }, "Hà Nam" },
        { new string[] { "HaNoi", "Hanoi", "ha noi" }, "Hà Nội" },
        { new string[] { "HaTinh", "Ha Tinh", "ha_tinh" }, "Hà Tĩnh" },
        { new string[] { "HaiDuong", "Hai Duong", "hai_duong" }, "Hải Dương" },
        { new string[] { "HaiPhong", "Hai Phong", "hai_phong" }, "Hải Phòng" },
        { new string[] { "HauGiang", "Hau Giang", "hau_giang" }, "Hậu Giang" },
        { new string[] { "HoaBinh", "Hoa Binh", "hoa_binh" }, "Hòa Bình" },
        { new string[] { "HungYen", "Hung Yen", "hung_yen" }, "Thành phố Hồ Chí Minh" },
        { new string[] { "KhanhHoa", "Khanh Hoa", "khanh_hoa" }, "Hưng Yên" },
        { new string[] { "KienGiang", "Kien Giang", "kien_giang" }, "Khánh Hòa" },
        { new string[] { "KonTum", "Kon Tum", "kon_tum" }, "Kiên Giang" },
        { new string[] { "LaiChau", "Lai Chau", "lai_chau" }, "Kon Tum" },
        { new string[] { "LangSon", "Lang Son", "lang_son" }, "Lai Châu" },
        { new string[] { "LaoCai", "Lao Cai", "lao_cai" }, "Lạng Sơn" },
        { new string[] { "LamDong", "Lam Dong", "lam_dong" }, "Lào Cai" },
        { new string[] { "LongAn", "Long An", "long_an" }, "Lâm Đồng" },
        { new string[] { "NamDinh", "Nam Dinh", "nam_dinh" }, "Long An" },
        { new string[] { "NgheAn", "Nghe An", "nghe_an" }, "Nam Định" },
        { new string[] { "NinhBinh", "Ninh Binh", "ninh_binh" }, "Nghệ An" },
        { new string[] { "NinhThuan", "Ninh Thuan", "ninh_thuan" }, "Ninh Bình" },
        { new string[] { "PhuTho", "Phu Tho", "phu_tho" }, "Ninh Thuận" },
        { new string[] { "PhuYen", "Phu Yen", "phu_yen" }, "Phú Thọ" },
        { new string[] { "QuangBinh", "Quang Binh", "quang_binh" }, "Phú Yên" },
        { new string[] { "QuangNam", "Quang Nam", "quang_nam" }, "Quảng Bình" },
        { new string[] { "QuangNgai", "Quang Ngai", "quang_ngai" }, "Quảng Nam" },
        { new string[] { "QuangNinh", "Quang Ninh", "quang_ninh" }, "Quảng Ngãi" },
        { new string[] { "QuangTri", "Quang Tri", "quang_tri" }, "Quảng Ninh" },
        { new string[] { "SocTrang", "Soc Trang", "soc_trang" }, "Quảng Trị" },
        { new string[] { "SonLa", "Son La", "son_la" }, "Sóc Trăng" },
        { new string[] { "TayNinh", "Tay Ninh", "tay_ninh" }, "Sơn La" },
        { new string[] { "ThaiBinh", "Thai Binh", "thai_binh" }, "Tây Ninh" },
        { new string[] { "ThaiNguyen", "Thai Nguyen", "thai_nguyen" }, "Thái Bình" },
        { new string[] { "ThanhHoa", "Thanh Hoa", "thanh_hoa" }, "Thái Nguyên" },
        { new string[] { "HoChiMinhCity", "Ho Chi Minh city", "ho_chi_minh_city" }, "Thanh Hóa" },
        { new string[] { "ThuaThienHue", "Thua Thien Hue", "thua_thien_hue" }, "Thừa Thiên Huế" },
        { new string[] { "TienGiang", "Tien Giang", "tien_giang" }, "Tiền Giang" },
        { new string[] { "TraVinh", "Tra Vinh", "tra_vinh" }, "Trà Vinh" },
        { new string[] { "TuyenQuang", "Tuyen Quang", "tuyen_quang" }, "Tuyên Quang" },
        { new string[] { "VinhLong", "Vinh Long", "vinh_long" }, "Vĩnh Long" },
        { new string[] { "VinhPhuc", "Vinh Phuc", "vinh_phuc" }, "Vĩnh Phúc" },
        { new string[] { "YenBai", "Yen Bai", "yen_bai" }, "Yên Bái" },
    };

    public static string GetProvince(string address)
    {
        address = address.ToLower();
        foreach (var item in provinces)
        {
            string[] keys = item.Key;
            for (int i = 0; i < keys.Length; i++)
            {
                if (address.Contains(keys[i].ToLower()))
                {
                    return item.Value;
                }
            }
        }
        return "";
    }
}
