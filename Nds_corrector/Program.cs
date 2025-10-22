class Program
{
    static void Main()
    {
        Console.WriteLine("Автоматические тесты:");
        Test1(1.81m, 20, "Тест 1");
        Test1(1.81m, 18, "Тест 2");
        Console.WriteLine();
        
        /*Console.WriteLine("Введите цену и процент ндс \nЦена: ");
        int priceWithNds =  Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Процент НДС: ");
        int procNds =  Convert.ToInt32(Console.ReadLine());
        
        CalcPrices(priceWithNds, procNds, out decimal with, out decimal without);
        Console.WriteLine($"CorrectPriceWithNds = {with:F2}, CorrectedPriceWithoutNds = {without:F2}");*/
        
        while (true)
        {
            Console.WriteLine("\n=== Ручной ввод данных ===");
            
            decimal inputPriceWithNds;
            while (true)
            {
                Console.Write("Введите рекомендованную цену с НДС (или 'exit' для выхода): ");
                string input = Console.ReadLine();
                
                if (input?.ToLower() == "exit")
                {
                    Console.WriteLine("Программа завершена.");
                    return;
                }
                
                if (decimal.TryParse(input, out inputPriceWithNds) && inputPriceWithNds >= 0)
                {
                    break;
                }
                
                Console.WriteLine("Ошибка: введите положительное число (например, 1.81)");
            }
            
            int procNds;
            while (true)
            {
                Console.Write("Введите процент НДС (0-99): ");
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out procNds) && procNds >= 0 && procNds <= 99)
                {
                    break;
                }
                
                Console.WriteLine("Ошибка: введите целое число от 0 до 99");
            }
            try
            {
                CalcPrices(inputPriceWithNds, procNds, out decimal correctedWith, out decimal correctedWithout);
                
                Console.WriteLine("\nРезультат: ");
                Console.WriteLine($"Входная цена с НДС: {inputPriceWithNds}");
                Console.WriteLine($"Процент НДС:    {procNds}%");
                Console.WriteLine($"Скорректированная с НДС:    {correctedWith:F2}");
                Console.WriteLine($"Скорректированная без НДС:  {correctedWithout:F2}");
                Console.WriteLine($"Отклонение: {Math.Abs(correctedWith - inputPriceWithNds):F20}");
                
                decimal check = correctedWithout * (1 + procNds / 100m);
                Console.WriteLine($"Проверка формулы:   {check:F2} (должно = {correctedWith:F2})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка расчета: {ex.Message}");
            }
        }
        
        
    }
    static void Test1(decimal priceWithNds, int procNds, string testName)
    {
        try
        {
            CalcPrices(priceWithNds, procNds, out decimal correctedWith, out decimal correctedWithout);
            Console.WriteLine($"{testName}: InputPrice={priceWithNds}, НДС={procNds}%");
            Console.WriteLine($"  CorrectedPriceWithNDS    = {correctedWith:F2}");
            Console.WriteLine($"  CorrectedPriceWithoutNDS = {correctedWithout:F2}");
            Console.WriteLine($"  Отклонение = {Math.Abs(correctedWith - priceWithNds):F20}");
            
            // Проверка формулы
            decimal check = correctedWithout * (1 + procNds / 100m);
            Console.WriteLine($"  Проверка: {correctedWithout:F2} × (1 + {procNds}/100) = {check:F2}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{testName} провалился: {ex.Message}\n");
        }
    }
    
    static void CalcPrices(decimal priceWithNds, int procNds, out  decimal CorrectWith, out decimal CorrectedWithout)
    {
        if(priceWithNds < 0) throw new ArgumentOutOfRangeException("priceWithNds");
        if(procNds < 0 || procNds > 99) throw new ArgumentOutOfRangeException("procNds");
        
        int commonNum = 100 + procNds;
        int a = Common(commonNum, 100);
        int b = commonNum / a;
        int step = 100 / a;

        decimal recommnderWithCent = priceWithNds * 100m;
        
        decimal fractionalK =  recommnderWithCent / b;
        long kCenter = (long)fractionalK;
        if (kCenter < 0 ) kCenter = 0;
        
        long bestK = kCenter;
        decimal besdiff = decimal.MaxValue;

        for (long k1 = Math.Max(0, kCenter - 1); k1 <= kCenter + 1; k1++)
        {
            decimal withCents = k1 * (decimal)b;
            decimal withPrice = withCents / 100m;
            decimal diff =  Math.Abs(withPrice - priceWithNds);

            if (diff < besdiff)
            {
                besdiff = diff;
                bestK = k1;
            }
        }
        
        long centFinal = bestK * b;
        long withoutCentFinal = bestK * step;
        
        CorrectWith = centFinal / 100m;
        CorrectedWithout = withoutCentFinal / 100m;

    }
    
    static int Common(int a, int b)
    {
        while (b != 0)
        {
            int c  = a % b;
            a = b;
            b = c;
        }
        if(a == 0) return 1;
        else return a;
    }
}