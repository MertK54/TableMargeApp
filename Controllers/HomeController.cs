using Microsoft.AspNetCore.Mvc;
using TableMargeApp.Models;
using TableMargeApp.ViewModels;
namespace TableMargeApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.TableTitle = "Üretim Operasyon Bildirimleri";

        // Üretim tablosu (Table1)
        List<Table1> table1List = new List<Table1>
        {
            new Table1 { kayitNo = 1, baslangic = new DateTime(2020, 05, 23, 7, 30, 0), bitis = new DateTime(2020, 05, 23, 8, 30, 0), toplamSure = new TimeSpan(1, 0, 0), statu = "URETIM", durusNedeni = "" },
            new Table1 { kayitNo = 2, baslangic = new DateTime(2020, 05, 23, 8, 30, 0), bitis = new DateTime(2020, 05, 23, 12, 0, 0), toplamSure = new TimeSpan(3, 30, 0), statu = "URETIM", durusNedeni = "" },
            new Table1 { kayitNo = 3, baslangic = new DateTime(2020, 05, 23, 12, 0, 0), bitis = new DateTime(2020, 05, 23, 13, 0, 0), toplamSure = new TimeSpan(1, 0, 0), statu = "URETIM", durusNedeni = "" },
            new Table1 { kayitNo = 4, baslangic = new DateTime(2020, 05, 23, 13, 0, 0), bitis = new DateTime(2020, 05, 23, 13, 45, 0), toplamSure = new TimeSpan(0, 45, 0), statu = "DURUS", durusNedeni = "ARIZA" },
            new Table1 { kayitNo = 5, baslangic = new DateTime(2020, 05, 23, 13, 45, 0), bitis = new DateTime(2020, 05, 23, 17, 30, 0), toplamSure = new TimeSpan(3, 45, 0), statu = "URETIM", durusNedeni = "" },
        };

        // Standart duruşlar tablosu (Table2)
        List<Table2> table2List = new List<Table2>
        {
            new Table2 { baslangic = new DateTime(2020, 05, 23, 10, 0, 0), bitis = new DateTime(2020, 05, 23, 10, 15, 0), durusNedeni = "Çay Molası" },
            new Table2 { baslangic = new DateTime(2020, 05, 23, 12, 0, 0), bitis = new DateTime(2020, 05, 23, 12, 30, 0), durusNedeni = "Yemek Molası" },
            new Table2 { baslangic = new DateTime(2020, 05, 23, 15, 0, 0), bitis = new DateTime(2020, 05, 23, 15, 15, 0), durusNedeni = "Çay Molası" },
        };

        // Üretim ve duruş tablolarını birleştirme (Table3)
        List<Table3> table3List = new List<Table3>();
        foreach (var row in table1List)
        {
            bool check = false;
            foreach (var row2 in table2List)
            {
                if (row.baslangic <= row2.baslangic && row.bitis >= row2.bitis)
                {
                    if (row.baslangic != row2.baslangic)
                    {
                        /* eğer duruş zamanı yani molalar üretim süresi içindeyse üretimden molaya kadar olan kısım */
                        /* bu kontrol olmadığında başlangıç 12.00 bitiş 12.00 olan satır oluyor. bu kontrol ile eşitliği denetledim.  */
                        table3List.Add(new Table3 { kayitNo = row.kayitNo, baslangic = row.baslangic, bitis = row2.baslangic, toplamSure = row2.baslangic - row.baslangic, statu = row.statu, durusNedeni = row.durusNedeni });
                    }

                    table3List.Add(new Table3 { kayitNo = row.kayitNo, baslangic = row2.baslangic, bitis = row2.bitis, toplamSure = row2.bitis - row2.baslangic, statu = "DURUS", durusNedeni = row2.durusNedeni });
                    /* burası da mola kısmı */

                    if (row2.bitis != row.bitis)
                    {
                        /* duruş yani mola bitiminden üretimin bitişine kadar olan kısım */
                        table3List.Add(new Table3 { kayitNo = row.kayitNo, baslangic = row2.bitis, bitis = row.bitis, toplamSure = row.bitis - row2.bitis, statu = row.statu, durusNedeni = row.durusNedeni });
                    }

                    check = true;
                    break;
                }
            }
            if (!check)
            {
                /* eğer duruş yani mola yoksa üretimi olduğu gibi ekle */
                table3List.Add(new Table3 { kayitNo = row.kayitNo, baslangic = row.baslangic, bitis = row.bitis, toplamSure = row.bitis - row.baslangic, statu = row.statu, durusNedeni = row.durusNedeni });
            }
        }
        TablesViewModel viewModel = new TablesViewModel
        {
            Table1List = table1List,
            Table2List = table2List,
            Table3List = table3List
        };
        return View(viewModel);
    }

}
