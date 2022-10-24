namespace Test
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Reflection.Metadata.Ecma335;
  using System.Text;
  using System.Threading.Tasks;

  internal class MemoProp
  {
    ////  Task<MarketData> loadNyseData = Task<MarketData>.Factory.StartNew(
    //() => LoadNyseData(), TaskCreationOptions.LongRunning);

    //    Task<MarketData> loadNasdaqData = Task<MarketData>.Factory.StartNew(
    //        () => LoadNasdaqData(), TaskCreationOptions.LongRunning);

    //Task<MarketData> mergeMarketData =
    //    factory.ContinueWhenAll<MarketData, MarketData>(
    //        new[] { loadNyseData, loadNasdaqData },
    //        (tasks) => MergeMarketData(from t in tasks select t.Result));


    public async Task Testing()
    {
      var sw = Stopwatch.StartNew();
      sw.Start();

      // initial tasks
      var test1 = Test1();
      var test2 = Test2();
      var test3 = Test3();

      // dependent tasks
      var test5 = TaskDependent(test1, test3, "One and Three");
      var test4 = TaskDependent(test1, test2, "One and Two");
      var test6 = TaskDependent(test4, test3, "Dependent 4 and Three");

      Console.WriteLine("testing Start");

      // Wait for all
      await Task.WhenAll(new Task[] { test1, test2, test3, test4, test5, test6 }).ConfigureAwait(false);

      sw.Stop();

      Console.WriteLine($"test1 {test1.Result}");
      Console.WriteLine($"test2 {test2.Result}");
      Console.WriteLine($"test3 {test3.Result}");
      Console.WriteLine($"test4 {test4.Result}");
      Console.WriteLine($"test5 {test5.Result}");
      Console.WriteLine($"test6 {test6.Result}");

      Console.WriteLine($"Total time {sw.ElapsedMilliseconds}ms");
    }
    
    public async Task<int>TaskDependent(Task<int> task1, Task<int> task2, string message)
    {
      Console.WriteLine("TaskDependent Start: " + message);
      await Task.WhenAll(new[] { task1, task2 }).ConfigureAwait(false);
      
      var result1 = await task1;
      var result2 = await task2;

      var result = result1 + result2;

      Console.WriteLine($"TaskDependent End: {message} Result: {result}");
      return result;
    }

    public async Task<int> Test1()
    {
      
      Console.WriteLine("Test 1 Starts");
      await Task.Delay(300).ConfigureAwait(false);
      Console.WriteLine("Test 1 Ends");
      return 1;
    }

    public async Task<int> Test2()
    {
      Console.WriteLine("Test 2 Starts");
      await Task.Delay(400).ConfigureAwait(false);
      Console.WriteLine("Test 2 Ends");
      return 2;
    }


    public async Task<int> Test3()
    {
      Console.WriteLine("Test 3 Starts");
      await Task.Delay(700).ConfigureAwait(false);
      Console.WriteLine("Test 3 Ends");
      return 3;
    }
  }
}
