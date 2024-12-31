using System;
using System.Collections.Generic;

namespace MarketYonetimSistemi
{
    
    public abstract class Kisi
    {
        public string Ad { get; set; }
        public string Iletisim { get; set; }

        protected Kisi(string ad, string iletisim)
        {
            Ad = ad;
            Iletisim = iletisim;
        }

        public abstract void BilgiGoster();
    }

    
    public abstract class Musteri : Kisi
    {
        public string MusteriId { get; set; }

        protected Musteri(string ad, string iletisim, string musteriId) : base(ad, iletisim)
        {
            MusteriId = musteriId;
        }
    }

 
    public class BireyselMusteri : Musteri
    {
        public string KimlikNo { get; set; }

        public BireyselMusteri(string ad, string iletisim, string musteriId, string kimlikNo)
            : base(ad, iletisim, musteriId)
        {
            KimlikNo = kimlikNo;
        }

        public override void BilgiGoster()
        {
            Console.WriteLine("Bireysel Müşteri: "+Ad+" İletişim: "+Iletisim+" ID: "+MusteriId+" Kimlik No: "+KimlikNo);
        }
    }

  
    public class KurumsalMusteri : Musteri
    {
        public string VergiNo { get; set; }

        public KurumsalMusteri(string ad, string iletisim, string musteriId, string vergiNo)
            : base(ad, iletisim, musteriId)
        {
            VergiNo = vergiNo;
        }

        public override void BilgiGoster()
        {
            Console.WriteLine("Kurumsal Müşteri: "+Ad+" İletişim: "+Iletisim+" ID: "+MusteriId+" Vergi No: "+VergiNo);
        }
    }

 
    public abstract class Odeme
    {
        public decimal Tutar { get; set; }

        protected Odeme(decimal tutar)
        {
            Tutar = tutar;
        }

        public abstract void OdemeYap();
    }

    // Ödeme yöntemleri
    public class KrediKartiOdeme : Odeme
    {
        public string KartNumarasi { get; set; }

        public KrediKartiOdeme(decimal tutar, string kartNumarasi) : base(tutar)
        {
            KartNumarasi = kartNumarasi;
        }

        public override void OdemeYap()
        {
            Console.WriteLine(Tutar+" tutarındaki kredi kartı ödemesi işleniyor. Kart Numarası: "+KartNumarasi);
        }
    }

    public class NakitOdeme : Odeme
    {
        public NakitOdeme(decimal tutar) : base(tutar) { }

        public override void OdemeYap()
        {
            Console.WriteLine(Tutar+" tutarındaki nakit ödeme işleniyor.");
        }
    }

    public class HavaleOdeme : Odeme
    {
        public string BankaHesabi { get; set; }

        public HavaleOdeme(decimal tutar, string bankaHesabi) : base(tutar)
        {
            BankaHesabi = bankaHesabi;
        }

        public override void OdemeYap()
        {
            Console.WriteLine(Tutar +" tutarındaki havale ödemesi "+ BankaHesabi+" hesabına işleniyor.");
        }
    }

    // Sipariş durumu enum
    public enum SiparisDurumu
    {
        Onaylandi,
        Hazirlaniyor,
        TeslimEdildi
    }

   
    public class Siparis
    {
        public string SiparisId { get; set; }
        public List<string> Urunler { get; set; } = new List<string>();
        public SiparisDurumu Durum { get; set; }

        public Siparis(string siparisId)
        {
            SiparisId = siparisId;
            Durum = SiparisDurumu.Onaylandi;
        }

        public void DurumGuncelle(SiparisDurumu durum)
        {
            Durum = durum;
            Console.WriteLine(SiparisId +" numaralı sipariş durumu "+Durum+" olarak güncellendi.");
        }
    }


    public abstract class Indirim
    {
        public string IndirimAdi { get; set; }

        protected Indirim(string indirimAdi)
        {
            IndirimAdi = indirimAdi;
        }

        public abstract decimal IndirimUygula(decimal tutar);
    }

    public class YuzdelikIndirim : Indirim
    {
        public decimal Yuzde { get; set; }

        public YuzdelikIndirim(string indirimAdi, decimal yuzde) : base(indirimAdi)
        {
            Yuzde = yuzde;
        }

        public override decimal IndirimUygula(decimal tutar)
        {
            return tutar - (tutar * Yuzde / 100);
        }
    }

    public class SabitIndirim : Indirim
    {
        public decimal SabitTutar { get; set; }

        public SabitIndirim(string indirimAdi, decimal sabitTutar) : base(indirimAdi)
        {
            SabitTutar = sabitTutar;
        }

        public override decimal IndirimUygula(decimal tutar)
        {
            return tutar - SabitTutar;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Müşteri oluşturma
                var bireysel = new BireyselMusteri("Zeynep Ece", "123456789", "M001", "12345678901");
                var kurumsal = new KurumsalMusteri("Yünkül Ltd.", "987654321", "M002", "9876543210");

                bireysel.BilgiGoster();
                kurumsal.BilgiGoster();

                // Ödeme işlemleri
                Odeme odeme1 = new KrediKartiOdeme(500, "4111111111111111");
                Odeme odeme2 = new NakitOdeme(200);

                odeme1.OdemeYap();
                odeme2.OdemeYap();

                // Sipariş oluşturma
                var siparis = new Siparis("S001");
                siparis.Urunler.Add("Elma");
                siparis.Urunler.Add("Muz");

                Console.WriteLine(siparis.SiparisId+" numaralı sipariş "+siparis.Urunler.Count +" ürünle oluşturuldu.");

                siparis.DurumGuncelle(SiparisDurumu.Hazirlaniyor);

                // İndirim uygulama
                Indirim indirim1 = new YuzdelikIndirim("Yaz İndirimi", 10);
                Indirim indirim2 = new SabitIndirim("Sadakat İndirimi", 50);

                decimal toplam = 1000;
                Console.WriteLine("Orijinal toplam:"+toplam);

                toplam = indirim1.IndirimUygula(toplam);
                Console.WriteLine("Yüzdelik indirim sonrası: "+toplam);

                toplam = indirim2.IndirimUygula(toplam);
                Console.WriteLine("Sabit indirim sonrası: "+toplam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: "+ex.Message);
            }
            Console.ReadLine();
        }
    }
}
