using NUnit.Framework;
using System;

namespace MetaStockDb.Test
{
    [TestFixture]
    public class PremiumDataDbTests
    {
        [Test]
        public void ReadData_using_PremiumData_revEngineered_code()
        {
            //MsMkt msMkt = new MsMkt(0, @"c:\ztg\db\PremiumDatax\Stocks\US\NASDAQ\S\F76.dat", "STX");
            //msMkt.Load();
            //if (msMkt.Count > 0)
            //{
            //}
        }

        [Test]
        public void ReadData()
        {
            var db = new PremiumDataDb(@"c:\ZTG\DB\PremiumData");
            db.LoadSymbolTable();
            Console.WriteLine(db.Count);
            //Assert.IsTrue(db.Count > 0);

            var dataFile = db.LoadBars("STX");
            //Assert.IsTrue(dataFile.Records.Count > 0);
        }
    }
}