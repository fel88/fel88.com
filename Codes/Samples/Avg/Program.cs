using System.Text;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Введите N: ");
int N = int.Parse(Console.ReadLine());

int[] data = new int[N]; // создаем массив чисел длиной N, где будем запоминать введенные пользователм числа

// считываем N чисел
for (int i = 0; i < N; i++)
{
    data[i] = int.Parse(Console.ReadLine());     // записываем очередное число в массив
}

// расчитаем среднее пройдясь по массиву и накопив sum 
int sum = 0; // аккумулятор суммы
for (int i = 0; i < N; i++)
{
    sum += data[i];
}

// вычислим среднее
double avg = sum / (double)N;
Console.WriteLine($"Среднее = {avg}");
//пройдемся еще раз по массиву и будем выводить числа, которые больше или равны среднему
Console.WriteLine("Числа больше среднего: ");
for (int i = 0; i < N; i++)
{
    if (data[i] >= avg)
        Console.WriteLine(data[i]);
}