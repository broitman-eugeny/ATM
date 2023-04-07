using System;

//Константы приложения Банкомат 
class ConstantsATM
{
    //Количество достоинств купюр
    public const int NumOfDignities=6;
    //Массив значений номиналов
    public static int[] ValuesOfDignities = {10, 50, 100, 500, 1000, 5000};
    //Максимальное количество купюр каждого достоинства, помещаемое в банкомат
    public static int[] MaxBillsInATM = {1000, 500, 400, 300, 200, 100};
}
