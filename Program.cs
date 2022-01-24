using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateData
{
    class Program
    {
        static void Main(string[] args)
        {
            StockManager stockManager = new StockManager();
            DataManager dataMgr = new DataManager();
            char menustr = '4';
            if ((args.Length == 0) || ((args[0].Equals("1") == false) && (args[0].Equals("2") == false) && (args[0].Equals("3") == false) && (args[0].Equals("4") == false)))
            {
                Console.WriteLine("Select Menu:");
                Console.WriteLine("1. Background update of NAV");
                Console.WriteLine("2. Background refresh NSE stock master");
                Console.WriteLine("3. Background update ALL stock price");
                Console.WriteLine("4. to exit");

                Console.Write("Enter your choice: ");
                menustr = Console.ReadKey().KeyChar; //ReadLine();
                Console.WriteLine();
            }
            else
            {
                menustr = args[0].First();
            }
            do
            {
                if (menustr == '1')
                {
                    //update NAV records
                    try
                    {
                        dataMgr.UpdateNAVFromLastFetchDate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("UpdateNAVFromLastFetchDate :" + ex.Message);
                    }
                }
                else if (menustr == '2')
                {
                    //this will get latest stock symbols for NSE only
                    try
                    {
                        stockManager.FetchStockMasterFromWebAndInsert();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("FetchStockMasterFromWebAndInsert :" + ex.Message);
                    }
                }
                else if (menustr == '3')
                {
                    //update stock price data for all existing symbols
                    try
                    {
                        DataTable stockMaster = stockManager.getStockMaster();
                        if ((stockMaster != null) && (stockMaster.Rows.Count > 0))
                        {
                            foreach (DataRow stockrow in stockMaster.Rows)
                            {
                                stockManager.FetchOnlineAndSaveDailyStockData(stockrow["SYMBOL"].ToString());
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("UpdateStockPriceData :" + ex.Message);
                    }
                }

                if ( (menustr != '4') && 
                    ((args.Length == 0) || 
                        ((args[0].Equals("1") == false) && (args[0].Equals("2") == false) && (args[0].Equals("3") == false) && (args[0].Equals("4") == false))))
                {
                    Console.WriteLine("Select Menu:");
                    Console.WriteLine("1. Background update of NAV");
                    Console.WriteLine("2. Background refresh NSE stock master");
                    Console.WriteLine("3. Background update ALL stock price");
                    Console.WriteLine("4. to exit");

                    Console.Write("Enter your choice: ");
                    menustr = Console.ReadKey().KeyChar; //ReadLine();
                }
                else
                {
                    menustr = '4';
                }

            } while (menustr != '4');
        }
    }
}
