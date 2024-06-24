using Domain.Entities;
using Domain.ValueObjects;

namespace Test.Domain.Entities;

public sealed class CaseTest
{
    // [Test]
    // [Parallelizable]
    // public void TestCaseCreation()
    // {
    //     var @case = Case.CreateNew("test", "",
    //     [
    //         new CaseDrop { DropChance = 0.1m, DropPrice = 14.08m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 6.96m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 6.73m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 0.72m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.3m, DropPrice = 0.03m, ItemType = null, ItemTypeId = default },
    //     ],
    //         DateTime.UtcNow,
    //         "system");
    //
    //     Console.WriteLine(@case.Price);
    //
    //     var drop = @case.Open();
    //     Console.WriteLine(drop);
    // }
    
    // test house edge
    // [Test]
    // [Parallelizable]
    // public void TestHouseEdge()
    // {
    //     var @case = Case.CreateNew("test", "",
    //     [
    //         new CaseDrop { DropChance = 0.1m, DropPrice = 14.08m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 6.96m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 6.73m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.2m, DropPrice = 0.72m, ItemType = null, ItemTypeId = default },
    //         new CaseDrop { DropChance = 0.3m, DropPrice = 0.03m, ItemType = null, ItemTypeId = default },
    //     ],
    //         DateTime.UtcNow,
    //         "system");
    //
    //     var price = @case.Price;
    //     List<decimal> draws = [];
    //
    //     for (var i = 0; i < 100_000; i++)
    //     {
    //         var drop = @case.Open();
    //         draws.Add(drop.DropPrice);
    //     }
    //
    //     var average = draws.Average();
    //     var winCount = draws.Count(x => x > price);
    //     var winCountPercentage = (decimal)winCount / draws.Count * 100;
    //
    //     Console.Out.WriteLine("@case.WinRate = {0}", @case.WinRate);
    //     Console.Out.WriteLine("@case.Roi = {0}", @case.Roi);
    //
    //
    //     Console.Out.WriteLine("winCount = {0} / {1}", winCount, draws.Count);
    //     Console.Out.WriteLine("winCount% = {0}%", winCountPercentage);
    //
    //     Console.Out.WriteLine("average = {0}", average);
    //     Console.Out.WriteLine("price = {0}", price);
    //     Console.Out.WriteLine("house edge = {0}", Case.HouseEdge);
    //     System.Console.Out.WriteLine("house kept = {0}", price - average);
    // }
}